using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Atualiza o status do Personagem - size 48
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_336
    {
        public SHeader Header;
        public SStatus Status;
        public byte Critical;
        public byte SaveMana;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public short[] Affect;
        public short Guild;
        public short GuildLevel;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Resist;

        public byte RegenHP;
        public byte RegenMP;
        public int CurrHp;
        public int CurrMp;
        public int Magic;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Special;

        public static P_336 New(Client client)
        {
            SMob mob = client.Character.Mob;
            P_336 tmp = new P_336
            {
                Header = SHeader.New(0x0336, Marshal.SizeOf<P_336>(), client.ClientId),
                Status = new SStatus(),
                Critical = mob.Critical,
                SaveMana = mob.SaveMana,
                Affect = Functions.GetAffect(mob.Affects),
                Guild = mob.GuildIndex,
                GuildLevel = mob.GuildIndex,
                Resist = mob.Resistencia,
                Magic = mob.MagicIncrement,
                Special = new byte[4],
                CurrMp = mob.GameStatus.CurMP,
                CurrHp = mob.GameStatus.CurHP
            };

            tmp.Status.Level = mob.GameStatus.Level;
            tmp.Status.Defense = mob.GameStatus.Defense;
            tmp.Status.Attack = mob.GameStatus.Attack;
            tmp.Status.MobSpeed = mob.GameStatus.MobSpeed;
            tmp.Status.MaxHP = mob.GameStatus.MaxHP;
            tmp.Status.MaxMP = mob.GameStatus.MaxMP;
            tmp.Status.CurHP = mob.GameStatus.CurHP;
            tmp.Status.CurMP = mob.GameStatus.CurMP;
            tmp.Status.Str = mob.GameStatus.Str;
            tmp.Status.Int = mob.GameStatus.Int;
            tmp.Status.Dex = mob.GameStatus.Dex;
            tmp.Status.Con = mob.GameStatus.Con;
            tmp.Status.Master = mob.GameStatus.Master;

            for (int i = 0; i < tmp.Special.Length; i++) { tmp.Special[i] = (byte)mob.GameStatus.Master[i]; }

            return tmp;
        }
    }
}
