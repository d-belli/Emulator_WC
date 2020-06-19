using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Envia os efeitos dos Equipamento para o wyd, ex: quando usa a skill de transformacao do BM envia o efeito para o wyd
    /// </summary>
    public struct P_36B
    {
        public SHeader Header;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public short[] SancCodEquip;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] AnctCode;  // 2 a 7	= 6

        public static P_36B New(Client client, short[] paSancCodEquip, byte[] paAnctCode)
        {
            P_36B tmp = new P_36B
            {
                Header = SHeader.New(0x036B, Marshal.SizeOf<P_36B>(), client.ClientId),
                SancCodEquip = paSancCodEquip,
                AnctCode = paAnctCode
            };
            return tmp;
        }
    }
}
