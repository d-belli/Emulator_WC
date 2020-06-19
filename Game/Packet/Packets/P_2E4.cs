using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Deleta o Item - size 20
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_2E4
    {
        public SHeader Header;
        public int Slot;
        public int ItemId;

        public static P_2E4 New(Client client, int slot, int itemId)
        {
            P_2E4 tmp = new P_2E4
            {
                Header = SHeader.New(0x02E4, Marshal.SizeOf<P_2E4>(), client.ClientId),
                Slot = (short)slot,
                ItemId = itemId
            };
            return tmp;
        }
        public static void controller (Client client, P_2E4 p2e4)
        {
            client.Character.Mob.Inventory[p2e4.Slot] = SItem.New();
        }
    }
}
