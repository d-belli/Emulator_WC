using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Mata o Mob - size 32
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_338
    {
        public SHeader Header;
        public int CPoint;
        public short MobId;
        public short ClientId;
        public int unknow;
        public ulong Exp;

        public static P_338 New(Client client, short mobId, ulong xpAdd)
        {
            P_338 tmp = new P_338
            {
                Header = SHeader.New(0x0338, Marshal.SizeOf<P_338>(), client.ClientId),
                CPoint = client.Character.Mob.CPoint == 0 ? 0 : (int)xpAdd,
                MobId = mobId,
                ClientId = (short)client.ClientId,
                Exp = xpAdd
            };
            return tmp;
        }

        public static void controller(Client client, short mobId, ulong xpAdd)
        {
            //mata o mob
            client.Send(P_338.New(client, mobId, xpAdd));

            //obtem os itens do mob
            SMobList mob = client.MobView.Where(a => a.Mob.ClientId == mobId).FirstOrDefault();

            //regra de drop:
            //0  a 14 slot sao drop normal      Tx 90%
            //15 a 29 slot drop dificil         Tx 65%
            //30 a 44 slot drop raro            TX 40%
            //45 a 60 slot drop super raro      Tx 15%


            int TaxaDrop = new Random().Next(0, 100);
            SItem itemDropado = SItem.New();

            //inicia o slot com 60 pois para dar a probalidade de nao receber nenhum item
            int slot = 60;

            if (TaxaDrop >= 0 && TaxaDrop <= 15)
                slot = new Random().Next(45, GetSlotAvailable(mob.Mob.Inventory, 45, 59));

            if (TaxaDrop >= 16 && TaxaDrop <= 40)
                slot = new Random().Next(30, GetSlotAvailable(mob.Mob.Inventory, 30, 44));

            if (TaxaDrop >= 41 && TaxaDrop <= 65)
                slot = new Random().Next(15, GetSlotAvailable(mob.Mob.Inventory, 15, 29));

            if (TaxaDrop >= 66 && TaxaDrop <= 100)
                slot = new Random().Next(0, GetSlotAvailable(mob.Mob.Inventory, 0, 14));

            itemDropado = mob.Mob.Inventory[slot];

            if (itemDropado.Id != 0)
            {
                int tamanho = 30;
                if (client.Character.Mob.Andarilho[0].Id != 0)
                    tamanho += 15;
                if (client.Character.Mob.Andarilho[1].Id != 0)
                    tamanho += 15;

                //checa se o inventario esta cheio
                if (mob.Mob.Inventory.Where(a => a.Id != 0).Count() == tamanho)
                    client.Send(P_101.New("O inventario esta cheio."));
                else
                {
                    client.Character.Mob.Gold += mob.Mob.Gold < 0 ? mob.Mob.Gold * -1 : mob.Mob.Gold;
                    client.Character.Mob.Inventory.ToList().ForEach(a => { slot = 0; if (a.Id == 0) return; else slot += 1; });
                    client.Character.Mob.AddItemToCharacter(client, itemDropado, TypeSlot.Inventory, slot);
                }

            }
            Log.Normal($"Teve: {TaxaDrop}% de conseguir algum item");

        }

        private static int GetSlotAvailable(SItem[] paIventory, int index, int count)
        {
            int ct = index;
            for (int i = index; i < count; i++)
            {
                ct += paIventory[i].Id == 0 ? 0 : 1;
            }
            return ct;
        }
    }
}
