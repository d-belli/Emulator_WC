using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;

namespace Emulator
{
    /// <summary>
    /// Uso de Item - size 168
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_373
    {
        public SHeader Header;
        public int SourType;
        public int SourSlot;
        public int DestType;
        public int DestSlot;

        public SPosition Grid;
        public short WarpID;

        public static void controller(Client client, P_373 p373)
        {
            SMob mob = client.Character.Mob;
            SItem itemSource = SItem.New();
            bool level = false;
            switch (p373.SourType)
            {
                case 0: itemSource = mob.Equip[p373.SourSlot]; break;
                case 1: itemSource = mob.Inventory[p373.SourSlot]; break;
                case 2: itemSource = client.Account.Storage.Item[p373.SourSlot]; break;
            }

            SItem itemDest = SItem.New();
            switch (p373.DestType)
            {
                case 0: itemDest = mob.Equip[p373.DestSlot]; break;
                case 1: itemDest = mob.Inventory[p373.DestSlot]; break;
                case 2: itemDest = client.Account.Storage.Item[p373.DestSlot]; break;
            }

            SItemList itemSourcetList = Config.Itemlist[itemSource.Id];
            SItemList itemDestList = Config.Itemlist[itemDest.Id];

            short vol = itemSourcetList.Ef.Where(a => a.Index == (short)ItemListEf.EF_VOLATILE).FirstOrDefault().Value;
            switch (vol)
            {
                case 1:     // Porcao de HP e MP
                    {
                        if (itemSourcetList.Ef.Where(a => a.Index == (short)ItemListEf.EF_HP).Count() != 0)
                            client.Character.Mob.GameStatus.CurHP += itemSourcetList.Ef.Where(a => a.Index == (short)ItemListEf.EF_HP).FirstOrDefault().Value;

                        if (itemSourcetList.Ef.Where(a => a.Index == (short)ItemListEf.EF_MP).Count() != 0)
                            client.Character.Mob.GameStatus.CurMP += itemSourcetList.Ef.Where(a => a.Index == (short)ItemListEf.EF_MP).FirstOrDefault().Value;
                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        break;
                    }
                case 4:     // PO
                case 5:     // PL
                    {
                        bool Egg = false;
                        if (itemDest.Id >= 2300 && itemDest.Id < 2330)
                            Egg = true;

                        if (itemDest.Id == 0)
                        {
                            client.Send(P_101.New("Somente para Equips"));
                            return;
                        }

                        if (Functions.GetItemSanc(itemDest) >= 6 && vol == 4)
                        {
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("Nao pode refinar items superiores a +6"));
                            return;
                        }

                        if (Functions.GetItemSanc(itemDest) >= 9)
                        {
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("Nao pode refinar items superiores a +9"));
                            return;
                        }

                        if (Functions.GetItemSanc(itemDest) == 27)
                        {
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("Nao pode refinar items superiores a +15"));
                            return;
                        } //+15

                        // Se o item nunca foi refinado adiciona o EF
                        if (Functions.GetItemSanc(itemDest) == 0)
                        {
                            itemDest.Ef[0].Type = (byte)ItemListEf.EF_SANC;
                            itemDest.Ef[0].Value = 0;
                        }

                        #region Refinar item Selado no inventário
                        if (p373.DestType == (int)TypeSlot.Inventory)
                        {
                            if (itemDest.Ef[2].Type != 43 && itemDest.Ef[2].Type < 116 || itemDest.Ef[2].Type > 125)
                            {
                                client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                                client.Send(P_101.New("Nao pode refinar mais"));
                                return;
                            }

                            if (itemDest.Id >= 1234 && itemDest.Id <= 1237 || itemDest.Id >= 1369 && itemDest.Id <= 1372 ||
                                itemDest.Id >= 1519 && itemDest.Id <= 1522 || itemDest.Id >= 1669 && itemDest.Id <= 1672 ||
                                itemDest.Id >= 1901 && itemDest.Id <= 1910 || itemDest.Id == 1714)
                            {
                                int _rd = new Random().Next(0, 100);
                                int _chance = Rate.ItemSeladoCelestial[(int)Functions.GetItemSanc(itemDest)];

                                if (_rd <= _chance)
                                {
                                    client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                                    client.Send(P_101.New("Refinacao efetuada com sucesso"));

                                    Functions.SetItemSanc(itemDest);
                                    client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                                }
                                else
                                {
                                    client.Send(P_101.New("Falha na Refinacao"));
                                    client.Character.Mob.RemoveItemToCharacter(client, (TypeSlot)p373.DestType, p373.DestSlot);
                                }
                                Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                                return;
                            }

                            client.Send(P_101.New("Somente para Equips"));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                            return;
                        }
                        #endregion

                        if (p373.DestType == (int)TypeSlot.Equip && p373.DestSlot > 11 && p373.DestSlot < 15 &&
                            itemDest.Id != 753 && itemDest.Id != 769 && itemDest.Id != 1726 && !Egg)
                        {
                            client.Send(P_101.New("Somente para Equips"));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                            return;
                        }

                        #region Pedras Arch
                        if (itemDest.Id >= 1752 && itemDest.Id <= 1759)
                        {
                            int _rd = new Random().Next(0, 100);
                            short NextPedra = 1744;
                            int vaRateSucess = 0;

                            if (itemDest.Id == 1752)//Orc Tropper
                            {
                                if (_rd < 56)
                                    NextPedra = 1744;//Inteligencia

                                else if (_rd < 80)
                                    NextPedra = 1745;//Sabedoria

                                else if (_rd < 90)
                                    NextPedra = 1746;//Misericórdia

                                else if (_rd < 93)
                                    NextPedra = 1747;//Abismo

                                vaRateSucess = 93;
                            }
                            else if (itemDest.Id == 1753)//Esqueleto
                            {
                                if (_rd < 21)
                                    NextPedra = 1744;//Inteligencia

                                else if (_rd < 76)
                                    NextPedra = 1745;//Sabedoria

                                else if (_rd < 86)
                                    NextPedra = 1746;//Misericórdia

                                else if (_rd < 90)
                                    NextPedra = 1747;//Abismo

                                vaRateSucess = 90;
                            }
                            else if (itemDest.Id == 1754)//Dragão Lich
                            {
                                if (_rd < 3)
                                    NextPedra = 1744;//Inteligencia

                                else if (_rd < 21)
                                    NextPedra = 1745;//Sabedoria

                                else if (_rd < 76)
                                    NextPedra = 1746;//Misericórdia

                                else if (_rd < 85)
                                    NextPedra = 1747;//Abismo

                                vaRateSucess = 85;
                            }
                            else if (itemDest.Id == 1755)//DemonLord
                            {
                                if (_rd < 3)
                                    NextPedra = 1744;//Inteligencia

                                else if (_rd < 10)
                                    NextPedra = 1745;//Sabedoria

                                else if (_rd < 25)
                                    NextPedra = 1746;//Misericórdia

                                else if (_rd < 80)
                                    NextPedra = 1747;//Abismo

                                vaRateSucess = 80;
                            }
                            else if (itemDest.Id == 1756)//Manticora
                            {
                                if (_rd < 50)
                                    NextPedra = 1748;//Beleza

                                else if (_rd < 62)
                                    NextPedra = 1749;//Vitória

                                else if (_rd < 68)
                                    NextPedra = 1750;//Originalidade

                                else if (_rd < 70)
                                    NextPedra = 1751;//Reino

                                vaRateSucess = 70;
                            }
                            else if (itemDest.Id == 1757)//Gargula de fogo
                            {
                                if (_rd < 9)
                                    NextPedra = 1748;//Beleza

                                else if (_rd < 59)
                                    NextPedra = 1749;//Vitória

                                else if (_rd < 63)
                                    NextPedra = 1750;//Originalidade

                                else if (_rd < 65)
                                    NextPedra = 1751;//Reino

                                vaRateSucess = 65;
                            }
                            else if (itemDest.Id == 1758)//Lugefer
                            {
                                if (_rd < 2)
                                    NextPedra = 1748;//Beleza

                                else if (_rd < 8)
                                    NextPedra = 1749;//Vitória

                                else if (_rd < 58)
                                    NextPedra = 1750;//Originalidade

                                else if (_rd < 62)
                                    NextPedra = 1751;//Reino

                                vaRateSucess = 62;
                            }
                            else if (itemDest.Id == 1759)//DemonLord
                            {
                                if (_rd < 2)
                                    NextPedra = 1748;//Beleza

                                else if (_rd < 5)
                                    NextPedra = 1749;//Vitória

                                else if (_rd < 10)
                                    NextPedra = 1750;//Originalidade

                                else if (_rd < 60)
                                    NextPedra = 1751;//Reino

                                vaRateSucess = 60;
                            }

                            if (_rd <= vaRateSucess)
                            {
                                client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                                client.Send(P_101.New("Refinacao efetuada com sucesso"));
                                itemDest.Id = NextPedra;

                                client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                            }
                            else
                            {
                                client.Send(P_101.New("Falha na Refinacao"));
                                client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                            }
                            Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                            return;
                        }
                        #endregion

                        #region Refinar brincos +10+
                        if (Functions.GetItemSanc(itemDest) >= 9 && Functions.GetItemSanc(itemDest) <= 22 &&
                            vol == 5 && p373.DestSlot == 8 && Config.Itemlist[itemDest.Id].Slot == 256)
                        {
                            int _chance = 15;

                            if (new Random().Next(0, 100) <= _chance)
                            {
                                client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                                client.Send(P_101.New("Refinacao efetuada com sucesso"));
                                Functions.SetItemSanc(itemDest);
                                client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                            }

                            else
                            {
                                client.Send(P_101.New("Falha na Refinacao"));
                                client.Character.Mob.RemoveItemToCharacter(client, (TypeSlot)p373.DestType, p373.DestSlot);
                            }
                            Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                            return;
                        }
                        #endregion

                        #region Refinar item celestial / Hard Core
                        int itemtype = itemDestList.Ef.Where(a => a.Value == (byte)ItemListEf.EF_MOBTYPE).FirstOrDefault().Value;

                        if (itemtype == 3)
                        {
                            int _rd = new Random().Next(0, 100);
                            int _chance = Rate.ItemSeladoCelestial[(int)Functions.GetItemSanc(itemDest)];

                            if (_rd <= _chance)
                            {
                                client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                                client.Send(P_101.New("Refinacao efetuada com sucesso"));
                                Functions.SetItemSanc(itemDest);

                                client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                            }
                            else
                            {
                                client.Send(P_101.New("Falha na Refinacao"));
                                client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                            }
                            Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                            return;
                        }
                        #endregion

                        //Nyerds nao refina
                        if (itemDest.Id == 769)
                        {
                            client.Send(P_101.New("Nyerdes nao pode ser refinado"));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                            return;
                        }

                        //trata a incubacao da montaria
                        if (itemDest.Id >= 2300 && itemDest.Id < 2330 &&
                            itemDest.Ef.Where(a => a.Type == (byte)ItemListEf.EF_INCUDELAY).FirstOrDefault().Value > 0)
                        {
                            client.Send(P_101.New("O tempo de incubacao nao acabou"));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                            return;
                        }

                        int RateSucess = 0;
                        switch (itemSource.Id)
                        {
                            case 4141: RateSucess = 100; break;
                            case 412: RateSucess = Rate.PoeraOriRate[Functions.GetItemSanc(itemDest)]; break;
                            case 413: RateSucess = Rate.PoeraLacRate[Functions.GetItemSanc(itemDest)]; break;
                        }

                        int _rand = new Random().Next(0, 100);
                        if (_rand <= RateSucess)
                        {
                            client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                            Functions.SetItemSanc(itemDest);
                            client.Send(P_101.New("Refinacao efetuada com sucesso"));

                            if (itemDest.Id >= 2300 && itemDest.Id < 2330)
                            {
                                //itemDest.Ef[2].Type = (byte)ItemListEf.EF_INCUDELAY;

                                //int incurand = rand() & 0x80000003;

                                //if (incurand < 0)
                                //    incurand = ((incurand - 1) | 0xFC) + 1;

                                //itemDest.Ef[2].Value = incurand + 6;

                                //int incubate = BASE_GetBonusItemAbility(itemDest, EF_INCUBATE);

                                //if (sanc >= incubate)
                                //{
                                //    itemDest.Id += 30;

                                //    itemDest.Ef[0].Value = 20000;
                                //    itemDest.Ef[1].Type = 1;
                                //    itemDest.Ef[1].Value = rand() % 20 + 10;
                                //    itemDest.Ef[2].Type = 30;
                                //    itemDest.Ef[2].Value = 1;

                                //    SendClientMessage(conn, g_pMessageStringTable[_NN_INCUBATED]);
                                //    MountProcess(conn, 0);

                                //    client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                                //}
                            }
                            client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                        }
                        else
                        {
                            client.Send(P_101.New("Falha na Refinacao"));
                            client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);

                            //if (itemDest.Id >= 2300 && itemDest.Id < 2330)
                            //{
                            //    itemDest.Ef[2].Type = 84;

                            //    int incu = rand() & 0x80000003;
                            //    if (incu < 0)
                            //        incu = ((incu - 1) | 0xFC) + 1;

                            //    itemDest.Ef[2].Value = incu;
                            //}

                            client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                        }
                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        break;
                    }
                case 7:     // Poera de fada
                    {
                        int Level = client.Character.Mob.BaseStatus.Level;
                        int maxlevel = 400;//pMob[conn].extra.ClassMaster == MORTAL || pMob[conn].extra.ClassMaster == ARCH ? MAX_LEVEL : MAX_CLEVEL;

                        if (Level >= maxlevel)
                        {
                            client.Send(P_101.New("Limite de level atingido."));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            break;
                        }
                        if (new Random().Next(0, 100) <= 50)
                        {
                            client.Character.Mob.Exp = Rate.Exp_Mortal_Arch[Level];
                            //client.Character.Mob.BaseStatus.Level += 1;
                            level = true;
                            client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                            client.Send(P_101.New("*** Level UP !!! ***"));
                        }
                        else
                        {
                            client.Send(P_101.New("Falhou em subir de nível."));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                        }
                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        break;
                    }
                case 9:     // Adamanthida/beril etc
                    {
                        int UniqueType = -1;
                        int nUnique = itemDestList.Unique;
                        int Type = itemSource.Id - 575;

                        if (nUnique == 5 || nUnique == 14 || nUnique == 24 || nUnique == 34)
                            UniqueType = 0;

                        if (nUnique == 6 || nUnique == 15 || nUnique == 25 || nUnique == 35)
                            UniqueType = 1;

                        if (nUnique == 7 || nUnique == 16 || nUnique == 26 || nUnique == 36)
                            UniqueType = 2;

                        if (nUnique == 8 || nUnique == 17 || nUnique == 27 || nUnique == 37)
                            UniqueType = 3;

                        if (nUnique == 10 || nUnique == 20 || nUnique == 30 || nUnique == 40)
                            UniqueType = 3;

                        if (nUnique != -1 && Type == UniqueType)
                        {
                            int Grade = itemDestList.Grade;

                            if (Grade > 0 && Grade < 4)
                            {
                                int _rand = new Random().Next(0, 100);
                                if (_rand <= 50)
                                {
                                    itemDest.Id = itemDestList.Extreme;

                                    client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                                    client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                                }
                                else
                                    client.Send(P_101.New("Falha na refinacao"));
                                client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                            }
                            else
                            {
                                client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                                client.Send(P_101.New("Este item não pode ser mais refinado."));
                            }
                        }
                        else
                        {
                            string msg = "";
                            switch (itemSource.Id)
                            {
                                case 575: msg = "Disponível somente para Aço, Sombrio, Águia, Metal."; break;
                                case 576: msg = "Disponível somente para Dourado, Concha, Osso, Combatente."; break;
                                case 577: msg = "Disponível somente para Anã, Conjurador, Aeon, Natureza."; break;
                                case 578: msg = "Disponível somente para Embutido, Mithril, Elemental, Teia."; break;
                            }

                            client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                            client.Send(P_101.New(msg));
                        }
                        break;
                    }
                case 11:    // Pergaminho do Retorno
                    {
                        Coord coord = Functions.GetFreeRespawnCoord(client.Map, client.Character);
                        P_290.Teleport(client, SPosition.New(coord));
                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        break;
                    }
                case 12:    // Gema Estrelar
                    {
                        //TODO: Reformular as posicoes que podem ser salva
                        if (!Functions.CanSaveCoord(client.Character.Mob.LastPosition))
                        {
                            client.Send(P_101.New("Não está disponível salvar esta área."));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            return;
                        }

                        client.Send(P_36A.New(client, Emotion.unknow, 0));
                        client.Character.Mob.PositionSaved = client.Character.Mob.LastPosition;
                        client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                        client.Send(P_101.New("Localização foi salva."));
                        break;
                    }
                case 13:    // Pergaminho do Portal
                    {
                        P_290.Teleport(client, client.Character.Mob.PositionSaved);
                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        break;
                    }
                case 15:    // Racaoes TODO: testar
                    {
                        if (p373.DestSlot != 14)
                        {
                            client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                            client.Send(P_101.New("Nao foi possivel refinar o item com Amago"));
                            return;
                        }

                        int mount = (itemDest.Id - 2330) % 30;

                        if (mount >= 6 && mount <= 15 || mount == 27)
                            mount = 6;

                        if (mount == 19)
                            mount = 7;

                        if (mount == 20)
                            mount = 8;

                        if (mount == 21 || mount == 22 || mount == 23 || mount == 28)
                            mount = 9;

                        if (mount == 24 || mount == 25 || mount == 26)
                            mount = 10;

                        if (mount == 29)
                            mount = 19;

                        int racid = itemSource.Id >= 3367 ? itemSource.Id - 3367 : itemSource.Id - 2420;

                        int racao = racid % 30;

                        if (mount == racao)
                        {
                            if (itemDest.Ef[0].Value > 0)
                            {
                                //if ((client.Character.Mob.Equip[14].Ef[0].Value + 5000) > 30000)
                                //    client.Character.Mob.Equip[14].Ef[0].Value = 30000;
                                //else
                                //    client.Character.Mob.Equip[14].Ef[0].Value += 5000;

                                int _racao = itemDest.Ef[2].Type + 2;

                                if (_racao > 100)
                                    _racao = 100;

                                itemDest.Ef[2].Type = (byte)_racao;

                                //Cria de Montaria
                                if (itemDest.Id >= 2330 && itemDest.Id < 2360)
                                    //MountProcess(conn, 0);

                                    // Montaria Adulta
                                    if (itemDest.Id >= 2360 && itemDest.Id < 2390)
                                        //ProcessAdultMount(conn, 0);

                                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                                //931
                                //SendClientSignalParm(conn, ESCENE_FIELD, _MSG_SoundEffect, 270);
                                client.Character.Mob.AddItemToCharacter(client, itemDest, (TypeSlot)p373.DestType, p373.DestSlot);
                            }
                            else
                            {
                                client.Send(P_101.New("Montaria deve ser revivido em erion."));
                                client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                            }
                        }
                        else
                        {
                            client.Send(P_101.New("Não corresponde à montaria."));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, (TypeSlot)p373.SourType, p373.SourSlot);
                        }
                        break;
                    }
                case 19:    // Fogo de Artificio
                    {
                        client.Send(P_36A.New(client, Emotion.FogoArtificio, 10));
                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        break;
                    }
                case 58:    // Vigor 1hora
                    {
                        int pos = 0;
                        for (int i = 0; i < client.Character.Mob.Affects.Length; i++)
                        {
                            if (client.Character.Mob.Affects[i].Index == 0)
                            {
                                pos = i;
                                break;
                            }
                            if (client.Character.Mob.Affects[i].Index == (byte)ItemListEf.EF_CLAN)
                            {
                                pos = i;
                                break;
                            }
                        }

                        if (client.Character.Mob.Affects[pos].Time <= (int)Time.Hora * 30)
                        {
                            client.Character.Mob.Affects[pos].Index = (byte)ItemListEf.EF_CLAN;
                            client.Character.Mob.Affects[pos].Master = 0;
                            client.Character.Mob.Affects[pos].Value = 0;
                            client.Character.Mob.Affects[pos].Time += (int)Time.Hora;
                            client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                        }
                        else
                        {
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("Não é possível comer mais."));
                        }
                        break;
                    }
                case 63:    // Frango Assado
                    {
                        int pos = 0;
                        for (int i = 0; i < client.Character.Mob.Affects.Length; i++)
                        {
                            if (client.Character.Mob.Affects[i].Index == 0)
                            {
                                pos = i;
                                break;
                            }
                            if (client.Character.Mob.Affects[i].Index == (byte)ItemListEf.EF_SPELL)
                            {
                                pos = i;
                                break;
                            }
                        }

                        if (client.Character.Mob.Affects[pos].Time <= (int)Time.Hora * 30)
                        {
                            client.Character.Mob.Affects[pos].Index = (byte)ItemListEf.EF_SPELL;
                            client.Character.Mob.Affects[pos].Master = 0;
                            client.Character.Mob.Affects[pos].Value = 0;
                            client.Character.Mob.Affects[pos].Time += (int)Time.Hora * 4;
                            client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                        }
                        else
                        {
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("Não é possível comer mais."));
                        }

                        break;
                    }
                case 64:    // Divina 7D
                case 65:    // Divina 15D
                case 66:    // Divina 30D
                    {
                        int pos = 0;
                        for (int i = 0; i < client.Character.Mob.Affects.Length; i++)
                        {
                            if (client.Character.Mob.Affects[i].Index == 0)
                            {
                                pos = i;
                                break;
                            }
                            if (client.Character.Mob.Affects[i].Index == (byte)ItemListEf.EF_GROUND)
                            {
                                pos = i;
                                break;
                            }
                        }

                        int time = 0;
                        switch (vol)
                        {
                            case 64: time = (int)Time.Hora * 7; break;
                            case 65: time = (int)Time.Hora * 15; break;
                            case 66: time = (int)Time.Dia; break;
                        }

                        if (client.Character.Mob.Affects[pos].Time <= (int)Time.Hora * 30)
                        {
                            client.Character.Mob.Affects[pos].Index = (byte)ItemListEf.EF_GROUND;
                            client.Character.Mob.Affects[pos].Master = 0;
                            client.Character.Mob.Affects[pos].Value = 0;
                            client.Character.Mob.Affects[pos].Time += time;
                            client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                        }
                        else
                        {
                            client.Send(P_101.New("Não é possível comer mais."));
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                        }
                        break;
                    }
                case 185:   // Barra de prata
                    {
                        int gold = 0;
                        switch (itemSource.Id)
                        {
                            case 4010: gold = 100000000; break;
                            case 4011: gold = 1000000000; break;
                            case 4026: gold = 1000000; break;
                            case 4027: gold = 5000000; break;
                            case 4028: gold = 10000000; break;
                            case 4029: gold = 50000000; break;
                        }

                        client.Character.Mob.Gold += gold;
                        if (client.Character.Mob.Gold < 0)
                        {
                            client.Character.Mob.Gold -= gold;
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("O limite de 2 Bilhao de gold foi atingido!"));
                        }
                        else
                        {
                            client.Send(P_387.New(client, gold));
                            client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                        }
                        //TODO: quando envia o dinheiro, estora o hp do client, n sei o motivo ainda
                        break;
                    }
                case 186:   // tintura ou removedor de tintura
                    {
                        if (p373.DestType != (int)TypeSlot.Equip)
                        {
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("Somente pode adiciona a tintura jogando no equipamento que estiver equipado."));
                            break;
                        }

                        if (Functions.GetItemSanc(itemDest) == 0)
                        {
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("O item precisa ter refinamento superior a +0"));
                            return;
                        }

                        int color = itemSource.Id - 3407;
                        if (color == 10)
                        {
                            bool removeu = false;
                            for (int i = 0; i < itemDest.Ef.Length; i++)
                            {
                                if ((itemDest.Ef[i].Type >= 116 || itemDest.Ef[i].Type <= 125))
                                {
                                    itemDest.Ef[i].Type = (byte)ItemListEf.EF_SANC;
                                    removeu = true;
                                    break;
                                }
                            }

                            if (!removeu)
                            {
                                client.Send(P_101.New("O item nao tem nehuma tintura para ser removido."));
                                client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.DestSlot);
                                return;
                            }
                        } //Removedor
                        else
                        {
                            for (int i = 0; i < itemDest.Ef.Length; i++)
                            {
                                //Altera a cor de itens com tintura
                                if ((itemDest.Ef[i].Type >= 116 || itemDest.Ef[i].Type <= 125))
                                {
                                    itemDest.Ef[i].Type = (byte)(116 + color);
                                    client.Character.Mob.AddItemToCharacter(client, itemDest, TypeSlot.Equip, p373.DestSlot);
                                    break;
                                }
                                // Adiciona a nova tintura
                                if (itemDest.Ef[i].Type == (byte)ItemListEf.EF_SANC)
                                {
                                    itemDest.Ef[i].Type = (byte)(116 + color);
                                    client.Character.Mob.AddItemToCharacter(client, itemDest, TypeSlot.Equip, p373.DestSlot);
                                    break;
                                }
                            }
                        }

                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                        break;
                    }
                case 190:   // Replation
                    {
                        int sanc = Functions.GetItemSanc(itemDest);

                        int replation = itemSource.Id >= 4016 && itemSource.Id <= 4020 ? itemSource.Id - 4015 : itemSource.Id - 4020;
                        if (replation != itemDestList.Ef.Where(a => a.Index == (byte)ItemListEf.EF_ITEMLEVEL).FirstOrDefault().Value)
                        {
                            //atualiza o item de origem
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("Impossivel usar Repation de outra classe"));
                            break;
                        }

                        //Nao troca mas os add se for superior a +6
                        if (sanc > 6)
                        {
                            //atualiza o item de origem
                            client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                            client.Send(P_101.New("Impossivel trocar os ADD de itens superiores a +6"));
                            break;
                        }

                        if (itemDest.Ef[0].Type == (short)ItemListEf.EF_SANC)
                        {
                            if (sanc < 6)
                            {
                                sanc += new Random().Next(itemDest.Ef[0].Value, itemDest.Ef[0].Value + 1);

                                if (sanc >= 6)
                                    sanc = 6;

                                itemDest.Ef[0].Value = (byte)sanc;
                            }
                        }
                        else
                        {
                            itemDest.Ef[0].Type = (byte)ItemListEf.EF_SANC;
                            itemDest.Ef[0].Value = (byte)(new Random().Next(0, 1));
                        }

                        switch (itemDestList.Slot)
                        {
                            case 2: // Elmo
                                {
                                    int rand = new Random().Next(0, 25);
                                    itemDest.Ef[1].Type = (byte)Rate.ReplationElmo[rand, 0];
                                    itemDest.Ef[0].Value = (byte)Rate.ReplationElmo[rand, 1];

                                    itemDest.Ef[2].Type = (byte)Rate.ReplationElmo[rand, 2];
                                    itemDest.Ef[0].Value = (byte)Rate.ReplationElmo[rand, 3];
                                    break;
                                }
                            case 4: // Peito
                            case 8: // Calca
                                {
                                    int rand = new Random().Next(0, 48);
                                    itemDest.Ef[1].Type = (byte)Rate.ReplationPeitoCalca[rand, 0];
                                    itemDest.Ef[1].Value = (byte)Rate.ReplationPeitoCalca[rand, 1];

                                    itemDest.Ef[2].Type = (byte)Rate.ReplationPeitoCalca[rand, 2];
                                    itemDest.Ef[2].Value = (byte)Rate.ReplationPeitoCalca[rand, 3];
                                    break;
                                }
                            case 16: // Luva
                                {
                                    int rand = new Random().Next(0, 27);
                                    itemDest.Ef[1].Type = (byte)Rate.ReplationLuva[rand, 0];
                                    itemDest.Ef[1].Value = (byte)Rate.ReplationLuva[rand, 1];

                                    itemDest.Ef[2].Type = (byte)Rate.ReplationLuva[rand, 2];
                                    itemDest.Ef[2].Value = (byte)Rate.ReplationLuva[rand, 3];
                                    break;
                                }
                            case 32: // Bota
                                {
                                    int rand = new Random().Next(0, 30);
                                    itemDest.Ef[1].Type = (byte)Rate.ReplationBota[rand, 0];
                                    itemDest.Ef[1].Value = (byte)Rate.ReplationBota[rand, 1];

                                    itemDest.Ef[2].Type = (byte)Rate.ReplationBota[rand, 2];
                                    itemDest.Ef[2].Value = (byte)Rate.ReplationBota[rand, 3];

                                    //if (itemDest.Ef[1].Type == EF_DAMAGE && BASE_GetItemAbility(Dest, EF_DAMAGE) > 30)
                                    //{
                                    //    itemDest.Ef[1].Value -= itemDest.Ef[1].Value < (BASE_GetItemAbility(Dest, EF_DAMAGE) - 30) ? itemDest.Ef[1].Value : (BASE_GetItemAbility(Dest, EF_DAMAGE) - 30);
                                    //    if (itemDest.Ef[1].Value < 0)
                                    //        itemDest.Ef[1].Value = 0;
                                    //}

                                    //if (itemDest.Ef[2].Type == EF_DAMAGE && BASE_GetItemAbility(Dest, EF_DAMAGE) > 30)
                                    //{
                                    //    itemDest.Ef[2].Value -= itemDest.Ef[2].Value < (BASE_GetItemAbility(Dest, EF_DAMAGE) - 30) ? itemDest.Ef[2].Value : (BASE_GetItemAbility(Dest, EF_DAMAGE) - 30);
                                    //    if (itemDest.Ef[2].Value < 0)
                                    //        itemDest.Ef[2].Value = 0;
                                    //}
                                    break;
                                }
                            default: client.Send(P_101.New("Impossivel trocar o ADD")); break;
                        }


                        //atualiza o item de destino
                        client.Character.Mob.AddItemToCharacter(client, itemDest, TypeSlot.Inventory, p373.SourSlot);

                        //atualiza o item de origem
                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                        client.Send(P_36A.New(client, Emotion.LevelUP, 3));
                        break;
                    }
                case 195:   // Pergaminho de caca
                    {
                        if (p373.WarpID <= 0 || p373.WarpID > 10)
                            return;

                        int vaId = itemSource.Id - 3432;
                        SPosition pos = SPosition.New(Rate.PergaminhaCaca[vaId, p373.WarpID - 1, 0], Rate.PergaminhaCaca[vaId, p373.WarpID - 1, 1]);
                        Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                        P_290.Teleport(client, pos);

                        break;
                    }
                case 198:   // Bau Experiencia
                    {
                        for (int i = 0; i < client.Character.Mob.Affects.Length; i++)
                        {
                            if (client.Character.Mob.Affects[i].Index == 0 || client.Character.Mob.Affects[i].Index == (byte)ItemListEf.EF_BAUEXPERIENCIA)
                            {
                                if (client.Character.Mob.Affects[i].Time <= (int)Time.Hora * 28)
                                {
                                    client.Character.Mob.Affects[i].Index = (byte)ItemListEf.EF_BAUEXPERIENCIA;
                                    client.Character.Mob.Affects[i].Master = 0;
                                    client.Character.Mob.Affects[i].Value = 2;
                                    client.Character.Mob.Affects[i].Time += (int)Time.Hora * 2;
                                    Functions.SetItemAmount(client, itemSource, p373.SourSlot);
                                }
                                else
                                {
                                    client.Send(P_101.New("Não é possível comer mais."));
                                    client.Character.Mob.AddItemToCharacter(client, itemSource, TypeSlot.Inventory, p373.SourSlot);
                                }
                                break;
                            }
                        }
                        break;
                    }
                #region Poção Kappa
                case 10:
                case 55:
                case 56:
                case 52:
                case 53:
                case 57:
                case 200:
                case 201:
                case 202:
                    {
                        short value = 0;
                        int tempo = 80;

                        switch (itemSource.Id)
                        {
                            case 787: value = 1; break;//Kappa
                            case 1764: value = 2; break; //Combatente
                            case 1765: value = 3; break; //Mental
                            case 3312: value = 3; tempo = (int)Time.Hora; break; ////Mental 60m
                            case 3311: value = 2; tempo = (int)Time.Hora; break; //Combatente 60m
                            case 3310: value = 1; tempo = (int)Time.Hora / 2; break; //Kappa 60m
                            case 3321: value = 3; tempo = (int)Time.Hora * 20; break; //Mental 20h
                            case 3320: value = 2; tempo = (int)Time.Hora * 20; break; //Combatente 20h
                            case 3319: value = 1; tempo = (int)Time.Hora * 20; break; ////Kappa 20h
                        }

                        int pos = 0;
                        for (int i = 0; i < client.Character.Mob.Affects.Length; i++)
                        {
                            if (client.Character.Mob.Affects[i].Index == (byte)Affect.Pocao && client.Character.Mob.Affects[i].Value == value)
                            {
                                pos = i;
                                break;
                            }
                        }

                        if (pos == 0)
                        {
                            for (int i = 0; i < client.Character.Mob.Affects.Length; i++)
                            {
                                if (client.Character.Mob.Affects[i].Index == 0)
                                {
                                    client.Character.Mob.Affects[i].Index = (byte)Affect.Pocao;
                                    client.Character.Mob.Affects[i].Master = 0;
                                    client.Character.Mob.Affects[i].Value = value;
                                    client.Character.Mob.Affects[i].Time = tempo;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            client.Character.Mob.Affects[pos].Index = (byte)Affect.Pocao;
                            client.Character.Mob.Affects[pos].Master = 0;
                            client.Character.Mob.Affects[pos].Value = value;
                            client.Character.Mob.Affects[pos].Time = tempo;
                        }
                        client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                        break;
                    }
                    #endregion
            }

            // Bolsa do andarilho
            if (itemSource.Id == 3467)
            {
                if (client.Character.Mob.Andarilho[0].Id == 0)
                {
                    client.Character.Mob.Andarilho[0] = itemSource;
                    client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                }
                else if (client.Character.Mob.Andarilho[1].Id == 0)
                {
                    client.Character.Mob.Andarilho[1] = itemSource;
                    client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, p373.SourSlot);
                }
                else
                    client.Send(P_101.New("Voce ja esta usando as 2 bolsas de andarilho"));
                client.Send(P_185.New(client));
            }
            Functions.GetCurrentScore(client, level);
        }
    }
}
