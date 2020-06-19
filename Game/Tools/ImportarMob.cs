using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace Emulator
{
    public static class ImportarMob
    {
        #region Convert Npc.bin to Npc.xml
        public static void createNpcBinToXml(string path)
        {
            try
            {
                List<string> directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();
                directories.Add(path);
                for (int i = 0; i < directories.Count(); i++)
                {
                    string[] fileEntries = Directory.GetFiles(directories[i]);

                    foreach (string fileName in fileEntries)
                    {
                        Byte[] data = File.ReadAllBytes(fileName);
                        STRUCT_MOB pMob = (STRUCT_MOB)Marshal.PtrToStructure(Marshal.UnsafeAddrOfPinnedArrayElement(data, 0), typeof(STRUCT_MOB));

                        ConvertNpcToXml(pMob, directories[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private static void ConvertNpcToXml(STRUCT_MOB NPC, string path)
        {
            path = path.Replace("mobs", @"mobs_XML");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (path.EndsWith(@"\") == false)
                path = path + @"\";

            if (NPC.name.EndsWith(".") == true)
                NPC.name = NPC.name.Substring(0, NPC.name.Length - 1);

            try
            {
ExportarToXml.ExportaToXml(convertMobBinToXml(NPC), path + NPC.name + ".xml");
            }
            catch (Exception e)
            {

                throw;
            }
            
        }

        private static SMob convertMobBinToXml(STRUCT_MOB paMob)
        {
            SMob mob = SMob.New();
            mob.Name = paMob.name;
            mob.CapaReino = paMob.Clan;
            mob.Merchant = paMob.Merchant;
            mob.GuildIndex = paMob.Guild;
            mob.ClassInfo = paMob.Class > 4 ? ClassInfo.TK : (ClassInfo)paMob.Class;
            mob.AffectInfo = paMob.resistencia;
            mob.QuestInfo = paMob.Quest;
            mob.Gold = paMob.gold;
            // mob.Unk1 = new byte[0](paMob.Unk1);
            mob.Exp = (ulong)paMob.Exp;
            mob.LastPosition = SPosition.New(paMob.SPX, paMob.SPY);
            mob.BaseStatus = new SStatus();
            mob.GameStatus = new SStatus();
            mob.Equip = new SItem[16];
            mob.Inventory = new SItem[60];
            mob.Andarilho = new SItem[2];
            //mob.Unk2 = new byte[8];
            mob.Item547 = 0;
            mob.ChaosPoints = 0;
            mob.CurrentKill = 0;
            mob.TotalKill = 0;
            //mob.Unk3 = new byte[2];
            mob.LearnedSkill = paMob.LearnedSkill;
            mob.StatusPoint = paMob.ScoreBonus;
            mob.MasterPoint = paMob.SpecialBonus;
            mob.SkillPoint = paMob.SkillBonus;
            mob.Critical = paMob.Critical;
            mob.SaveMana = paMob.SaveMana;
            mob.SkillBar1 = paMob.SkillBar;

            //mob.Unk4 = new byte[4];
            mob.Resistencia[0] = paMob.Resist[0];
            mob.Resistencia[1] = paMob.Resist[1];
            mob.Resistencia[2] = paMob.Resist[2];
            mob.Resistencia[3] = paMob.Resist[3];
            //mob.Unk5 = new byte[210];
            mob.MagicIncrement = (short)paMob.Magic;
            mob.Tab = "";


            mob.BaseStatus.Level = paMob.BaseScore.Level;
            mob.BaseStatus.Defense = paMob.BaseScore.Defesa;
            mob.BaseStatus.Attack = paMob.BaseScore.Ataque;

            mob.BaseStatus.Merchant = paMob.BaseScore.Merchante;
            mob.BaseStatus.MobSpeed = paMob.BaseScore.Speed;
            mob.BaseStatus.Direction = paMob.BaseScore.Direcao;
            mob.BaseStatus.ChaosRate = paMob.BaseScore.ChaosRate;

            mob.BaseStatus.MaxHP = paMob.BaseScore.MaxHP;
            mob.BaseStatus.MaxMP = paMob.BaseScore.MaxMP;
            mob.BaseStatus.CurHP = paMob.BaseScore.HP;
            mob.BaseStatus.CurMP = paMob.BaseScore.MP;

            mob.BaseStatus.Str = paMob.BaseScore.Str;
            mob.BaseStatus.Int = paMob.BaseScore.Int;
            mob.BaseStatus.Dex = paMob.BaseScore.Dex;
            mob.BaseStatus.Con = paMob.BaseScore.Con;



            ///
            mob.GameStatus.Level = paMob.CurrentScore.Level;
            mob.GameStatus.Defense = paMob.CurrentScore.Defesa;
            mob.GameStatus.Attack = paMob.CurrentScore.Ataque;

            mob.GameStatus.Merchant = paMob.CurrentScore.Merchante;
            mob.GameStatus.MobSpeed = paMob.CurrentScore.Speed;
            mob.GameStatus.Direction = paMob.CurrentScore.Direcao;
            mob.GameStatus.ChaosRate = paMob.CurrentScore.ChaosRate;

            mob.GameStatus.MaxHP = paMob.CurrentScore.MaxHP;
            mob.GameStatus.MaxMP = paMob.CurrentScore.MaxMP;
            mob.GameStatus.CurHP = paMob.CurrentScore.HP;
            mob.GameStatus.CurMP = paMob.CurrentScore.MP;

            mob.GameStatus.Str = paMob.CurrentScore.Str;
            mob.GameStatus.Int = paMob.CurrentScore.Int;
            mob.GameStatus.Dex = paMob.CurrentScore.Dex;
            mob.GameStatus.Con = paMob.CurrentScore.Con;

            mob.Affects = new SAffect[32];

            for (int i = 0; i < paMob.Equip.Length; i++)
            {
                mob.Equip[i].Id = paMob.Equip[i].sIndex;
                mob.Equip[i].Ef = new SItemEF[3];
                for (int a = 0; a < paMob.Equip[i].sEffect.Length; a++)
                {
                    mob.Equip[i].Ef[a].Type = paMob.Equip[i].sEffect[a].cEfeito;
                    mob.Equip[i].Ef[a].Value = paMob.Equip[i].sEffect[a].cValue;
                }
            }

            try
            {
                for (int i = 0; i < 60; i++)
                {
                    mob.Inventory[i].Id = paMob.Carry[i].sIndex;
                    mob.Inventory[i].Ef = new SItemEF[3];
                    for (int a = 0; a < paMob.Carry[i].sEffect.Length; a++)
                    {
                        mob.Inventory[i].Ef[a].Type = paMob.Carry[i].sEffect[a].cEfeito;
                        mob.Inventory[i].Ef[a].Value = paMob.Carry[i].sEffect[a].cValue;
                    }
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex); ;
            }


            return mob;
        }
        #endregion

        #region Convert NpcGener.txt to NpcGener.xml
        public static void createNpcGeneratorToXml(string path, string fileName)
        {
            ExportarToXml.ExportaToXml(convertNpcGeneratorToXml(path + fileName), path + @"\NPCGenerator.xml");
        }

        private static List<SMobList> convertNpcGeneratorToXml(string path)
        {
            StreamReader stream = new StreamReader(path);
            string linha = null;
            int i = 0;
            List<SMobList> mobList = new List<SMobList>();
            SMobList mob = SMobList.New();

            try
            {
                while ((linha = stream.ReadLine()) != null)
                {
                    if (linha == "")
                        continue;
                    if (i == 5)
                    {

                    }

                    if (linha.Substring(0, 1).Equals("#") || linha.Substring(0, 1).Equals("/") || linha.Substring(0, 1) == null)
                        continue;

                    if (linha.Contains("MaxNumMob"))
                    {
                        i = i + 1;
                        mob.Number = (short)Convert.ToInt16(linha.Split(':')[1].Substring(1, linha.Split(':')[1].Length - 1));
                    }


                    if (linha.Contains("Leader"))
                    {
                        i = i + 1;
                        mob.MobName = linha.Split(':')[1].Substring(1, linha.Split(':')[1].Length - 1).Trim();
                        mob.MobName = mob.MobName.EndsWith(".") == true ? mob.MobName.Substring(0, mob.MobName.Length - 1) : mob.MobName;
                    }

                    if (linha.Contains("StartX"))
                    {
                        i = i + 1;
                        mob.PositionInicial.X = (short)Convert.ToInt16(linha.Split(':')[1].Substring(1, linha.Split(':')[1].Length - 1));
                    }

                    if (linha.Contains("StartY"))
                    {
                        i = i + 1;
                        mob.PositionInicial.Y = (short)Convert.ToInt16(linha.Split(':')[1].Substring(1, linha.Split(':')[1].Length - 1));
                    }

                    if (linha.Contains("DestX"))
                    {
                        i = i + 1;
                        mob.PositionFinal.X = (short)Convert.ToInt16(linha.Split(':')[1].Substring(1, linha.Split(':')[1].Length - 1));
                    }

                    if (linha.Contains("DestY"))
                    {
                        i = i + 1;
                        var apenasDigitos = new Regex(@"[^\d]");
                         string var = apenasDigitos.Replace(linha.Split(':')[1].Substring(1, linha.Split(':')[1].Length - 1), "");
                        mob.PositionFinal.Y = (short)Convert.ToInt16(var);
                    }

                    if (linha.Contains("RouteType"))
                    {
                        i = i + 1;
                        mob.FreqStep = (short)Convert.ToInt16(linha.Split(':')[1].Substring(1, linha.Split(':')[1].Length - 1));
                    }

                    if (i == 9)
                    {
                        i = 1;
                        mobList.Add(mob);
                        mob = SMobList.New();
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
            return mobList;
        }
        #endregion
    }
}
