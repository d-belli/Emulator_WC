using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Compra o item do NPC - size 24
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_379
    {
        public SHeader Header;  //0  a 11 = 12
        public short NpcId;     //12 a 13 = 2
        public short SlotNpc;   //14 a 15 = 2
        public short InventorySlot;
        public short unknow;
        public int Gold;

        public static P_379 New(Client client, P_379 p379)
        {
            P_379 tmp = new P_379
            {
                Header = SHeader.New(0x0379, Marshal.SizeOf<P_379>(), client.ClientId),
                NpcId = p379.NpcId,
                SlotNpc = p379.SlotNpc,
                InventorySlot = p379.InventorySlot,
                Gold = p379.Gold
            };
            return tmp;
        }

        public static void controller(Client client, P_379 p379)
        {
            SMob Npc = Config.MobList.Where(a => a.Mob.ClientId == p379.NpcId).FirstOrDefault().Mob;
            SItem itemId = Config.MobList.Where(a => a.Mob.ClientId == p379.NpcId).FirstOrDefault().Mob.Inventory[p379.SlotNpc];
            SItemList item = Config.Itemlist[itemId.Id];

            if (item.Price <= client.Character.Mob.Gold)
            {
                int tamanho = 30;
                if (client.Character.Mob.Andarilho[0].Id != 0)
                    tamanho += 15;
                if (client.Character.Mob.Andarilho[1].Id != 0)
                    tamanho += 15;

                //checa se o inventario esta cheio
                if (client.Character.Mob.Inventory.Where(a => a.Id != 0).Count() == tamanho)
                    client.Send(P_101.New("O inventario esta cheio."));
                else
                {
                    client.Character.Mob.Gold -= item.Price;
                    client.Character.Mob.Inventory[p379.InventorySlot] = itemId;

                    //Adiciona o item no inventorio
                    client.Character.Mob.AddItemToCharacter(client, itemId, TypeSlot.Inventory, p379.InventorySlot);

                    //atualiza o gold do mob
                    client.Send(P_3AF.New(client, client.Character.Mob.Gold));
                }
            }
            else
                client.Send(P_101.New("Gold insuficiente"));
        }

    }
}
