using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Mostra a emotion - size 36
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_36A
    {
        public SHeader Header;
        public Emotion Motion;
        public short Parm;
        public int NotUsed;

        public static P_36A New(Client client, Emotion emo, short parm)
        {
            P_36A tmp = new P_36A
            {
                Header = SHeader.New(0x036A, Marshal.SizeOf<P_36A>(), client.ClientId),
                Motion = emo,
                Parm = parm,
                NotUsed = 0
            };
            return tmp;
        }
    }
}
