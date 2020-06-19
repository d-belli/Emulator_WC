using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;

namespace Emulator
{
    public static class Functions
    {
        // Retorna uma string sem caracteres nulos
        public static string GetString(byte[] Data)
        {
            if (Data == null)
            {
                throw new Exception("data == null");
            }

            return Config.Encoding.GetString(Data).TrimEnd('\0');
        }

        // Diminui a Quantidade do Item 
        public static void SetItemAmount(Client Client, SItem Item, int Slot)
        {
            for (int i = 0; i < Item.Ef.Length; i++)
            {
                if (Item.Ef[i].Type == (byte)ItemListEf.EF_AMOUNT)
                {
                    Item.Ef[i].Value -= 1;

                    if (Item.Ef[i].Value == 0)
                        Client.Character.Mob.RemoveItemToCharacter(Client, TypeSlot.Inventory, Slot);
                }
            }
        }

        // Retorna o ID do item pra visão
        public static short GetVisualItemCode(SItem Item, bool Mont)
        {
            int value = 0;

            if (Mont)
            {// montaria
                if (Item.Id >= 3980 && Item.Id < 3995)
                    return Item.Id;

                if (Item.Id == 2360)
                {
                    if (Item.Ef[0].Value > 0)
                        value = Item.Ef[1].Type / 10;
                    else
                        return 0;
                }

                return (short)(Item.Id | (value * 0x1000));
            }
            else
            {
                if (Item.Ef[0].Type == 43)
                    value = Item.Ef[0].Value;
                else if (Item.Ef[1].Type == 43)
                    value = Item.Ef[1].Value;
                else if (Item.Ef[2].Type == 43)
                    value = Item.Ef[2].Value;

                else if (Item.Ef[0].Type >= 116 && Item.Ef[0].Type <= 125)
                    return (short)(Item.Id | (12 * 0x1000));

                else if (Item.Ef[1].Type >= 116 && Item.Ef[1].Type <= 125)
                    return (short)(Item.Id | (12 * 0x1000));

                else if (Item.Ef[2].Type >= 116 && Item.Ef[2].Type <= 125)
                    return (short)(Item.Id | (12 * 0x1000));

                else
                    return Item.Id;
            }

            if (value < 230)
                value %= 10;

            else if (value < 234)
                value = 10;
            else if (value < 238)
                value = 11;
            else if (value < 242)
                value = 12;
            else if (value < 246)
                value = 13;
            else if (value < 250)
                value = 14;
            else if (value < 254)
                value = 15;
            else// value < 256
                value = 16;

            return (short)(Item.Id | (value * 0x1000));
        }

        // Retorna o código anct do item ou sua tintura
        public static byte GetVisualAnctCode(SItem Item)
        {
            byte value = 0;

            if (Item.Id >= 2360 && Item.Id <= 2390 && Item.Ef[0].Value > 0)
            {
                switch (Item.Ef[2].Value)
                {
                    case 11:
                        return Convert.ToByte(0x10B);
                    case 12:
                        return Convert.ToByte(0x10C);
                    case 13:
                        return Convert.ToByte(0x10D);
                    case 14:
                        return Convert.ToByte(0x10E);
                    case 15:
                        return Convert.ToByte(0x10F);
                    case 16:
                        return Convert.ToByte(0x110);
                    case 17:
                        return Convert.ToByte(0x111);
                    case 18:
                        return Convert.ToByte(0x112);
                    case 19:
                        return Convert.ToByte(0x113);
                    case 20:
                        return Convert.ToByte(0x114);

                    case 35:
                        if (Item.Id == 2363 || Item.Id == 2377)
                            return Convert.ToByte(0x115);
                        break;

                    default:
                        return 0x00;
                }

                return 0;
            }
            if (Item.Ef[0].Type == 43)
                value = Item.Ef[0].Value;
            else if (Item.Ef[1].Type == 43)
                value = Item.Ef[1].Value;
            else if (Item.Ef[2].Type == 43)
                value = Item.Ef[2].Value;


            if (Item.Ef[0].Type >= 116 && Item.Ef[0].Type <= 125)
                return value = (byte)(Convert.ToInt32(Item.Ef[0].Type) - 3);
            if (Item.Ef[1].Type >= 116 && Item.Ef[1].Type <= 125)
                return value = (byte)(Convert.ToInt32(Item.Ef[1].Type) - 3);
            if (Item.Ef[2].Type >= 116 && Item.Ef[2].Type <= 125)
                return value = (byte)(Convert.ToInt32(Item.Ef[2].Type) - 3);

            if (value == 0)
                return 0;

            if (value < 230)
                return 43;

            switch (value % 4)
            {
                case 0: return 0x30;
                case 1: return 0x40;
                case 2: return 0x10;
                default: return 0x20;
            }
        }

        // Retorna o valor da refinação
        public static byte GetItemSanc(SItem Item)
        {
            for (int i = 0; i < Item.Ef.Length; i++)
            {
                if (Item.Ef[i].Type == (byte)ItemListEf.EF_SANC || Item.Ef[i].Type >= 116 && Item.Ef[i].Type <= 125)
                {
                    return Item.Ef[i].Value;
                }
            }
            return 0;
        }

        // Retorna o valor dos Affect
        public static short[] GetAffect(SAffect[] affect)
        {
            short[] vaAffect = new short[32];
            for (int i = 0; i < affect.Length; i++)
            {
                int type = affect[i].Index;
                int value = affect[i].Time;

                if (value > 2550000)
                    value = 2550000;

                vaAffect[i] = (short)((type << 8) + (value & 0xFF));
            }
            return vaAffect;
        }

        // Adiciona o valor da refinação
        public static void SetItemSanc(SItem Item)
        {
            int sanc = GetItemSanc(Item);

            if (sanc < 9)
                sanc += 1;
            if (sanc == 10)
                sanc = 12;
            else if (sanc == 11)
                sanc = 15;
            else if (sanc == 12)
                sanc = 18;
            else if (sanc == 13)
                sanc = 22;
            else if (sanc == 14)
                sanc = 27;

            for (int i = 0; i < Item.Ef.Length; i++)
            {
                if (Item.Ef[i].Type == (byte)ItemListEf.EF_SANC || Item.Ef[i].Type >= 116 && Item.Ef[i].Type <= 125)
                {
                    Item.Ef[i].Value = (byte)sanc;
                }
            }
        }

        // Retorna quantas vezes o valor da refinação aumenta o status do item
        public static double GetItemSanValue(int Slot, byte Sanc)
        {
            if (Slot == 0 || Slot == 14)
            {
                return 0;
            }
            else
            {
                if (Slot >= 1 && Slot <= 7)
                {
                    switch (Sanc)
                    {
                        case 1:
                            return 1.1;
                        case 2:
                            return 1.2;
                        case 3:
                            return 1.3;
                        case 4:
                            return 1.4;
                        case 5:
                            return 1.5;
                        case 6:
                            return 1.6;
                        case 7:
                            return 1.7;
                        case 8:
                            return 1.8;
                        case 9:
                            return 1.9;
                        case 230:
                        case 231:
                        case 232:
                        case 233:
                            return 2;
                        case 234:
                        case 235:
                        case 236:
                        case 237:
                            return 2.2;
                        case 238:
                        case 239:
                        case 240:
                        case 241:
                            return 2.5;
                        case 242:
                        case 243:
                        case 244:
                        case 245:
                            return 2.8;
                        case 246:
                        case 247:
                        case 248:
                        case 249:
                            return 3.2;
                        case 250:
                        case 251:
                        case 252:
                        case 253:
                            return 3.7;
                        default:
                            return 1;
                    }
                }
                else if (Slot >= 8 && Slot <= 15)
                {
                    switch (Sanc)
                    {
                        case 1:
                            return 1.1;
                        case 2:
                            return 1.2;
                        case 3:
                            return 1.3;
                        case 4:
                            return 1.4;
                        case 5:
                            return 1.5;
                        case 6:
                            return 1.6;
                        case 7:
                            return 1.7;
                        case 8:
                            return 1.8;
                        case 9:
                            return 2;
                        case 230:
                        case 231:
                        case 232:
                        case 233:
                            return 2.2;
                        case 234:
                        case 235:
                        case 236:
                        case 237:
                            return 2.5;
                        case 238:
                        case 239:
                        case 240:
                        case 241:
                            return 2.8;
                        case 242:
                        case 243:
                        case 244:
                        case 245:
                            return 3.2;
                        case 246:
                        case 247:
                        case 248:
                        case 249:
                            return 3.7;
                        case 250:
                        case 251:
                        case 252:
                        case 253:
                            return 4;
                        default:
                            return 1;
                    }
                }
                else
                {
                    return 1;
                }
            }
        }

        // Atualiza os status do personagem
        public static void GetCurrentScore(Client Client, bool UP)
        {
            ref SMob mob = ref Client.Character.Mob;

            Score status = Score.New();
            if (UP)
                mob.BaseStatus.Level += 1;

            int
                level = mob.BaseStatus.Level,
                statusPoint = 0,
                masterPoint = 0,
                skillPoint = 0;

            double
                bootsSpeed = 0;

            #region Mounts
            double
                mountDamage = 0,
                mountMagic = 0,
                mountEvasion = 0,
                mountResist = 0,
                mountSpeed = 0;

            byte
                MountLevel = mob.Equip[14].Ef[1].Type;

            short montID = mob.Equip[14].Id;

            if (montID >= 2360 && montID <= 2361)
            {
                mountSpeed = 4;
            }
            else if (montID >= 2362 && montID <= 2365)
            {
                mountSpeed = 5;
            }
            else if (montID >= 2366 && montID <= 2389)
            {
                mountSpeed = 6;
            }

            switch (montID)
            {
                #region Porco
                case 2360:
                    break;
                #endregion
                #region Javali
                case 2361:
                    break;
                #endregion
                #region Lobo
                case 2362:
                    mountDamage = 10 + (MountLevel * 0.5);
                    mountMagic = 1 + (MountLevel * 0.1);
                    break;
                #endregion
                #region Dragão Menor
                case 2363:
                    mountDamage = 16 + (MountLevel * 0.8);
                    mountMagic = 2 + (MountLevel * 0.15);
                    break;
                #endregion
                #region Urso
                case 2364:
                    mountDamage = 20 + (MountLevel * 1.0);
                    mountMagic = 3 + (MountLevel * 0.2);
                    break;
                #endregion
                #region Dente de Sabre
                case 2365:
                    mountDamage = 30 + (MountLevel * 1.5);
                    mountMagic = 3 + (MountLevel * 0.25);
                    break;
                #endregion
                #region Cavalo sem Sela N
                case 2366:
                    mountDamage = 50 + (MountLevel * 2.5);
                    mountMagic = 7 + (MountLevel * 0.5);
                    mountEvasion = 4;
                    break;
                #endregion
                #region Cavalo Fantasm N
                case 2367:
                    mountDamage = 60 + (MountLevel * 3.0);
                    mountMagic = 9 + (MountLevel * 0.6);
                    mountEvasion = 5;
                    break;
                #endregion
                #region Cavalo Leve N
                case 2368:
                    mountDamage = 70 + (MountLevel * 3.5);
                    mountMagic = 9 + (MountLevel * 0.65);
                    mountEvasion = 6;
                    break;
                #endregion
                #region Cavalo Equip N
                case 2369:
                    mountDamage = 80 + (MountLevel * 4.0);
                    mountMagic = 10 + (MountLevel * 0.7);
                    mountEvasion = 7;
                    break;
                #endregion
                #region Andaluz N
                case 2370:
                    mountDamage = 100 + (MountLevel * 5.0);
                    mountMagic = 12 + (MountLevel * 0.85);
                    mountEvasion = 8;
                    break;
                #endregion
                #region Cavalo sem Sela B
                case 2371:
                    mountDamage = 50 + (MountLevel * 2.5);
                    mountMagic = 7 + (MountLevel * 0.5);
                    mountResist = 16;
                    break;
                #endregion
                #region Cavalo Fantasm B
                case 2372:
                    mountDamage = 60 + (MountLevel * 3.0);
                    mountMagic = 9 + (MountLevel * 0.6);
                    mountResist = 20;
                    break;
                #endregion
                #region Cavalo Leve B
                case 2373:
                    mountDamage = 70 + (MountLevel * 3.5);
                    mountMagic = 9 + (MountLevel * 0.65);
                    mountResist = 24;
                    break;
                #endregion
                #region Cavalo Equip B
                case 2374:
                    mountDamage = 80 + (MountLevel * 4.0);
                    mountMagic = 10 + (MountLevel * 0.7);
                    mountResist = 28;
                    break;
                #endregion
                #region Andaluz B
                case 2375:
                    mountDamage = 100 + (MountLevel * 5.0);
                    mountMagic = 12 + (MountLevel * 0.85);
                    mountResist = 32;
                    break;
                #endregion
                #region Fenrir
                case 2376:
                    mountDamage = 110 + (MountLevel * 5.5);
                    mountMagic = 13 + (MountLevel * 0.9);
                    break;
                #endregion
                #region Dragão
                case 2377:
                    mountDamage = 120 + (MountLevel * 6);
                    mountMagic = 13 + (MountLevel * 0.9);
                    break;
                #endregion
                #region Fenrir das Sombrar
                case 2378:
                    mountDamage = 130 + (MountLevel * 6.5);
                    mountMagic = 15 + (MountLevel * 1.0);
                    mountEvasion = 6;
                    mountResist = 28;
                    break;
                #endregion
                #region Tigre de Fogo
                case 2379:
                    mountDamage = 130 + (MountLevel * 6.5);
                    mountMagic = 15 + (MountLevel * 0.9);
                    mountEvasion = 6;
                    mountResist = 28;
                    break;
                #endregion
                #region Dragão Vermelho
                case 2380:
                    mountDamage = 140 + (MountLevel * 7);
                    mountMagic = 16 + (MountLevel * 1.1);
                    mountEvasion = 8;
                    mountResist = 32;
                    break;
                #endregion
                #region Unicórnio
                case 2381:
                    mountDamage = 114 + (MountLevel * 5.7);
                    mountMagic = 13 + (MountLevel * 0.9);
                    mountEvasion = 2;
                    mountResist = 16;
                    break;
                #endregion
                #region Pegasus
                case 2382:
                    mountDamage = 114 + (MountLevel * 5.7);
                    mountMagic = 13 + (MountLevel * 0.9);
                    mountEvasion = 3;
                    mountResist = 8;
                    break;
                #endregion
                #region Unisus
                case 2383:
                    mountDamage = 114 + (MountLevel * 5.7);
                    mountMagic = 13 + (MountLevel * 0.9);
                    mountEvasion = 4;
                    mountResist = 12;
                    break;
                #endregion
                #region Grifo
                case 2384:
                    mountDamage = 118 + (MountLevel * 5.9);
                    mountMagic = 14 + (MountLevel * 0.95);
                    mountEvasion = 3;
                    mountResist = 20;
                    break;
                #endregion
                #region Hipogrifo
                case 2385:
                    mountDamage = 120 + (MountLevel * 6);
                    mountMagic = 14 + (MountLevel * 0.95);
                    mountEvasion = 4;
                    mountResist = 16;
                    break;
                #endregion
                #region Grifo Sangrento
                case 2386:
                    mountDamage = 120 + (MountLevel * 6);
                    mountMagic = 14 + (MountLevel * 0.95);
                    mountEvasion = 5;
                    mountResist = 16;
                    break;
                #endregion
                #region Svadilfari
                case 2387:
                    mountDamage = 120 + (MountLevel * 6);
                    mountMagic = 6 + (MountLevel * 0.4);
                    mountEvasion = 6;
                    mountResist = 28;
                    break;
                #endregion
                #region Sleipnir
                case 2388:
                    mountDamage = 60 + (MountLevel * 3);
                    mountMagic = 14 + (MountLevel * 0.95);
                    mountEvasion = 6;
                    mountResist = 28;
                    break;
                #endregion
                #region Helriohdon
                case 2389:
                    mountDamage = 120 + (MountLevel * 6);
                    mountMagic = 15 + (MountLevel * 1);
                    mountEvasion = 8;
                    mountResist = 28;
                    break;
                    #endregion
            }

            status.Attack += mountDamage;
            status.Magic += mountMagic;

            status.Evasion += mountEvasion;

            for (int i = 0; i < status.Resist.Length; i++) { status.Resist[i] += mountResist; }
            #endregion

            #region Equips
            for (int i = 0; i < mob.Equip.Length; i++)
            {
                SItemList itemList = Config.Itemlist[mob.Equip[i].Id];

                if (itemList.Name != "")
                {
                    byte sanc = GetItemSanc(mob.Equip[i]);
                    double sanc_value = GetItemSanValue(i, sanc);

                    status += ((itemList + mob.Equip[i].Ef) * sanc_value);

                    //trata armadura e calca
                    if (i == 2 || i == 3)
                    {
                        if (sanc == 9 || (sanc >= 230 && sanc <= 253)) //sanc eh o add do item +9 
                        {
                            status.Defense += 25;
                        }
                    }
                    //trata a bota
                    if (i == 5)
                    {
                        bootsSpeed += itemList.Ef.Where(a => a.Index == (int)ItemListEf.EF_RUNSPEED).FirstOrDefault().Value;
                        if (sanc == 9)
                        {
                            bootsSpeed += 1;
                        }
                        else if (sanc >= 230 && sanc <= 253)
                        {
                            bootsSpeed += 2;
                        }
                    }
                }
            }

            status.MoveSpeed += mob.BaseStatus.MobSpeed + mountSpeed;
            #endregion

            #region Base
            status.Str += mob.BaseStatus.Str;
            status.Int += mob.BaseStatus.Int;
            status.Dex += mob.BaseStatus.Dex;
            status.Con += mob.BaseStatus.Con;
            status.HP += mob.BaseStatus.MaxHP;
            status.MP += mob.BaseStatus.MaxMP;
            status.Defense += mob.BaseStatus.Defense + level;

            for (int i = 0; i < mob.BaseStatus.Master.Length; i++)
            {
                status.Master[i] = mob.BaseStatus.Master[i];
            }

            if (mob.ClassInfo == ClassInfo.TK)
            { // TK
                status.HP += ((level + status.Con) * 3f);
                status.MP += ((level + status.Int) * 1f);
                statusPoint += 25;
            }
            else if (mob.ClassInfo == ClassInfo.FM)
            { // FM
                status.HP += ((level + status.Con) * 1f);
                status.MP += ((level + status.Int) * 3f);
                statusPoint += 23;
            }
            else if (mob.ClassInfo == ClassInfo.BM)
            { // BM
                status.HP += ((level + status.Con) * 1f);
                status.MP += ((level + status.Int) * 2f);
                statusPoint += 26;
            }
            else if (mob.ClassInfo == ClassInfo.HT)
            { // HT
                status.HP += ((level + status.Con) * 2f);
                status.MP += ((level + status.Int) * 1f);
                statusPoint += 36;
            }
            #endregion

            #region Pontos
            switch (Config.Itemlist[mob.Equip[0].Id].Ef[1].Value)
            {
                case 1:
                    { // Mortal
                        for (int i = 0; i < level; i++)
                        {
                            if (i < 254)
                            {
                                statusPoint += 5;
                            }
                            else if (i < 299)
                            {
                                statusPoint += 10;
                            }
                            else if (i < 354)
                            {
                                statusPoint += 20;
                            }
                            else if (i < 399)
                            {
                                statusPoint += 12;
                            }

                            if (i < 199)
                            {
                                skillPoint += 3;
                            }
                            else if (i < 354)
                            {
                                skillPoint += 4;
                            }
                            else if (i < 399)
                            {
                                skillPoint += 3;
                            }

                            masterPoint += 2;
                        }

                        break;
                    }

                case 2:
                    { // Arch
                        for (int i = 0; i < level; i++)
                        {
                            if (i < 354)
                            {
                                statusPoint += 6;
                            }
                            else if (i < 399)
                            {
                                statusPoint += 12;
                            }

                            if (i < 354)
                            {
                                skillPoint += 4;
                            }
                            else if (i < 399)
                            {
                                skillPoint += 2;
                            }

                            if (i < 199)
                            {
                                masterPoint += 2;
                            }
                            else if (i < 354)
                            {
                                masterPoint += 3;
                            }
                            else if (i < 399)
                            {
                                masterPoint += 1;
                            }
                        }

                        break;
                    }

                case 3:
                    { // Celestial
                        break;
                    }

                case 4:
                    { // Sub Celestial
                        break;
                    }
            }

            statusPoint -= mob.BaseStatus.Str + mob.BaseStatus.Int + mob.BaseStatus.Dex + mob.BaseStatus.Con;
            masterPoint -= mob.BaseStatus.Master.Sum(a => a);
            Client.Character.Skill.ForEach(a =>
            {
                foreach (SSkillList itemSkill in Config.SkilList)
                {
                    skillPoint -= itemSkill.IdSkill == a ? itemSkill.PrecoDePontos : 0;
                }
            });

            mob.StatusPoint = (short)(statusPoint);
            mob.MasterPoint = (short)(masterPoint);
            mob.SkillPoint = (short)(skillPoint);
            #endregion

            #region Status para Atk
            status.Attack += (status.Str / 4) + (status.Dex / 6) + (status.Master[0] * 1.65);
            status.AttackSpeed += (status.AttackSpeed * 10) + (status.Dex / 5);
            #endregion

            #region Porcentagens
            status.HP += status.HP * (status.PHP / 100d);
            status.MP += status.MP * (status.PMP / 100d);
            #endregion

            #region Limits
            status.Attack = (status.Attack > 32000 ? 32000 : (status.Attack < 0 ? 0 : status.Attack));
            status.Defense = (status.Defense > 32000 ? 32000 : (status.Defense < 0 ? 0 : status.Defense));

            status.HP = (status.HP > 32000 ? 32000 : (status.HP < 0 ? 0 : status.HP));
            status.MP = (status.MP > 32000 ? 32000 : (status.MP < 0 ? 0 : status.MP));

            status.Str = (status.Str > 32000 ? 32000 : (status.Str < 0 ? 0 : status.Str));
            status.Int = (status.Int > 32000 ? 32000 : (status.Int < 0 ? 0 : status.Int));
            status.Dex = (status.Dex > 32000 ? 32000 : (status.Dex < 0 ? 0 : status.Dex));
            status.Con = (status.Con > 32000 ? 32000 : (status.Con < 0 ? 0 : status.Con));

            for (int i = 0; i < status.Master.Length; i++)
            {
                status.Master[i] = (status.Master[i] > 255 ? 255 : (status.Master[i] < 0 ? 0 : status.Master[i]));
            }
            for (int i = 0; i < status.Resist.Length; i++) { status.Resist[i] = (status.Resist[i] > 250 ? 250 : (status.Resist[i] < 0 ? 0 : status.Resist[i])); }

            status.Critical /= 4;
            status.Critical = status.Critical > 255 ? 255 : status.Critical;

            status.Magic /= 4;
            status.Magic = status.Magic > 65000 ? 65000 : status.Magic;

            status.AttackSpeed = (status.AttackSpeed > ushort.MaxValue ? ushort.MaxValue : (status.AttackSpeed < 0 ? 0 : status.AttackSpeed));

            status.Evasion = (status.Evasion > short.MaxValue ? short.MaxValue : (status.Evasion < 0 ? 0 : status.Evasion));

            status.MoveSpeed = (status.MoveSpeed > 6 ? 6 : (status.MoveSpeed < 1 ? 1 : status.MoveSpeed));
            #endregion

            #region Affects
            // Recalcula os buffs
            for (int i = 0; i < mob.Affects.Length; i++)
            {

                switch (mob.Affects[i].Index)
                {
                    case (byte)Affect.FrangoAssado: status.Attack += 2000; break;
                    case (byte)Affect.Divina:
                        {
                            status.HP = ((status.HP / 100) * 20) + status.HP;
                            status.MP = ((status.MP / 100) * 20) + status.MP;
                            status.Attack = ((status.Attack / 100) * 20) + status.Attack;
                            status.Magic = ((status.Magic / 100) * 10) + status.Magic;
                            break;
                        }
                    case (byte)Affect.Vigor:
                        {
                            status.HP = ((status.HP / 100) * 10) + status.HP;
                            status.MP = ((status.MP / 100) * 10) + status.MP;
                            break;
                        }
                    case (byte)Affect.EscudoMagico:
                        {
                            status.Defense += level / 3 + mob.Affects[i].Value;
                            break;
                        }
                    case (byte)Affect.ToqueAthena:
                        {
                            int value = level / 10 + mob.Affects[i].Value;

                            for (int a = 0; a < status.Master.Length; a++)
                            {
                                double tv = status.Master[a] + value;

                                if (tv > 400)
                                    tv = 400;

                                status.Master[a] = (short)tv;
                            }
                            break;
                        }
                    case (byte)Affect.ProtecaoElemental:
                        {
                            int add = (mob.Affects[i].Value + level / 4) / 10;

                            if (level >= 255)
                                add += 20;

                            status.Defense += add;

                            //resistencia
                            status.Resist[0] += add;
                            status.Resist[1] += add;
                            status.Resist[2] += add;
                            status.Resist[3] += add;
                            break;
                        }
                    case (byte)Affect.Transformacao: // Skill de BM
                        {
                            switch (mob.Equip[0].Id)
                            {
                                case 22: status.Attack += 10; status.Critical += 5; break;
                                case 23: status.HP += 10; status.HPRegen += 20; status.MPRegen += 20; status.Attack += 20; break;
                                case 24: status.HP += 5; status.Defense += 5; status.HPRegen += 20; status.MPRegen += 20; status.Attack += 30; break;
                                case 25: status.Attack += 30; break;
                                case 32: status.HP += 10; status.Defense += 5; status.HPRegen += 10; status.MPRegen += 10; status.Attack += 30; break;
                            }

                            int value = mob.Equip[0].Id == 32 ? 4 : mob.Equip[0].Id - 22;

                            int min = (int)status.Attack + Rate.TransfBonus[value].Unk;
                            int max = (int)status.Attack + Rate.TransfBonus[value].Unk2;
                            int delta = max - min;
                            int multi = delta * level / 200 + min;

                            min = (int)status.Defense + Rate.TransfBonus[value].Unk3;
                            max = (int)status.Defense + Rate.TransfBonus[value].Unk4;
                            delta = max - min;
                            multi = delta * level / 200 + min;
                            status.Defense = status.Defense * multi / 100;

                            min = (int)status.HP + Rate.TransfBonus[value].Unk5;
                            max = (int)status.HP + Rate.TransfBonus[value].Unk6;
                            delta = max - min;
                            multi = delta * level / 200 + min;
                            status.Defense = status.Defense * multi / 100;

                            status.Master[0] += 30;
                            status.Master[1] += 30;
                            status.Master[1] += 30;
                            status.Master[1] += 30;

                            status.AttackSpeed = Rate.TransfBonus[value].Unk9 + status.Attack;
                            status.MoveSpeed = Rate.TransfBonus[value].Unk7;

                            break;
                        }
                    case (byte)Affect.Velocidade: status.MoveSpeed += mob.Affects[i].Value; break;
                    case (byte)Affect.Dano: status.Attack += mob.Affects[i].Value; break;
                }
            }

            #endregion

            #region Update MOB
            mob.GameStatus.Level = mob.BaseStatus.Level;

            mob.GameStatus.Attack = (short)(status.Attack);
            mob.GameStatus.Defense = (short)(status.Defense);

            mob.GameStatus.MaxHP = (ushort)(status.HP);
            mob.GameStatus.MaxMP = (ushort)(status.MP);

            mob.GameStatus.Str = (short)(status.Str);
            mob.GameStatus.Int = (short)(status.Int);
            mob.GameStatus.Dex = (short)(status.Dex);
            mob.GameStatus.Con = (short)(status.Con);

            mob.AttackSpeed = (short)status.AttackSpeed;
            mob.Evasion = (short)(status.Evasion);

            mob.MagicIncrement = (short)(status.Magic);
            mob.Critical = (byte)(status.Critical);

            for (int i = 0; i < status.Master.Length; i++)
            {
                mob.GameStatus.Master[i] = (byte)(status.Master[i]);
            }

            for (int i = 0; i < status.Resist.Length; i++) { mob.Resistencia[i] = (byte)(status.Resist[i]); }

            mob.GameStatus.MobSpeed = (byte)status.MoveSpeed;
            #endregion

            #region Level UP
            if (UP)
            {
                mob.GameStatus.CurHP = mob.GameStatus.MaxHP;
                mob.GameStatus.CurMP = mob.GameStatus.MaxMP;
            }
            else
            {
                if (mob.GameStatus.CurHP > mob.GameStatus.MaxHP)
                {
                    mob.GameStatus.CurHP = mob.GameStatus.MaxHP;
                }

                if (mob.GameStatus.CurMP > mob.GameStatus.MaxMP)
                {
                    mob.GameStatus.CurMP = mob.GameStatus.MaxMP;
                }
            }
            #endregion

            //Salva as novas informacoes do Client
            UserMobDAO.CreateOrUpdateChar(Client.Character);

            //atualiza o status do Client
            Client.Send(P_336.New(Client));
            Client.Send(P_337.New(Client));

            //Atualiza os Affects
            Client.Send(P_3B9.New(Client));
        }

        // Retorna uma coordenada livre para respawn
        public static Coord GetFreeRespawnCoord(Map Map, Character Character)
        {
            int minX, maxX, minY, maxY;

            // Filtra a cidade
            switch (Character.Mob.CityId)
            {
                case City.Armia:
                    {
                        if (Character.Mob.BaseStatus.Level <= 35)
                        { minX = 2107; maxX = 2120; minY = 2038; maxY = 2045; }
                        else
                        { minX = 2085; maxX = 2110; minY = 2095; maxY = 2108; }
                        break;
                    }
                case City.Noutoun: { minX = 2085; maxX = 2110; minY = 2095; maxY = 2108; break; }
                default: minX = 0; maxX = 0; minY = 0; maxY = 0; break;
            }

            // Válida as coordenadas
            if (minX == 0 || maxX == 0 || minY == 0 || maxY == 0)
            {
                throw new Exception($"CityId {Character.Mob.CityId} not found");
            }

            // Prepara uma lista de coordenadas
            List<Coord> coords = new List<Coord>();

            for (int x = minX; x <= maxX; x++)
            {
                for (int y = minY; y <= maxY; y++)
                {
                    Coord coord = Map.GetCoord(x, y);

                    if (coord.CanWalk)
                    {
                        coords.Add(coord);
                    }
                }
            }
            // Remove as coordenadas que não se pode andar
            coords.RemoveAll(a => !a.CanWalk);

            // Verifica se há coordenadas disponíveis
            if (coords.Count > 0)
            {
                // Retorna uma coordenada aleatória
                return coords[Config.Random.Next(0, coords.Count)];
            }
            return null;
        }

        // Remove cliente do mundo
        public static void RemoveFromWorld(Client Client)
        {
            // Limpa cliente da sua posição
            Client.Map.GetCoord(Client.Character.Mob.LastPosition).Client = null;

            // Limpa a variável do mapa para que, no UpdateSurrounds, todos os clientes que tenham a visão deste o removam
            Client.Map = null;

            // Prepara pacote de logout
            P_165 p165 = P_165.New(Client.ClientId, LeaveVision.LogOut);

            //remove a visao do client os mobs/npcs
            foreach (var item in Client.MobView)
            {
                Client.Send(P_165.New(item.Mob.ClientId, LeaveVision.Normal));
            }
            Client.MobView.Clear();

            // Atualiza os arredores, agora sem esse cliente
            Client.Surround.UpdateSurrounds(null, null, left =>
            {
                // Varre os que estavam na visão
                left.ForEach(a =>
                {
                    switch (a)
                    {
                        case Client client2:
                            {
                                // Envia o logout para os outros clientes
                                client2.Send(p165);
                                break;
                            }
                    }
                });
            });

        }

        // Atualiza visao do cliente
        public static void UpdateFromaWorld(Client Client, P_36C p36C)
        {
            //Atualiza a visao do cliente
            P_364.UpdateClientVisaoMob(Client);

            // Atualiza os arredores
            Client.Surround.UpdateSurrounds(
                same =>
            {
                // Varre a lista
                foreach (object o in same)
                {
                    switch (o)
                    {
                        case Client client2:
                            {
                                client2.Send(p36C);
                                break;
                            }
                    }
                }
            }, entered =>
            {
                // Visão do cliente
                P_364 p364 = P_364.New(Client.Character.Mob, EnterVision.Normal);

                // Varre a lista
                foreach (object o in entered)
                {
                    switch (o)
                    {
                        case Client client2:
                            {
                                client2.Send(p364);
                                client2.Send(p36C);
                                Client.Send(P_364.New(client2.Character.Mob, EnterVision.Normal));
                                break;
                            }
                    }
                }
            }, left =>
            {
                // Visão do cliente
                P_165 p165 = P_165.New(Client.ClientId, LeaveVision.Normal);

                // Varre a lista
                foreach (object o in left)
                {
                    switch (o)
                    {
                        case Client client2:
                            {
                                client2.Send(p36C);
                                client2.Send(p165);
                                Client.Send(P_165.New(client2.ClientId, LeaveVision.Normal));
                                break;
                            }
                    }
                }
            });
        }

        // Coord que podem ser salva
        public static bool CanSaveCoord(SPosition posMob)
        {
            if ((posMob.X < 2052 || posMob.X > 2170) && (posMob.Y > 2060 || posMob.Y < 2300)) return true;
            if ((posMob.X < 1672 || posMob.X > 1707) && (posMob.Y < 2432 || posMob.Y > 2675)) return true;
            if ((posMob.X < 1966 || posMob.X > 2000) && (posMob.Y < 2448 || posMob.Y > 2476)) return true;
            if ((posMob.X < 3090 || posMob.X > 3122) && (posMob.Y < 3090 || posMob.Y > 3690)) return true;
            if ((posMob.X < 1036 || posMob.X > 1130) && (posMob.Y > 1684 || posMob.Y < 1900)) return true;

            return false;
        }

        // Obtem a posicao da skill ou traz a ultima posicao livre
        public static int GetSlotAffect(Client client, int idSkill)
        {
            int index = 0;

            SSkillList skill = Config.SkilList.Where(a => a.IdSkill == idSkill).FirstOrDefault();
            for (int i = 0; i < client.Character.Mob.Affects.Length; i++)
            {
                // Ainda nao usou o buff, entao adiciona o novo buff na ultima posicao
                if (client.Character.Mob.Affects[i].Index == 0)
                {
                    index = i; break;
                }

                // Ja esta usando a skill, entao pega a posicao da skill para atualizar ela
                if (client.Character.Mob.Affects[i].Index == skill.TipoDeEfeito)
                {
                    index = i; break;
                }
            }
            return index;
        }
    }
}