using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
	/// Abre o NPC de venda/skill - size 16
	/// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_27B
    {
        public SHeader Header;  //0 a 11  = 12
        public short npcID;     //12 a 13 = 2
        public short Unknown;   //14 a 15 = 2

        public static void controller(Client client, P_27B p_27b)
        {
            SMob npc = Config.MobList.Where(a => a.Mob.ClientId == p_27b.npcID).FirstOrDefault().Mob;
            if (npc.Merchant == (short)NpcMerchant.Skill)
                client.Send(P_17C.New(npc, ShopType.Skill));
            else
                client.Send(P_17C.New(npc, ShopType.Normal));
        }
    }
}
