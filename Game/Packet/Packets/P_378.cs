using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Adiciona a skill na barra de skill - size 32
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_378
    {
        public SHeader Header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Skill1;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] Skill2;

        public static void controller(Client client, P_378 p378)
        {
            client.Character.Mob.SkillBar1 = p378.Skill1;
            client.Character.Mob.SkillBar2 = p378.Skill2;

            UserMobDAO.CreateOrUpdateChar(client.Character);
        }
    }
}
