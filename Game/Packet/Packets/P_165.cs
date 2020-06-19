using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Remove Mob da visão - size 16
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_165
    {
        // Atributos
        public SHeader Header;    // 0 a 11		= 12
        public LeaveVision Type;  // 12 a 15	= 4     1: morte,    2: logout

        // Construtores
        public static P_165 New(int ClientID, LeaveVision Type)
        {
            P_165 tmp = new P_165
            {
                Header = SHeader.New(0x165, Marshal.SizeOf<P_165>(), ClientID),
                Type = Type
            };
            return tmp;
        }
    }
}