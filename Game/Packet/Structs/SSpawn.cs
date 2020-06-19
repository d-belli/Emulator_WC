using System;
using System.Runtime.InteropServices;

namespace Emulator
{
 
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
    public struct SSpawn
    {

        public SPosition Position;

        public Int16 SessionId;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 12)]
        public String Name;

        public Byte ChaosPoints;
        public Byte CurrentKill;
        public Int16 TotalKill;

        public SItemEF[] ItemEffect;

        public SItemEF[] AffectEffect;

        public Int16 GuildIndex;

        public UInt16 Unknow_100; // Constant 0xCC00

        public SStatus Status;

        public Int16 SpawnType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public Byte[] AnctCode;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 26)]
        public String TabValue;

        public Int16 Unknow_174;

        public static SSpawn New(SMob mob, Int16 sessionId, String paramTab)
        //: base(0x364, 0x7530)
        {
            SSpawn tmp = new SSpawn
            {
                SpawnType = 2,/*1-efeito normal/2-efeito motherfucker*/
                Position = mob.LastPosition,
                SessionId = sessionId,
                Name = mob.Name,
                ItemEffect = new SItemEF[3],
                AffectEffect = new SItemEF[3],
                Status = mob.BaseStatus,
                AnctCode = new Byte[16],
                TabValue = paramTab,
                ChaosPoints = 200,
                TotalKill = 200,
                Unknow_100 = 0xCC00,
                GuildIndex = 0,
            };
            for (int x = 0; x < 16; x++)
            {
                SItem item = mob.Equip[x];
                //ItemEffect.Effects[x] = (short)(Calculos.CalculoEfeito(item, x));
               // AnctCode[x] = (byte)(Calculos.CalculoAnct(mob, x));
            }
            return tmp;

        }
    }
}
