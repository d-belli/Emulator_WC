using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
	/// Vende o item para o NPC - size 20
	/// </summary>
    struct P_37A
    {
        public SHeader Header;  //0  a 11 = 12
        public short NpcId;     //12 a 13 = 2
        public int SlotMob;     //14 a 17 = 4
        public short unknown;   //18 a 19 = 2

        public static P_37A New(Client client, P_37A p379)
        {
            P_37A tmp = new P_37A
            {
                Header = SHeader.New(0x0379, Marshal.SizeOf<P_37A>(), client.ClientId),
                NpcId = p379.NpcId,
                SlotMob = p379.SlotMob,
                unknown = p379.unknown
            };
            return tmp;
        }

        public static void controller(Client client, P_37A p37a)
        {
            int gold = client.Character.Mob.Gold + Config.Itemlist[client.Character.Mob.Inventory[p37a.SlotMob].Id].Price;

            if (gold > 2000000000)
            {
                client.Character.Mob.AddItemToCharacter(client, client.Character.Mob.Inventory[p37a.SlotMob], TypeSlot.Inventory, p37a.SlotMob);
                client.Send(P_101.New("O limite de 2 Bilhao de gold foi atingido!"));
            }
            else
            {
                client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p37a.SlotMob);

                //atualiza o gold do mob
                client.Character.Mob.Gold = gold;
                client.Send(P_3AF.New(client, client.Character.Mob.Gold));
            }

        }
    }
}
