using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Adiciona o Item do client- size 24
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_182
    {
        public SHeader Header;
        public short Type;
        public short Slot;
        SItem Item;

        public static P_182 New(Client client, SItem item, TypeSlot type, int slot)
        {
            P_182 tmp = new P_182
            {
                Header = SHeader.New(0x0182, Marshal.SizeOf<P_182>(), client.ClientId),
                Type = (short)type,
                Slot = (short)slot,
                Item = item
            };
            return tmp;
        }
    }
}
