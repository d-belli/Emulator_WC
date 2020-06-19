using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// unknown - size 36
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_2CB
    {
        public SHeader Header;
        public short unknown;
        public short unknown1;
        public short unknown2;
        public short unknown3;
        public short unknown4;
        public short unknown5;
        public short unknown6;
        public short unknown7;
        public short unknown8;
        public short unknown9;
        public short unknown10;
        public short unknown11;

        public static void controller(Client client, P_2CB p2cb)
        {

        }
    }
}
