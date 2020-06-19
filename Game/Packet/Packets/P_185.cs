using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
	/// Atualiza o inventory do character - size 528
	/// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_185
    {
        public SHeader Header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public SItem[] Inventory;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public SItem[] Andarilho;     // 748 a 763		= 16
        public int Gold;

        public static P_185 New(Client client)
        {
            P_185 tmp = new P_185
            {
                Header = SHeader.New(0x0185, Marshal.SizeOf<P_185>(), client.ClientId),
                Inventory = new SItem[60],
                Andarilho = new SItem[2],
                Gold = client.Character.Mob.Gold
            };

            for (int i = 0; i < tmp.Inventory.Length; i++) { tmp.Inventory[i] = client.Character.Mob.Inventory[i]; }
            for (int i = 0; i < tmp.Andarilho.Length; i++) { tmp.Andarilho[i] = client.Character.Mob.Andarilho[i]; }

            return tmp;
        }

    }
}
