using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
	/// Atualiza o gold do Mob - size 14
	/// </summary>
    struct P_3AF
    {
        public SHeader Header;
        public int Gold;

        public static P_3AF New(Client client, int pGold)
        {
            P_3AF tmp = new P_3AF
            {
                Header = SHeader.New(0x03AF, Marshal.SizeOf<P_3AF>(), client.ClientId),
                Gold = pGold
            };
            return tmp;
        }
    }
}
