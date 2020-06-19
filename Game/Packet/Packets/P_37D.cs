using System;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    ///  Adiciona ao grupo size 40
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct P_37D
    {
        public SHeader Header;

        public short ClientId;
        public short Level;
        public short MaxHp;
        public short Hp;

        public short LiderID;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] NameBytes;
        public short Target;

        public string Name
        {
            get => Functions.GetString(this.NameBytes);
            set
            {
                this.NameBytes = Config.Encoding.GetBytes(value);
                Array.Resize(ref this.NameBytes, 16);
            }
        }

        public static P_37D New(short liderId, short level, short maxHp, short hp, short clientId, byte[] name, short target)
        {
            P_37D tmp = new P_37D
            {
                Header = SHeader.New(0x037D, Marshal.SizeOf<P_37D>(), 0),
                ClientId = clientId,
                Level = level,
                MaxHp = maxHp,
                Hp = hp,
                LiderID = liderId,
                NameBytes = name,
                Target = target
            };
            return tmp;
        }
    }
}
