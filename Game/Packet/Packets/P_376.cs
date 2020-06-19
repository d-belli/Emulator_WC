using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Move Item - size 20
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct P_376
    {
        public SHeader Header;  //0  a 11 = 12
        public byte TypeDest;
        public byte DestSlot;
        public byte TypeSrc;
        public byte SrcSlot;
        public int WarpID;

        public static P_376 New(Client client, P_376 p376)
        {
            P_376 tmp = new P_376
            {
                Header = SHeader.New(0x0376, Marshal.SizeOf<P_376>(), client.ClientId),
                TypeDest = p376.TypeDest,
                DestSlot = p376.DestSlot,
                TypeSrc = p376.TypeSrc,
                SrcSlot = p376.SrcSlot,
                WarpID = p376.WarpID
            };
            return tmp;
        }

        public static void controller(Client client, P_376 p376)
        {
            SItem tempItem = new SItem();
            switch (p376.TypeSrc)
            {
                case (byte)TypeSlot.Storage:
                    tempItem = client.Account.Storage.Item[p376.SrcSlot];
                    switch (p376.TypeDest)
                    {
                        case (byte)TypeSlot.Storage: // bau para o bau
                                                     //Se eh volatile nao faz a alteracao de posicao e sim agrupa-os
                            if (AgrupaVolatile(client, ref p376))
                            {
                                client.Account.Storage.Item[p376.SrcSlot] = client.Account.Storage.Item[p376.DestSlot];
                                client.Account.Storage.Item[p376.DestSlot] = tempItem;
                            }
                            break;
                        case (byte)TypeSlot.Inventory: // bau para o inventorio

                            //Se eh volatile nao faz a alteracao de posicao e sim agrupa-os
                            if (AgrupaVolatile(client, ref p376))
                            {
                                client.Account.Storage.Item[p376.SrcSlot] = client.Character.Mob.Inventory[p376.DestSlot];
                                client.Character.Mob.Inventory[p376.DestSlot] = tempItem;
                            }
                            break;
                        case (byte)TypeSlot.Equip: // bau para o equip

                            if (permicaoEquipItem(tempItem, client.Character.Mob, p376.DestSlot))
                            {
                                client.Account.Storage.Item[p376.SrcSlot] = client.Character.Mob.Equip[p376.DestSlot];
                                client.Character.Mob.Equip[p376.DestSlot] = tempItem;
                            }
                            else
                                client.Send(P_101.New("Não é permitido usar o item"));

                            Functions.GetCurrentScore(client, false);
                            break;
                    }
                    break;
                case (byte)TypeSlot.Inventory:
                    tempItem = client.Character.Mob.Inventory[p376.SrcSlot];
                    switch (p376.TypeDest)
                    {
                        case (byte)TypeSlot.Storage: //Iventorio para o bau
                            //Se eh volatile nao faz a alteracao de posicao e sim agrupa-os
                            if (AgrupaVolatile(client, ref p376))
                            {
                                client.Character.Mob.Inventory[p376.SrcSlot] = client.Account.Storage.Item[p376.DestSlot];
                                client.Account.Storage.Item[p376.DestSlot] = tempItem;
                            }
                            break;
                        case (byte)TypeSlot.Inventory: //Iventorio para o inventorio

                            //Se eh volatile nao faz a alteracao de posicao e sim agrupa-os
                            if (AgrupaVolatile(client, ref p376))
                            {
                                client.Character.Mob.Inventory[p376.SrcSlot] = client.Character.Mob.Inventory[p376.DestSlot];
                                client.Character.Mob.Inventory[p376.DestSlot] = tempItem;
                            }

                            break;
                        case (byte)TypeSlot.Equip://Iventorio para o equip

                            if (permicaoEquipItem(tempItem, client.Character.Mob, p376.DestSlot))
                            {
                                client.Character.Mob.Inventory[p376.SrcSlot] = client.Character.Mob.Equip[p376.DestSlot];
                                client.Character.Mob.Equip[p376.DestSlot] = tempItem;
                            }
                            else
                                client.Send(P_101.New("Não é permitido usar o item"));

                            Functions.GetCurrentScore(client, false);
                            break;
                    }
                    break;
                case (byte)TypeSlot.Equip:
                    tempItem = client.Character.Mob.Equip[p376.SrcSlot];
                    switch (p376.TypeDest)
                    {
                        case (byte)TypeSlot.Storage: //Equipe para o bau
                            client.Character.Mob.Equip[p376.SrcSlot] = client.Account.Storage.Item[p376.DestSlot];
                            client.Account.Storage.Item[p376.DestSlot] = tempItem;
                            break;
                        case (byte)TypeSlot.Inventory: //Equipe para o iventario
                            client.Character.Mob.Equip[p376.SrcSlot] = client.Character.Mob.Inventory[p376.DestSlot];
                            client.Character.Mob.Inventory[p376.DestSlot] = tempItem;
                            break;
                        case (byte)TypeSlot.Equip: // Equipe para equipe
                            client.Character.Mob.Equip[p376.SrcSlot] = client.Character.Mob.Equip[p376.DestSlot];
                            client.Character.Mob.Equip[p376.DestSlot] = tempItem;
                            break;
                    }
                    Functions.GetCurrentScore(client, false);
                    break;
            }
            client.Send(P_376.New(client, p376));
        }

        public static bool permicaoEquipItem(SItem paramItemAEquipar, SMob userMob, int slot)
        {
            if (paramItemAEquipar.Id != 0)
            {
                SItemList itemList = Config.Itemlist[paramItemAEquipar.Id];

                if (userMob.BaseStatus.Level >= itemList.Level && userMob.BaseStatus.Str >= itemList.Str &&
                    userMob.BaseStatus.Int >= itemList.Int && userMob.BaseStatus.Dex >= itemList.Dex &&
                    userMob.BaseStatus.Con >= itemList.Con)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool AgrupaVolatile(Client client, ref P_376 p376)
        {
            SItemList itemListOrigem = Config.Itemlist[client.Character.Mob.Inventory[p376.SrcSlot].Id];
            SItemList itemListDestino = Config.Itemlist[client.Character.Mob.Inventory[p376.DestSlot].Id];

            bool volatileOrigem = itemListOrigem.Ef.Where(a => a.Index == (byte)ItemListEf.EF_VOLATILE).FirstOrDefault().Value == 190;
            bool volatileDestino = itemListDestino.Ef.Where(a => a.Index == (byte)ItemListEf.EF_VOLATILE).FirstOrDefault().Value == 190;
            if (volatileOrigem && volatileDestino)
            {
                SItem itemDestino = SItem.New(client.Character.Mob.Inventory[p376.DestSlot]);
                itemDestino.Ef[1].Type = (byte)ItemListEf.EF_AMOUNT;

                if (client.Character.Mob.Inventory[p376.SrcSlot].Ef.Where(a => a.Type == (byte)ItemListEf.EF_AMOUNT).Count() > 0)
                    itemDestino.Ef[1].Value += client.Character.Mob.Inventory[p376.SrcSlot].Ef.Where(a => a.Type == (byte)ItemListEf.EF_AMOUNT).FirstOrDefault().Value;

                if (client.Character.Mob.Inventory[p376.SrcSlot].Ef.Where(a => a.Type == (byte)ItemListEf.EF_AMOUNT).FirstOrDefault().Value == 0 ||
                    client.Character.Mob.Inventory[p376.DestSlot].Ef.Where(a => a.Type == (byte)ItemListEf.EF_AMOUNT).FirstOrDefault().Value == 0)
                    itemDestino.Ef[1].Value += 1;

                if (client.Character.Mob.Inventory[p376.SrcSlot].Ef.Where(a => a.Type == (byte)ItemListEf.EF_AMOUNT).FirstOrDefault().Value == 0 &&
                    client.Character.Mob.Inventory[p376.DestSlot].Ef.Where(a => a.Type == (byte)ItemListEf.EF_AMOUNT).FirstOrDefault().Value == 0)
                    itemDestino.Ef[1].Value += 1;

                client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p376.SrcSlot);
                client.Character.Mob.Inventory[p376.DestSlot] = itemDestino;

                //Alterao a posicao de origem para o destino, pois qnd o packet enviar para o wyd ele troca a posicao automatica
                p376.SrcSlot = p376.DestSlot;

                //Adiciona o item no char do Mob
                client.Character.Mob.AddItemToCharacter(client, itemDestino, TypeSlot.Inventory, p376.DestSlot);

                return false;
            }
            return true;
        }
    }
}
