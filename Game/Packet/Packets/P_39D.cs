using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Ataque do Mob - size 168
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_39D
    {
        public SHeader Header;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Unk_1;

        public int CurrentHp;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public byte[] Unk_2;

        public ulong CurrentExp;
        public short unk0;

        public SPosition Client;
        public SPosition Mob;

        public short ClientID;
        public short Progress;

        public byte Motion;
        public byte SkillParm;
        public byte DoubleCritical;
        public byte FlagLocal;

        public short Rsv;

        public int CurrentMp;

        public short SkillIndex;
        public short ReqMp;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
        SDamage[] Damage;

        public static P_39D New(Client client, P_39D p39D)
        {
            P_39D tmp = new P_39D
            {
                Header = SHeader.New(0x039D, Marshal.SizeOf<P_39D>(), client.ClientId),
                Client = p39D.Client,
                ClientID = p39D.ClientID,
                CurrentExp = client.Character.Mob.Exp,
                CurrentHp = client.Character.Mob.GameStatus.CurHP,
                CurrentMp = client.Character.Mob.GameStatus.CurMP,
                Damage = p39D.Damage,
                DoubleCritical = p39D.DoubleCritical,
                FlagLocal = p39D.FlagLocal,
                Mob = p39D.Mob,
                Motion = p39D.Motion,
                Progress = p39D.Progress,
                ReqMp = p39D.ReqMp,
                Rsv = p39D.Rsv,
                SkillIndex = p39D.SkillIndex,
                SkillParm = p39D.SkillParm,
                unk0 = p39D.unk0,
                Unk_1 = p39D.Unk_1,
                Unk_2 = p39D.Unk_2
            };
            return tmp;
        }

        public static void controller(Client client, P_39D p39d)
        {
            int tamanho = 0;
            p39d.Damage.ToList().ForEach(a =>
            {
                tamanho += client.MobView.Where(b => a.MobId == b.Mob.ClientId).Count();
            });

            SMobList[] mobList = new SMobList[tamanho];
            for (int i = 0; i < p39d.Damage.Length - 1; i++)
            {
                SMobList dmg = client.MobView.Where(a => a.Mob.ClientId == p39d.Damage[i].MobId).FirstOrDefault();
                if (dmg.Mob.ClientId != 0)
                {
                    mobList[i] = dmg;
                    //remove os mob para que possa ser atualizado com o novo hp ja com o dano recebido depois
                    client.MobView.Remove(dmg);
                }

            }

            //check o modo que esta sendo atacado
            switch (p39d.SkillIndex)
            {
                case 151://Arco
                case -1://espada ou sem arma
                    {
                        for (int i = 0; i < mobList.Count(); i++)
                        {
                            mobList[i].Mob.GameStatus.CurHP -= client.Character.Mob.GameStatus.Attack;
                            mobList[i].Mob.GameStatus.CurHP += mobList[i].Mob.GameStatus.Defense;

                            for (int a = 0; a < p39d.Damage.Length; a++)
                            {
                                if (p39d.Damage[a].MobId == mobList[i].Mob.ClientId)
                                    p39d.Damage[a].Damage = client.Character.Mob.GameStatus.Attack - mobList[i].Mob.GameStatus.Defense;
                            }
                        }
                        break;
                    }
                case 27: // FM: skill Cura
                case 29: // FM: skill Recuperar
                    {
                        SSkillList skill = Config.SkilList[p39d.SkillIndex];
                        int kind = p39d.SkillIndex % 24 / 8 + 1;
                        int special = client.Character.Mob.GameStatus.Master[kind];
                        int dam = 0;

                        if (p39d.SkillIndex == 27)
                            dam = (special * 2) + skill.InstanceValue;
                        else
                            dam = ((special * 3) / 2) + skill.InstanceValue;

                        client.Character.Mob.GameStatus.CurHP += dam;


                        if (client.Character.Mob.GameStatus.CurHP > client.Character.Mob.GameStatus.MaxHP)
                            client.Character.Mob.GameStatus.CurHP = client.Character.Mob.GameStatus.MaxHP;

                        p39d.Damage[0].Damage -= dam;
                        break;
                    }
                case 32: // FM: skill Atake de fogo
                case 33: // FM: skill Ralampago
                case 34: // FM: skill Lanca de gelo
                case 35: // FM: skill Meteoro
                case 36: // FM: skill Nevasca
                case 38: // FM: skill Fenix
                case 39: // FM: skill 8
                    {
                        SSkillList skill = Config.SkilList[p39d.SkillIndex];
                        client.Character.Mob.GameStatus.CurMP -= skill.ManaInicial;

                        SStatus status = client.Character.Mob.GameStatus;

                        int kind = p39d.SkillIndex % 24 / 8 + 1;
                        int special = status.Master[kind];

                        switch (client.Character.Mob.ClassInfo)
                        {
                            case ClassInfo.TK: // TK Magico
                                {
                                    client.Character.Mob.MagicIncrement = (short)(special + skill.InstanceValue + status.Master[0] + status.Level + (status.Int / 4) + (status.Int / 40));
                                    break;
                                }
                            case ClassInfo.FM: // testado
                                {
                                    client.Character.Mob.MagicIncrement = (short)((status.Int * 0.368) + (skill.InstanceValue / 4.2) + (status.Level / 2) + (1.15 * special));
                                    break;
                                }
                            case ClassInfo.BM:
                                {
                                    client.Character.Mob.MagicIncrement = (short)((status.Int / 30) + (status.Int / 3) + skill.InstanceValue + 2 * special);
                                    break;
                                }
                            case ClassInfo.HT:
                                {
                                    client.Character.Mob.MagicIncrement = (short)((3 * status.Master[0]) + (3 * status.Str) + special + skill.InstanceValue);
                                    break;
                                }
                        };

                        for (int i = 0; i < mobList.Count(); i++)
                        {
                            mobList[i].Mob.GameStatus.CurHP -= client.Character.Mob.MagicIncrement;
                            mobList[i].Mob.GameStatus.CurHP += mobList[i].Mob.GameStatus.Defense;

                            for (int a = 0; a < p39d.Damage.Length; a++)
                            {
                                if (p39d.Damage[a].MobId == mobList[i].Mob.ClientId)
                                    p39d.Damage[a].Damage = client.Character.Mob.MagicIncrement - mobList[i].Mob.GameStatus.Defense;
                            }
                        }
                        break;
                    }
                case 37: // FM: skill Trovao
                case 41: // FM: skill Velocidade
                case 43: // FM: skill Escudo Magico
                case 44: // FM: skill Dano
                case 45: // FM: skill Toque de Athena
                case 53: // BM: skill Protecao Elemental
                    {
                        SSkillList skill = Config.SkilList[p39d.SkillIndex];
                        client.Character.Mob.GameStatus.CurMP -= skill.ManaInicial;

                        int index = Functions.GetSlotAffect(client, skill.IdSkill);

                        client.Character.Mob.Affects[index].Index = (byte)skill.TipoDeEfeito;
                        client.Character.Mob.Affects[index].Value = (short)skill.ValorDoEfeito;
                        client.Character.Mob.Affects[index].Time = skill.TempoDoEfeito;
                        Functions.GetCurrentScore(client, false);
                        break;
                    }
                case 56: // BM: skill Sumona Condor
                case 57: // BM: skill Sumona javali Selvagem
                case 58: // BM: skill Sumona Lobo Selvagem
                case 59: // BM: skill Sumona Urso Selvagem
                case 60: // BM: skill Sumona Grande Tigre
                case 61: // BM  skill Sumona Gorila Gigante
                case 62: // BM  skill Sumona Dragao
                case 63: // BM  skill Sumona Succubus
                    {

                        SSkillList skill = Config.SkilList.ToArray()[p39d.SkillIndex - 1]; ;

                        if (skill.InstanceValue >= 1 && skill.InstanceValue <= 50)
                        {
                            int summons = 0;

                            if (skill.InstanceValue == 1 || skill.InstanceValue == 2)
                                summons = client.Character.Mob.GameStatus.Master[2] / 30;

                            else if (skill.InstanceValue == 3 || skill.InstanceValue == 4 || skill.InstanceValue == 5)
                                summons = client.Character.Mob.GameStatus.Master[2] / 40;

                            else if (skill.InstanceValue == 6 || skill.InstanceValue == 7)
                                summons = client.Character.Mob.GameStatus.Master[2] / 80;

                            else if (skill.InstanceValue == 8)
                                summons = 1;

                            //TODO: Falta fazer meio complexo isso
                            //client.Character.Mob.GenerateSummon(client, skill.InstanceValue - 1, summons);

                        }
                        break;
                    }
                case 64: // BM: skill Transformacao Lobisomem
                case 66: // BM: skill Transformacao Urso
                case 68: // BM: skill Transformacao Astaroth
                case 70: // BM: skill Transformacao Titan
                case 71: // BM: skill Transformacao Eden
                    {
                        SSkillList skill = Config.SkilList[p39d.SkillIndex];
                        for (int i = 0; i < client.Character.Mob.Affects.Length; i++)
                        {
                            if (client.Character.Mob.Affects[i].Index == 0 || client.Character.Mob.Affects[i].Index == (byte)Affect.Transformacao)
                            {
                                client.Character.Mob.Affects[i].Index = (byte)skill.TipoDeEfeito;
                                client.Character.Mob.Affects[i].Value = (short)skill.ValorDoEfeito;
                                client.Character.Mob.Affects[i].Time = skill.TempoDoEfeito;
                                Functions.GetCurrentScore(client, false);
                                break;
                            }
                        }

                        short id = 0;
                        switch (p39d.SkillIndex)
                        {
                            case 64:
                                { id = 22; break; }
                            case 66:
                                { id = 23; break; }
                            case 68:
                                { id = 24; break; }
                            case 70:
                                { id = 25; break; }
                            case 71:
                                { id = 32; break; }
                        }

                        //troca a face do client para a skill sumonada
                        client.Character.Mob.Equip[0].Id = id;

                        short[] sanc = new short[16];
                        byte[] anc = new byte[16];

                        for (int i = 0; i < client.Character.Mob.Equip.Length; i++)
                        {
                            //obtem os codigos de todos itens
                            sanc[i] = Functions.GetVisualItemCode(client.Character.Mob.Equip[i], i == 14 ? true : false);

                            //obtem os codigos Anct de todos os itens
                            anc[i] = Functions.GetVisualAnctCode(client.Character.Mob.Equip[i]);
                        }

                        //envia os efeitos
                        client.Send(P_36B.New(client, sanc, anc));

                        //atualiza o score do client
                        Functions.GetCurrentScore(client, false);
                        break;
                    }
            }

            //Adiciona os mob a list de view com dano Hp reduzido
            client.MobView.AddRange(mobList);

            //envia o dano recebido para o mob
            client.Send(P_39D.New(client, p39d));

            //Varre a lista de mob para ver se alguem morreu
            for (int i = 0; i < mobList.Count(); i++)
            {
                if (mobList[i].Mob.GameStatus.CurHP < 0)
                {
                    //mata o mob e envia o drop se houver
                    P_338.controller(client, mobList[i].Mob.ClientId, mobList[i].Mob.Exp);

                    //trata o recebimento do xp
                    AddXpMob(client, mobList[i].Mob.Exp);
                }
            }

            //Cada atake checa se a experiencia estorou o limite e avanca o lvl a cada atake
            if (client.Character.Mob.Exp >= Rate.Exp_Mortal_Arch[client.Character.Mob.BaseStatus.Level])
            {
                client.Character.Mob.BaseStatus.Level += 1;
                Functions.GetCurrentScore(client, false);
                client.Send(P_36A.New(client, Emotion.LevelUP, 3));
            }
        }

        private static void AddXpMob(Client client, ulong pAddXp)
        {
            client.Character.Mob.Exp += (ulong)(pAddXp * P_39D.GetBonusXp(client.Character));

            //atualiza o xp do client
            client.Send(P_337.New(client));

            //trata as mensagens de level, 1/4, 2/4 e 3/4
            if (client.Character.Mob.BaseStatus.Level <= 399)
            {
                if (client.Character.Mob.Exp <= Rate.Exp_Mortal_Arch[client.Character.Mob.BaseStatus.Level])
                {
                    ulong vaLevelOrigem = client.Character.Mob.BaseStatus.Level == 0 ? 0 : Rate.Exp_Mortal_Arch[client.Character.Mob.BaseStatus.Level - 1];
                    ulong vaNextLevel = Rate.Exp_Mortal_Arch[client.Character.Mob.BaseStatus.Level];

                    ulong diferenca = vaNextLevel - vaLevelOrigem;

                    ulong va1 = vaLevelOrigem + (diferenca / 4);
                    ulong va2 = vaLevelOrigem + ((diferenca / 4) * 2);
                    ulong va3 = vaLevelOrigem + ((diferenca / 4) * 3);

                    ulong xpAtual = client.Character.Mob.Exp;

                    if ((xpAtual >= va1 && xpAtual < va2))
                    {
                        client.Send(P_101.New("1/4 BONUS"));
                        client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                    }
                    else if ((xpAtual >= va2 && xpAtual <= va3))
                    {
                        client.Send(P_101.New("2/4 BONUS"));
                        client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                    }
                    else if ((xpAtual >= va3 && xpAtual <= vaNextLevel))
                    {
                        client.Send(P_101.New("3/4 BONUS"));
                        client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                    }
                }
                else
                {
                    Functions.GetCurrentScore(client, true);
                    client.Send(P_101.New("++++++ Level Up ++++++"));
                    client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                }
            }
        }

        private static double GetBonusXp(Character character)
        {
            double bonus = 0;

            //verifica se esta usando fada
            switch (character.Mob.Equip[13].Id)
            {
                case 3900:
                case 3903:
                case 3906:
                case 3911:
                case 3912:
                case 3913:
                    bonus += 16; // fadas que tem 16% de xp bonus
                    break;

                case 3902:
                case 3905:
                case 3904:
                case 3907:
                case 3908:
                    bonus += 32;// fadas que tem 32% de xp bonus
                    break;
            };

            // verifica se o mob esta usando bau de experiencia
            bonus += character.Mob.Affects.Where(a => a.Index == (byte)ItemListEf.EF_BAUEXPERIENCIA).Count() == 0 ? 100 : 200;

            // para cada membro do grupo adiciona 10% na xp
            bonus += (character.PartyID.Count() * 10);

            return bonus / 100;
        }
    }
}
