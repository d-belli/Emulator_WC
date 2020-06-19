using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Envia os affects - size 268
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_3B9
    {
        public SHeader Header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public SAffect[] Affects;

        public static P_3B9 New(Client client)
        {
            P_3B9 tmp = new P_3B9
            {
                Header = SHeader.New(0x03B9, Marshal.SizeOf<P_3B9>(), client.ClientId),
                Affects = client.Character.Mob.Affects
            };
            return tmp;
        }
    }
}
