using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Emulator
{
    /// <summary>
    /// Estrutura do mob - size 1336
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SMob
    {
        // Atributos
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] NameBytes;      // 0 a 15				= 16
                                      //CapeInfo reino azul ou vermelho
        public byte CapaReino;           // 16						= 1
        public byte Merchant;           // 17						= 1

        public short GuildIndex;        // 18 a 19			= 2
        public ClassInfo ClassInfo;          // 20						= 1
        public byte AffectInfo;         // 21						= 1
        public short QuestInfo;         // 22 a 23			= 2

        public int Gold;                // 24 a 27			= 4

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Unk1;           // 28 a 31			= 4

        public ulong Exp;               // 32 a 39			= 8

        public SPosition LastPosition;  // 40 a 43			= 4

        public SStatus BaseStatus;      // 44 a 91			= 48
        public SStatus GameStatus;      // 92 a 139			= 48

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public SItem[] Equip;         // 140 a 267		= 128
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 60)]
        public SItem[] Inventory;     // 268 a 747		= 480
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public SItem[] Andarilho;     // 748 a 763		= 16

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] Unk2;           // 764 a 771		= 8

        public short Item547;           // 772 a 773		= 2
        public byte ChaosPoints;        // 774					= 1
        public byte CurrentKill;        // 775					= 1
        public short TotalKill;         // 776 a 777		= 2

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Unk3;           // 778 a 779		= 2

        public ulong LearnedSkill;             // 780 a 787		= 8

        public short StatusPoint;       // 788 a 789		= 2
        public short MasterPoint;       // 790 a 791		= 2
        public short SkillPoint;        // 792 a 793		= 2

        public byte Critical;           // 794					= 1
        public byte SaveMana;           // 795					= 1

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] SkillBar1;      // 796 a 799		= 4

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Unk4;           // 800 a 803		= 4

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Resistencia;         // 804	a 807		0:Fire, 1:Ice, 2:Holy, 3:hunder

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 210)]
        public byte[] Unk5;           // 808 a 1017		= 210

        public short MagicIncrement;    // 1018 a 1019	= 2

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] Unk6;           // 1020 a 1025	= 6

        public short ClientId;          // 1026 a 1027	= 2
        [XmlIgnore]
        public City CityId;            // 1028 a 1029	= 2  

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] SkillBar2;      // 1030 a 1045	= 16

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Unk7;           // 1046 a 1047	= 2

        public uint CPoint;               // 1048 a 1051	= 4

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 26)]
        public byte[] TabBytes;       // 1052 a 1077	= 26

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
        public byte[] Unk8;           // 1078 a 1079	= 2

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public SAffect[] Affects;     // 1080 a 1335	= 256

        public short AttackSpeed;

        public short Evasion;

        public SPosition PositionSaved;

        // Ajudantes
        public string Name
        {
            get => Functions.GetString(this.NameBytes);
            set
            {
                this.NameBytes = Config.Encoding.GetBytes(value);
                Array.Resize(ref this.NameBytes, 16);
            }
        }
        public string Tab
        {
            get => Functions.GetString(this.TabBytes);
            set
            {
                this.TabBytes = Config.Encoding.GetBytes(value);
                Array.Resize(ref this.TabBytes, 26);
            }
        }

        // Construtores
        public static SMob New()
        {
            SMob tmp = new SMob
            {
                Name = "",

                CapaReino = 0,
                Merchant = 0,

                GuildIndex = 0,
                ClassInfo = 0,
                AffectInfo = 0,
                QuestInfo = 0,

                Gold = 0,

                Unk1 = new byte[4],

                Exp = 0,

                LastPosition = SPosition.New(),

                BaseStatus = SStatus.New(),
                GameStatus = SStatus.New(),

                Equip = new SItem[16],
                Inventory = new SItem[60],
                Andarilho = new SItem[2],

                Unk2 = new byte[8],

                Item547 = 0,
                ChaosPoints = 0,
                CurrentKill = 0,
                TotalKill = 0,

                Unk3 = new byte[2],

                LearnedSkill = 0,

                StatusPoint = 0,
                MasterPoint = 0,
                SkillPoint = 0,

                Critical = 0,
                SaveMana = 0,

                SkillBar1 = new byte[4],

                Unk4 = new byte[4],

                Resistencia = new byte[4],

                Unk5 = new byte[210],

                MagicIncrement = 0,

                Unk6 = new byte[6],

                ClientId = 0,
                CityId = City.Armia,

                SkillBar2 = new byte[16],

                Unk7 = new byte[2] { 204, 204 },

                CPoint = 0,

                Tab = "",

                Unk8 = new byte[2],

                Affects = new SAffect[32],
                AttackSpeed = 0,
                Evasion = 0
            };

            for (int i = 0; i < tmp.Equip.Length; i++) { tmp.Equip[i] = SItem.New(); }
            for (int i = 0; i < tmp.Inventory.Length; i++) { tmp.Inventory[i] = SItem.New(); }
            for (int i = 0; i < tmp.Andarilho.Length; i++) { tmp.Andarilho[i] = SItem.New(); }

            for (int i = 0; i < tmp.SkillBar1.Length; i++) { tmp.SkillBar1[i] = 255; }
            for (int i = 0; i < tmp.SkillBar2.Length; i++) { tmp.SkillBar2[i] = 255; }

            for (int i = 0; i < tmp.Affects.Length; i++) { tmp.Affects[i] = SAffect.New(); }
            for (int i = 0; i < tmp.Resistencia.Length; i++) { tmp.Resistencia[i] = new byte(); }

            return tmp;
        }
        private static SMob Base(string name, ClassInfo mobClass)
        {
            SMob mob = New();

            mob.Name = name;
            mob.BaseStatus.Defense = 40;
            mob.BaseStatus.MobSpeed = 2;

            switch (mobClass)
            {
                case ClassInfo.TK:
                    {
                        mob.BaseStatus.Str = 8;
                        mob.BaseStatus.Int = 4;
                        mob.BaseStatus.Dex = 7;
                        mob.BaseStatus.Con = 6;
                        mob.BaseStatus.CurMP = 80 + ((mob.BaseStatus.Level + mob.BaseStatus.Con) * 3);
                        mob.BaseStatus.MaxHP = 80 + ((mob.BaseStatus.Level + mob.BaseStatus.Con) * 3);
                        mob.BaseStatus.CurMP = 45 + ((mob.BaseStatus.Level + mob.BaseStatus.Int) * 1);
                        mob.BaseStatus.MaxMP = 45 + ((mob.BaseStatus.Level + mob.BaseStatus.Int) * 1);

                        mob.Equip[0].Id = 1;
                        mob.Equip[1].Id = 1104;
                        mob.Equip[2].Id = 1116;
                        mob.Equip[3].Id = 1128;
                        mob.Equip[4].Id = 1140;
                        mob.Equip[5].Id = 1152;
                        mob.Inventory[0].Id = 917;
                        break;
                    }

                case ClassInfo.FM:
                    {
                        mob.BaseStatus.Str = 5;
                        mob.BaseStatus.Int = 8;
                        mob.BaseStatus.Dex = 5;
                        mob.BaseStatus.Con = 5;
                        mob.BaseStatus.CurHP = 60 + ((mob.BaseStatus.Level + mob.BaseStatus.Con) * 1);
                        mob.BaseStatus.MaxHP = 60 + ((mob.BaseStatus.Level + mob.BaseStatus.Con) * 1);
                        mob.BaseStatus.CurMP = 65 + ((mob.BaseStatus.Level + mob.BaseStatus.Int) * 3);
                        mob.BaseStatus.MaxMP = 65 + ((mob.BaseStatus.Level + mob.BaseStatus.Int) * 3);

                        mob.Equip[0].Id = 11;
                        mob.Equip[1].Id = 1254;
                        mob.Equip[2].Id = 1266;
                        mob.Equip[3].Id = 1278;
                        mob.Equip[4].Id = 1290;
                        mob.Equip[5].Id = 1302;
                        mob.Inventory[0].Id = 891;
                        break;
                    }

                case ClassInfo.BM:
                    {
                        mob.BaseStatus.Str = 6;
                        mob.BaseStatus.Int = 6;
                        mob.BaseStatus.Dex = 9;
                        mob.BaseStatus.Con = 5;
                        mob.BaseStatus.CurHP = 70 + ((mob.BaseStatus.Level + mob.BaseStatus.Con) * 1);
                        mob.BaseStatus.MaxHP = 70 + ((mob.BaseStatus.Level + mob.BaseStatus.Con) * 1);
                        mob.BaseStatus.CurMP = 55 + ((mob.BaseStatus.Level + mob.BaseStatus.Int) * 2);
                        mob.BaseStatus.MaxMP = 55 + ((mob.BaseStatus.Level + mob.BaseStatus.Int) * 2);

                        mob.Equip[0].Id = 21;
                        mob.Equip[1].Id = 1416;
                        mob.Equip[2].Id = 1419;
                        mob.Equip[3].Id = 1422;
                        mob.Equip[4].Id = 1425;
                        mob.Equip[5].Id = 1428;
                        mob.Inventory[0].Id = 917;
                        break;
                    }

                case ClassInfo.HT:
                    {
                        mob.BaseStatus.Str = 8;
                        mob.BaseStatus.Int = 9;
                        mob.BaseStatus.Dex = 13;
                        mob.BaseStatus.Con = 6;
                        mob.BaseStatus.CurHP = 75 + ((mob.BaseStatus.Level + mob.BaseStatus.Con) * 2);
                        mob.BaseStatus.MaxHP = 75 + ((mob.BaseStatus.Level + mob.BaseStatus.Con) * 2);
                        mob.BaseStatus.CurMP = 60 + ((mob.BaseStatus.Level + mob.BaseStatus.Int) * 1);
                        mob.BaseStatus.MaxMP = 60 + ((mob.BaseStatus.Level + mob.BaseStatus.Int) * 1);

                        mob.Equip[0].Id = 31;
                        mob.Equip[1].Id = 1566;
                        mob.Equip[2].Id = 1569;
                        mob.Equip[3].Id = 1572;
                        mob.Equip[4].Id = 1575;
                        mob.Equip[5].Id = 1578;
                        mob.Inventory[0].Id = 816;
                        break;
                    }
            }

            mob.BaseStatus.Attack = (mob.AttackSpeed * 10) + (mob.BaseStatus.Dex / 5);
            mob.AttackSpeed = (short)((mob.AttackSpeed * 10) + (mob.BaseStatus.Dex / 5));
            mob.Item547 = 547;
            mob.ChaosPoints = 150;
            mob.CurrentKill = 0;
            mob.TotalKill = 0;

            mob.LastPosition = SPosition.New(2100, 2100);

            mob.GameStatus = mob.BaseStatus;

            return mob;
        }
        public static SMob TK(string name)
        {
            SMob mob = Base(name, ClassInfo.TK);
            mob.ClassInfo = ClassInfo.TK;
            return mob;
        }
        public static SMob FM(string name)
        {
            SMob mob = Base(name, ClassInfo.FM);
            mob.ClassInfo = ClassInfo.FM;
            return mob;
        }
        public static SMob BM(string name)
        {
            SMob mob = Base(name, ClassInfo.BM);
            mob.ClassInfo = ClassInfo.BM;
            return mob;
        }
        public static SMob HT(string name)
        {
            SMob mob = Base(name, ClassInfo.HT);
            mob.ClassInfo = ClassInfo.HT;
            return mob;
        }

        public void AddItemToCharacter(Client client, SItem pItem, TypeSlot typeSlot, int slot)
        {
            int primeiroSlotVazio = 0;
            if (slot == 0)
            {
                for (int i = 0; i < this.Inventory.Length; i++)
                {
                    if (this.Inventory[i].Id == 0)
                    {
                        primeiroSlotVazio = i;
                        break;
                    }
                }
                this.Inventory[primeiroSlotVazio] = pItem;
            }
            else
                primeiroSlotVazio = slot;

            //Adiciona o item no char do Mob
            client.Send(P_182.New(client, pItem, typeSlot, primeiroSlotVazio));
        }

        public void RemoveItemToCharacter(Client client, TypeSlot typeSlot, int slot)
        {
            client.Character.Mob.Inventory[slot] = SItem.New();
            client.Send(P_182.New(client, SItem.New(), typeSlot, slot));
        }
    }

    /// <summary>
    /// Estrutura do Dano - size 8
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SDamage
    {
        public int MobId;
        public int Damage;
    }
}