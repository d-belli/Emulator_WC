using System.Runtime.InteropServices;

namespace Emulator
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct STRUCT_MOB
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string name; // 0 ~ 15  = 16
        public byte Clan; // 16 = 1
        public byte Merchant; // 17 = 1
        public short Guild; // 18 a 19 = 2
        public byte Class; // 20 = 1
        public byte resistencia; // 21 = 1
        public short Quest; // 22 a 23 = 2 

        public int gold; // 24 a 27 = 4

        public int Unk1; // 28 a 31 = 4

        public long Exp; // 32 a 39 = 8

        public short SPX; // 40 = 1
        public short SPY; // 41 = 2

        public STRUCT_SCORE BaseScore; // 42 a 89 = 48
        public STRUCT_SCORE CurrentScore; // 90 a 137 = 48

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public STRUCT_ITEM[] Equip; // 138 a 265 = 128
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public STRUCT_ITEM[] Carry; // 266 a 777 = 512

        public ulong LearnedSkill; // 778 a 785 = 8

        public short ScoreBonus; // 788 a 789 = 2
        public short SpecialBonus; // 790 a 791 = 2
        public short SkillBonus; // 792 a 793 = 2

        public byte Critical; // 786 = 1
        public byte SaveMana; // 787 = 1

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] SkillBar; // 794 a 797

        public byte GuildLevel; // 798 = 1

        public int Magic; // 799 a 802 = 4
        public short RegenHP; // 803 a 804 = 2
        public short RegenMP; // 805 a 806 = 2

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Resist; // 806 a 809 = 4
    }

    public struct STRUCT_ITEM
    {
        public short sIndex;//2
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
        public sItemADD[] sEffect;//8
    };

    public struct sItemADD//2
    {
        public byte cEfeito;
        public byte cValue;
    }

    public struct STRUCT_SCORE
    {
        public int Level;
        public int Defesa;
        public int Ataque;

        public byte Merchante;
        public byte Speed;
        public byte Direcao;
        public byte ChaosRate;

        public int MaxHP;
        public int MaxMP;
        public int HP;
        public int MP;

        public short Str;
        public short Int;
        public short Dex;
        public short Con;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public short[] Special;

    };
}
