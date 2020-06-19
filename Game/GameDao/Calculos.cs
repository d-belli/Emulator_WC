using Emulator;
using System;
using System.Linq;

namespace Emulador
{

    public class Calculos
    {
        //private int RandomNumber(int min, int max)
        //{
        //    Random random = new Random();
        //    return random.Next(min, max);
        //}

        ////public static int SlotExiste(Account mob)
        ////{
        ////    int existe = 0;
        ////    for (int i = 0; i < mob.Characters[mob.status].Inventory.Items.Length; i++)
        ////    {
        ////        if (mob.Characters[mob.status].Inventory.Items[i].Id == 0)
        ////        {
        ////            existe = i;
        ////            return existe;
        ////        }
        ////    }
        ////    return existe;
        ////}
        //public static bool CordExist(Account mob)
        //{
        //    bool existe = true;

        //    return existe;
        //}
       
        //public static int CalcDnSkll(Account mob, int dninicial)
        //{
        //    int att = 0;
        //    att += (mob.Characters[mob.status].BaseStatus.Level / 3);
        //    att += (mob.Characters[mob.status].BaseStatus.ThreeMastery * 2);
        //    att += dninicial;

        //    int latk = 450;

        //    int skillID = (69) % 24;
        //    if ((mob.Characters[mob.status].LearnedSkill & (1 << skillID)) != 0)
        //    {
        //        latk += 60;
        //    }

        //    if (att > latk)
        //        att = latk;
        //    return att;
        //}
        
        //public static int Começo(Account mob)
        //{
        //    int Começo = 0;
        //    if (mob.status == 0)
        //    { Começo = 0; }
        //    else if (mob.status == 1)
        //    { Começo = 16; }
        //    else if (mob.status == 2)
        //    { Começo = 33; }
        //    else if (mob.status == 3)
        //    { Começo = 50; }
        //    return Começo;
        //}
        //public static int Fim(Account mob)
        //{
        //    int Fim = 0;
        //    if (mob.status == 0)
        //    { Fim = 15; }
        //    else if (mob.status == 1)
        //    { Fim = 32; }
        //    else if (mob.status == 2)
        //    { Fim = 49; }
        //    else if (mob.status == 3)
        //    { Fim = 66; }
        //    return Fim;
        //}
        //public static int CalcSkill(SMob Mob, int master, Int32 magic)
        //{
        //    int damage = 0;

        //    damage += ((((Mob.BaseStatus.Int / 25) * 8) * (magic + 100)) / 100); // 16 6

        //    switch (master)
        //    {
        //        case 0:
        //            damage += ((((Mob.BaseStatus.OneMastery * 6) / 4) * ((magic * 2) + 100)) / 100);
        //            break;

        //        case 1:
        //            damage += ((((Mob.BaseStatus.TwoMastery * 6) / 4) * ((magic * 2) + 100)) / 100);
        //            break;

        //        case 2:
        //            damage += ((((Mob.BaseStatus.WeaponMastery * 6) / 4) * ((magic * 2) + 100)) / 100);
        //            break;
        //    }
        //    damage += ((Mob.BaseStatus.Level * 4) / 10);
        //    return damage;
        //}
        //public static Int16 CalculoAnct(SMob mob, int mnt)
        //{
        //    Int16 AnctCode = 0;
        //    if (mob.Equip.Items[mnt].EF1 == 120)
        //    {
        //        return AnctCode = 21;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 121)
        //    {
        //        return AnctCode = 22;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 122)
        //    {
        //        return AnctCode = 28;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 123)
        //    {
        //        return AnctCode = 24;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 124)
        //    {
        //        return AnctCode = 23;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 125)
        //    {
        //        return AnctCode = 42;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 116)
        //    {
        //        return AnctCode = 17;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 117)
        //    {
        //        return AnctCode = 18;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 118)
        //    {
        //        return AnctCode = 19;
        //    }
        //    else if (mob.Equip.Items[mnt].EF1 == 119)
        //    {
        //        return AnctCode = 20;
        //    }
        //    else if (mnt == 14)
        //    {
        //        return AnctCode = mob.Equip.Items[14].EFV3;
        //    }

        //    else if (mob.Equip.Items[mnt].EFV1 == 230
        //            || mob.Equip.Items[mnt].EFV1 == 234
        //            || mob.Equip.Items[mnt].EFV1 == 238
        //            || mob.Equip.Items[mnt].EFV1 == 242
        //            || mob.Equip.Items[mnt].EFV1 == 246
        //            || mob.Equip.Items[mnt].EFV1 == 250)
        //    {
        //        return AnctCode = 16;
        //    }
        //    else if (mob.Equip.Items[mnt].EFV1 == 231
        //        || mob.Equip.Items[mnt].EFV1 == 235
        //        || mob.Equip.Items[mnt].EFV1 == 239
        //        || mob.Equip.Items[mnt].EFV1 == 243
        //        || mob.Equip.Items[mnt].EFV1 == 247
        //        || mob.Equip.Items[mnt].EFV1 == 251)
        //    {
        //        return AnctCode = 43;
        //    }
        //    else if (mob.Equip.Items[mnt].EFV1 == 232
        //        || mob.Equip.Items[mnt].EFV1 == 236
        //        || mob.Equip.Items[mnt].EFV1 == 240
        //        || mob.Equip.Items[mnt].EFV1 == 244
        //        || mob.Equip.Items[mnt].EFV1 == 248
        //        || mob.Equip.Items[mnt].EFV1 == 252)
        //    {
        //        return AnctCode = 48;
        //    }
        //    else if (mob.Equip.Items[mnt].EFV1 == 233
        //        || mob.Equip.Items[mnt].EFV1 == 237
        //        || mob.Equip.Items[mnt].EFV1 == 241
        //        || mob.Equip.Items[mnt].EFV1 == 245
        //        || mob.Equip.Items[mnt].EFV1 == 249
        //        || mob.Equip.Items[mnt].EFV1 == 253)
        //    {
        //        return AnctCode = 64;
        //    }
        //    return AnctCode;
        //}

        //public static UInt16 CalculoEfeito(SItem Item, int mnt)
        //{
        //    int value = 0;

        //    if (mnt == 14)
        //    {// montaria
        //        if (Item.Id == 0)
        //            return 0;
        //        else if (Item.EFV1 == 0)
        //            return (UInt16)(Item.Id);
        //        else if (Item.EF2 <= 8)
        //            return (UInt16)(Item.Id + (0 * 0x1000));
        //        else if (Item.EF2 <= 16)
        //            return (UInt16)(Item.Id + (1 * 0x1000));
        //        else if (Item.EF2 <= 24)
        //            return (UInt16)(Item.Id + (2 * 0x1000));
        //        else if (Item.EF2 <= 32)
        //            return (UInt16)(Item.Id + (3 * 0x1000));
        //        else if (Item.EF2 <= 40)
        //            return (UInt16)(Item.Id + (4 * 0x1000));
        //        else if (Item.EF2 <= 48)
        //            return (UInt16)(Item.Id + (5 * 0x1000));
        //        else if (Item.EF2 <= 56)
        //            return (UInt16)(Item.Id + (6 * 0x1000));
        //        else if (Item.EF2 <= 64)
        //            return (UInt16)(Item.Id + (7 * 0x1000));
        //        else if (Item.EF2 <= 72)
        //            return (UInt16)(Item.Id + (8 * 0x1000));
        //        else if (Item.EF2 <= 80)
        //            return (UInt16)(Item.Id + (9 * 0x1000));
        //        else if (Item.EF2 <= 88)
        //            return (UInt16)(Item.Id + (10 * 0x1000));
        //        else if (Item.EF2 <= 96)
        //            return (UInt16)(Item.Id + (11 * 0x1000));
        //        else if (Item.EF2 <= 104)
        //            return (UInt16)(Item.Id + (12 * 0x1000));
        //        else if (Item.EF2 <= 112)
        //            return (UInt16)(Item.Id + (13 * 0x1000));
        //        else if (Item.EF2 <= 119)
        //            return (UInt16)(Item.Id + (14 * 0x1000));
        //        else if (Item.EF2 >= 120)
        //            return (UInt16)(Item.Id + (15 * 0x1000));

        //    }
        //    else
        //    {
        //        if (Item.Id == 0)
        //            return 0;
        //        if (Item.EF1 == 43 || (Item.EF1 >= 116 && Item.EF1 <= 125))
        //            value = Item.EFV1;
        //        else
        //            return (UInt16)(Item.Id);
        //    }

        //    if (value == 0)
        //        return (UInt16)(Item.Id);
        //    else if (value <= 1)
        //        value = 1;
        //    else if (value <= 2)
        //        value = 2;
        //    else if (value <= 3)
        //        value = 3;
        //    else if (value <= 4)
        //        value = 4;
        //    else if (value <= 5)
        //        value = 5;
        //    else if (value <= 6)
        //        value = 6;
        //    else if (value <= 7)
        //        value = 7;
        //    else if (value <= 8)
        //        value = 8;
        //    else if (value <= 9)
        //        value = 9;
        //    else if (value >= 230 && value <= 233)
        //        value = 10;
        //    else if (value >= 234 && value <= 237)
        //        value = 11;
        //    else if (value >= 238 && value <= 241)
        //        value = 12;
        //    else if (value >= 242 && value <= 245)
        //        value = 13;
        //    else if (value >= 246 && value <= 249)
        //        value = 14;
        //    else if (value >= 250 && value <= 253)
        //        value = 15;
        //    else
        //        return (UInt16)(Item.Id);

        //    return (UInt16)(Item.Id + (value * 0x1000));
        //}
        //public static UInt16 CalcCritico(SMob Player)
        //{
        //    Random RandCritico = new Random();
        //    int RandCritico2 = RandCritico.Next(0, 140);
        //    UInt16 Critico = 0;

        //    if (RandCritico2 <= Player.Critical)
        //    {
        //        Critico = 2;
        //    }
        //    else
        //    {
        //        Critico = 0;
        //    }

        //    return Critico;
        //}
        //public static short exerceDanoMag(short danoAtaqueMag, SMob paramMobAtacado)
        //{

        //    return danoAtaqueMag;
        //}
        //public static short exerceDano(short danoAtaque, SMob paramMobAtacado, int Critico)
        //{

        //    int dif = ((danoAtaque * 2) / 100);

        //    int min = (danoAtaque - dif);
        //    int max = (danoAtaque + dif);

        //    int MAX_MIN = (max - min);

        //    if (MAX_MIN > 0)
        //    {

        //        Random random = new Random();
        //        int ramdom2 = (random.Next() % MAX_MIN);

        //        if (ramdom2 > ((MAX_MIN / 3) * 2))
        //            danoAtaque = (short)(danoAtaque - (short)(ramdom2));
        //        else
        //            danoAtaque = (short)(danoAtaque + (short)(ramdom2));
        //    }
        //    else
        //        MAX_MIN = 0;

        //    short Atk = (short)(danoAtaque - (((paramMobAtacado.BaseStatus.Defense / 3) * 2) + (paramMobAtacado.BaseStatus.MaxHP / 100)));
        //    if (Atk <= 0)
        //        Atk = 0;

        //    if (Atk >= 32000)
        //        Atk = 32000;

        //    /* danoAtaque = (short)(danoAtaque - paramMobAtacado.BaseStatus.Defense);
        //     if (danoAtaque < 0)
        //         danoAtaque = 0;*/
        //    if (Atk == 0)
        //        Atk = 1;
        //    if (Critico == 2)
        //    {
        //        Atk = (short)(Atk * 1.4);
        //    }
        //    return Atk;

        //}

        //public static short exerceDanoPVP(short danoAtaque, SMob paramMobAtacado)
        //{

        //    int dif = ((danoAtaque * 4) / 250);

        //    int min = (danoAtaque - dif);
        //    int max = (danoAtaque + dif);

        //    int MAX_MIN = (max - min);

        //    if (MAX_MIN > 0)
        //    {

        //        Random random = new Random();
        //        int ramdom2 = (random.Next() % MAX_MIN);

        //        if (ramdom2 > ((MAX_MIN / 6) * 2))
        //            danoAtaque = (short)(danoAtaque - (short)(ramdom2));
        //        else
        //            danoAtaque = (short)(danoAtaque + (short)(ramdom2));
        //    }
        //    else
        //        MAX_MIN = 0;

        //    short Atk = (short)(danoAtaque - (((paramMobAtacado.BaseStatus.Defense / 7) * 9) + ((paramMobAtacado.BaseStatus.MaxHP / 100) * 6)));
        //    if (Atk <= 0)
        //        Atk = 0;

        //    if (Atk >= 32000)
        //        Atk = 32000;

        //    /* danoAtaque = (short)(danoAtaque - paramMobAtacado.BaseStatus.Defense);
        //     if (danoAtaque < 0)
        //         danoAtaque = 0;*/
        //    if (Atk == 0)
        //        Atk = 1;
        //    return Atk;

        //}

        //public static short danoTotalAdd(Single paramItem)
        //{

        //    return retornaAdicionalGenerico(paramItem, "EF_DAMAGE", "2");

        //}

        //public static short defesaTotalAdd(SItem paramItem)
        //{
        //    return retornaAdicionalGenerico(paramItem, "EF_AC", "3");

        //}

        //public static short criticoTotalAdd(SItem paramItem)
        //{
        //    return (short)((double)retornaAdicionalGenerico(paramItem, "EF_CRITICAL", "71") / (short)4);
        //}


        //public static short velocidadeTotalAdd(SItem paramItem)
        //{
        //    return retornaAdicionalGenerico(paramItem, "EF_ATTSPEED", "26");
        //}

        ///*public static short velocidadeDeAndarTotalAdd(Item paramItem)
        //{
        //    ItemRead it = ReadItemList.getItemPorId(paramItem.Id);
        //    short Add = 0, refine = 0;
        //    foreach (addItem ai in it.add)
        //    {
        //        if (ai == null)
        //            break;

        //        if (ai.type == paramEfNat)
        //        {
        //            Add = (short)(Add + (short)ai.value);
        //        }
        //    }

        //    if (paramItem.EF1.ToString().Trim().Equals(paramEfAdd))
        //        Add = (short)(Add + ((short)paramItem.EFV1));
        //    else
        //        if (paramItem.EF2.ToString().Trim().Equals(paramEfAdd))
        //            Add = (short)(Add + ((short)paramItem.EFV2));
        //        else
        //            if (paramItem.EF3.ToString().Trim().Equals(paramEfAdd))
        //                Add = (short)(Add + ((short)paramItem.EFV3));

        //    return retornaAdicionalGenerico(paramItem, "EF_ATTSPEED", "26");
        //}
        //*/
        //public static short retornaAdicionalGenerico(SItem paramItem, String paramEfNat, String paramEfAdd)
        //{
        //    ItemRead it = ReadItemList.getItemPorId(paramItem.Id);
        //    short Add = 0, refine = 0;
        //    foreach (addItem ai in it.add)
        //    {
        //        if (ai == null)
        //            break;

        //        if (ai.type == paramEfNat)
        //        {
        //            Add = (short)(Add + (short)ai.value);
        //        }
        //    }

        //    Add = (short)paramItem.Ef.Where(a => a.Type.ToString() == paramEfAdd).FirstOrDefault().Value;

        //    Add = (short)paramItem.Ef.Where(a => a.Type.ToString() == "43").FirstOrDefault().Value;

        //    Add = (short)(Add * (((double)retornaPorcentagem(paramItem.EFV1) / 10) + 1));


        //    if (paramItem.EF1.ToString().Trim().Equals("43"))
        //    {
        //        Add = (short)(Add * (((double)retornaPorcentagem(paramItem.EFV1) / 10) + 1));
        //    }
        //    else
        //        if (paramItem.EF2.ToString().Trim().Equals("43"))
        //        {
        //            Add = (short)(Add * (((double)retornaPorcentagem(paramItem.EFV2) / 10) + 1));
        //        }
        //        else
        //            if (paramItem.EF3.ToString().Trim().Equals("43"))
        //            {
        //                Add = (short)(Add * (((double)retornaPorcentagem(paramItem.EFV3) / 10) + 1));
        //            }

        //    return Add;
        //}

        //public static UInt32 magicoTotalAdd(SItem paramItem)
        //{
        //    ItemRead it = ReadItemList.getItemPorId(paramItem.Id);
        //    UInt32 Add = 0, retorno = 0;
        //    byte refina = 0;

        //    //Verifica ADD natural
        //    foreach (addItem ai in it.add)
        //    {
        //        if (ai == null)
        //            break;

        //        if (ai.type == "EF_MAGIC")
        //        {
        //            Add = (UInt32)(Add + (UInt32)ai.value);
        //        }
        //    }

        //    //Verifica ADD
        //    if (paramItem.EF1 == 60)
        //    {
        //        Add = (UInt32)(Add + paramItem.EFV1);
        //    }
        //    else
        //        if (paramItem.EF2 == 60)
        //        {
        //            Add = (UInt32)(Add + paramItem.EFV2);
        //        }
        //        else
        //            if (paramItem.EF3 == 60)
        //            {
        //                Add = (UInt32)(Add + paramItem.EFV3);
        //            }

        //    retorno = Add;
        //    double perc = 0;
        //    //Verifica REFINAÇÃO
        //    if (paramItem.EF1 == 43)
        //    {
        //        refina = paramItem.EFV1;
        //        if (retornaRefinacao(paramItem.EFV1) <= 10)
        //        {
        //            retorno = (UInt32)(Add * (double)(1 + ((double)retornaRefinacao(paramItem.EFV1) / 10)));
        //        }
        //        else
        //        {
        //            if (retornaRefinacao(paramItem.EFV1) == 11)
        //                perc = 0.2;
        //            else
        //                if (retornaRefinacao(paramItem.EFV1) == 12)
        //                    perc = 0.25;
        //                else
        //                    if (retornaRefinacao(paramItem.EFV1) == 13)
        //                        perc = 0.27;
        //                    else
        //                        if (retornaRefinacao(paramItem.EFV1) == 14)
        //                            perc = 0.30;
        //                        else
        //                            if (retornaRefinacao(paramItem.EFV1) == 15)
        //                                perc = 0.34;
        //            retorno = (UInt32)((Add + Add) + ((double)perc * Add) * (retornaRefinacao(paramItem.EFV1) - 10));

        //        }
        //    }
        //    else
        //        if (paramItem.EF2 == 43)
        //        {
        //            refina = paramItem.EFV2;
        //            if (retornaRefinacao(paramItem.EFV2) <= 10)
        //            {

        //                retorno = (UInt32)(Add * (double)(1 + ((double)retornaRefinacao(paramItem.EFV2) / 10)));
        //            }
        //            else
        //            {
        //                if (retornaRefinacao(paramItem.EFV2) == 11)
        //                    perc = 0.2;
        //                else
        //                    if (retornaRefinacao(paramItem.EFV2) == 12)
        //                        perc = 0.25;
        //                    else
        //                        if (retornaRefinacao(paramItem.EFV2) == 13)
        //                            perc = 0.27;
        //                        else
        //                            if (retornaRefinacao(paramItem.EFV2) == 14)
        //                                perc = 0.30;
        //                            else
        //                                if (retornaRefinacao(paramItem.EFV2) == 15)
        //                                    perc = 0.34;
        //                retorno = (UInt32)((Add + Add) + ((double)perc * Add) * (retornaRefinacao(paramItem.EFV2) - 10));
        //            }
        //        }
        //        else
        //            if (paramItem.EF3 == 43)
        //            {
        //                refina = paramItem.EFV3;
        //                if (retornaRefinacao(paramItem.EFV3) <= 10)
        //                {
        //                    retorno = (UInt32)(Add * (double)(1 + ((double)retornaRefinacao(paramItem.EFV3) / 10)));
        //                }
        //                else
        //                {
        //                    if (retornaRefinacao(paramItem.EFV3) == 11)
        //                        perc = 0.2;
        //                    else
        //                        if (retornaRefinacao(paramItem.EFV3) == 12)
        //                            perc = 0.25;
        //                        else
        //                            if (retornaRefinacao(paramItem.EFV3) == 13)
        //                                perc = 0.27;
        //                            else
        //                                if (retornaRefinacao(paramItem.EFV3) == 14)
        //                                    perc = 0.30;
        //                                else
        //                                    if (retornaRefinacao(paramItem.EFV3) == 15)
        //                                        perc = 0.34;
        //                    retorno = (UInt32)((Add + Add) + ((double)perc * Add) * (retornaRefinacao(paramItem.EFV3) - 10));
        //                }
        //            }

        //    //MessageBox.Show(retorno+"");
        //    return retorno;
        //}


        //public static short magicoTotalMontaria(Int16 idMont, byte levelMont)
        //{
        //    short totalMago = 0;
        //    double percMont = 0;//variavel utilizada para setar a constante da montaria
        //    switch (idMont)
        //    {
        //        case 2362:
        //            {
        //                percMont = 0.1;
        //                totalMago = (short)(percMont * levelMont);
        //                break;
        //            }
        //        default:
        //            {
        //                //cria
        //                break;
        //            }
        //    }

        //    return totalMago;
        //}
        //private static byte retornaPorcentagem(byte paramEfv)
        //{
        //    byte retorno = 1;

        //    if (paramEfv <= 9)
        //        retorno = paramEfv;
        //    else
        //        if (paramEfv >= 230 && paramEfv <= 233)
        //            retorno = 10; //porcentagem +10 (retorno/10)+1
        //        else
        //            if (paramEfv >= 234 && paramEfv <= 237)
        //                retorno = 12;//porcentagem+11 (retorno/10)+1
        //            else
        //                if (paramEfv >= 238 && paramEfv <= 241)
        //                    retorno = 15;//porcentagem+12 (retorno/10)+1
        //                else
        //                    if (paramEfv >= 242 && paramEfv <= 245)
        //                        retorno = 18;//porcentagem+13 (retorno/10)+1
        //                    else
        //                        if (paramEfv >= 246 && paramEfv <= 249)
        //                            retorno = 22;//porcentagem+14 (retorno/10)+1
        //                        else
        //                            if (paramEfv >= 249 && paramEfv <= 252)
        //                                retorno = 27;//porcentagem+14 (retorno/10)+1
        //    return retorno;
        //}

        //public static byte retornaRefinacao(byte paramEfv)
        //{
        //    if (paramEfv >= 230 && paramEfv <= 233)
        //    {
        //        paramEfv = 10;
        //    }
        //    else
        //        if (paramEfv >= 234 && paramEfv <= 237)
        //            paramEfv = 11;
        //        else
        //            if (paramEfv >= 238 && paramEfv <= 241)
        //                paramEfv = 12;
        //            else
        //                if (paramEfv >= 242 && paramEfv <= 245)
        //                    paramEfv = 13;
        //                else
        //                    if (paramEfv >= 246 && paramEfv <= 249)
        //                        paramEfv = 14;
        //                    else
        //                        if (paramEfv >= 249 && paramEfv <= 252)
        //                            paramEfv = 15;

        //    return paramEfv;
        //}

    }
}
