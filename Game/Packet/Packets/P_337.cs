using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Atualiza os pontos de atribuicao do personagem - size 48
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_337
    {
        public SHeader Header;
        public uint CPoint;
        public ulong Exp;
        public ulong Learn;
        public short StatusPoint;
        public short MasterPoint;
        public short SkillPoint;
        public short Magic;
        public int GoldMob;

        public static P_337 New(Client client)
        {
            P_337 tmp = new P_337
            {
                Header = SHeader.New(0x0337, Marshal.SizeOf<P_337>(), client.ClientId),
                CPoint = client.Character.Mob.CPoint == 0 ? 0 : (uint)client.Character.Mob.Exp,
                Exp = client.Character.Mob.Exp,
                Learn = client.Character.Mob.LearnedSkill,
                StatusPoint = client.Character.Mob.StatusPoint,
                MasterPoint = client.Character.Mob.MasterPoint,
                GoldMob = client.Character.Mob.Gold,
                SkillPoint = client.Character.Mob.SkillPoint,
                Magic = client.Character.Mob.MagicIncrement,
            };
            return tmp;
        }
    }
}
