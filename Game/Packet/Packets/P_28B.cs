using System;
using System.Linq;

namespace Emulator
{
    /// <summary>
    /// Trata os npc de quest, buff e etc
    /// </summary>
    struct P_28B
    {
        public SHeader Header;
        public int Index;
        public int unk;

        public static void controller(Client client, P_28B p28b)
        {
            SMob npc = Config.MobList.Where(a => a.Mob.ClientId == p28b.Index).FirstOrDefault().Mob;
            switch (npc.Merchant)
            {
                case 4: // Dragon Dourado
                    {
                        break;
                    }
                case 10: // Amuleto Mistico
                    {
                        break;
                    }
                case 11: // Exploit Leader | testar
                    {
                        break;
                    }
                case 12: // Jeffi
                    {
                        break;
                    }
                case 13: // Shama
                    {
                        break;
                    }
                case 14: // Rei Azul
                    {
                        break;
                    }
                case 15: // Rei Vermelho
                    {
                        break;
                    }
                case 19: // Composicao Sephi
                    {
                        break;
                    }
                case 26: // Rei
                    {
                        break;
                    }
                case 30: // Zakum
                    {
                        break;
                    }
                case 31: // Mestre Habilidade
                    {
                        break;
                    }
                case 58: // Mount Master
                    {
                        break;
                    }
                case 62: // Dragon Arzan
                    {
                        break;
                    }
                case 68: // God Government
                    {
                        break;
                    }
                case 72: // Uxamll
                    {
                        break;
                    }
                case 74: // Kibita
                    {
                        break;
                    }
                case 76: // Urnammu
                    {
                        break;
                    }
                case 78: // Black Oracle
                    {
                        break;
                    }
                case 100: // Quest Mortal
                    {
                        int npcGrade = Config.Itemlist[npc.Equip[0].Id].Grade;
                        switch (npcGrade)
                        {
                            case 0: // Coveiro
                                {
                                    int levelAtual = client.Character.Mob.GameStatus.Level;
                                    if (levelAtual < 39 || levelAtual >= 115)
                                    {
                                        client.Send(P_333.New(p28b.Index, "Nível Insuficiente. Isto não pode ser utilizado."));
                                        break;
                                    }

                                    // Verifica se existe o ticket de entrada
                                    if (client.Character.Mob.Inventory.Where(a => a.Id == 4038).Any())
                                    {
                                        int slot = 0;
                                        // Procura em qual slot o ticket esta
                                        for (int i = 0; i < client.Character.Mob.Inventory.Length; i++)
                                        {
                                            if (client.Character.Mob.Inventory[i].Id == 4038) break; else slot += 1;
                                        }

                                        // Remove o ticket
                                        client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, slot);

                                        // Envia o client para a quest
                                        P_290.Teleport(client, SPosition.New(new Random().Next(2395, 2398), new Random().Next(2102, 2105)));
                                    }
                                    else
                                        client.Send(P_333.New(p28b.Index, $"Você deve trazer o item {Config.Itemlist[4038].Name}."));

                                    break;
                                }
                            case 1: // Jardineiro
                                {
                                    int levelAtual = client.Character.Mob.GameStatus.Level;
                                    if (levelAtual < 115 || levelAtual >= 190)
                                    {
                                        client.Send(P_333.New(p28b.Index, "Nível Insuficiente. Isto não pode ser utilizado."));
                                        break;
                                    }

                                    // Verifica se existe o ticket de entrada
                                    if (client.Character.Mob.Inventory.Where(a => a.Id == 4039).Any())
                                    {
                                        int slot = 0;
                                        // Procura em qual slot o ticket esta
                                        for (int i = 0; i < client.Character.Mob.Inventory.Length; i++)
                                        {
                                            if (client.Character.Mob.Inventory[i].Id == 4039) break; else slot += 1;
                                        }

                                        // Remove o ticket
                                        client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, slot);

                                        // Envia o client para a quest
                                        P_290.Teleport(client, SPosition.New(new Random().Next(2232, 2335), new Random().Next(1712, 1716)));
                                    }
                                    else
                                        client.Send(P_333.New(p28b.Index, $"Você deve trazer o item {Config.Itemlist[4039].Name}."));
                                    break;
                                }
                            case 2: // Kaizen
                                {
                                    int levelAtual = client.Character.Mob.GameStatus.Level;
                                    if (levelAtual < 190 || levelAtual >= 265)
                                    {
                                        client.Send(P_333.New(p28b.Index, "Nível Insuficiente. Isto não pode ser utilizado."));
                                        break;
                                    }

                                    // Verifica se existe o ticket de entrada
                                    if (client.Character.Mob.Inventory.Where(a => a.Id == 4040).Any())
                                    {
                                        int slot = 0;
                                        // Procura em qual slot o ticket esta
                                        for (int i = 0; i < client.Character.Mob.Inventory.Length; i++)
                                        {
                                            if (client.Character.Mob.Inventory[i].Id == 4040) break; else slot += 1;
                                        }

                                        // Remove o ticket
                                        client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, slot);

                                        // Envia o client para a quest
                                        P_290.Teleport(client, SPosition.New(new Random().Next(462, 466), new Random().Next(3900, 3904)));
                                    }
                                    else
                                        client.Send(P_333.New(p28b.Index, $"Você deve trazer o item {Config.Itemlist[4040].Name}."));
                                    break;
                                }
                            case 3: // Hidra
                                {
                                    int levelAtual = client.Character.Mob.GameStatus.Level;
                                    if (levelAtual < 265 || levelAtual >= 320)
                                    {
                                        client.Send(P_333.New(p28b.Index, "Nível Insuficiente. Isto não pode ser utilizado."));
                                        break;
                                    }

                                    // Verifica se existe o ticket de entrada
                                    if (client.Character.Mob.Inventory.Where(a => a.Id == 4041).Any())
                                    {
                                        int slot = 0;
                                        // Procura em qual slot o ticket esta
                                        for (int i = 0; i < client.Character.Mob.Inventory.Length; i++)
                                        {
                                            if (client.Character.Mob.Inventory[i].Id == 4041) break; else slot += 1;
                                        }

                                        // Remove o ticket
                                        client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, slot);

                                        // Envia o client para a quest
                                        P_290.Teleport(client, SPosition.New(new Random().Next(666, 670), new Random().Next(3754, 3758)));
                                    }
                                    else
                                        client.Send(P_333.New(p28b.Index, $"Você deve trazer o item {Config.Itemlist[4041].Name}."));
                                    break;
                                }
                            case 4: // Elfo
                                {
                                    int levelAtual = client.Character.Mob.GameStatus.Level;
                                    if (levelAtual < 320 || levelAtual >= 350)
                                    {
                                        client.Send(P_333.New(p28b.Index, "Nível Insuficiente. Isto não pode ser utilizado."));
                                        break;
                                    }

                                    // Verifica se existe o ticket de entrada
                                    if (client.Character.Mob.Inventory.Where(a => a.Id == 4042).Any())
                                    {
                                        int slot = 0;
                                        // Procura em qual slot o ticket esta
                                        for (int i = 0; i < client.Character.Mob.Inventory.Length; i++)
                                        {
                                            if (client.Character.Mob.Inventory[i].Id == 4042) break; else slot += 1;
                                        }

                                        // Remove o ticket
                                        client.Character.Mob.RemoveItemToCharacter(client, TypeSlot.Inventory, slot);

                                        // Envia o client para a quest
                                        P_290.Teleport(client, SPosition.New(new Random().Next(1320, 1324), new Random().Next(4039, 4043)));
                                    }
                                    else
                                        client.Send(P_333.New(p28b.Index, $"Você deve trazer o item {Config.Itemlist[4042].Name}."));
                                    break;
                                }
                            case 5: // Lider Aprendiz
                                {
                                    break;
                                }
                            case 7: // Persen
                            case 8:
                            case 9:
                                {
                                    break;
                                }
                            case 13: // Capa do Reino
                                {
                                    break;
                                }
                            case 14: // Capa verde
                                {
                                    break;
                                }
                            case 15: // Molar Gargula
                                {
                                    break;
                                }
                            case 16: // Treinador quest newbie 1
                                {
                                    break;
                                }
                            case 17: // Treinador quest newbie 2
                                {
                                    break;
                                }
                            case 18: // Treinador quest newbie 3
                                {
                                    break;
                                }
                            case 19: // Treinador quest Newbie 4
                                {
                                    break;
                                }
                            case 22: // Sobrevivente
                                {
                                    break;
                                }
                            case 30: // Guarda Real
                                {
                                    break;
                                }
                        }
                        break;
                    }
                case 120: // Carbuncle buff
                    {
                        if (client.Character.Mob.GameStatus.Level >= 100)
                        {
                            client.Send(P_333.New(p28b.Index, "Nível Insuficiente. Isto não pode ser utilizado."));
                            break;
                        }

                        int id = 5040;
                        //buffa o cliente
                        for (int i = 0; i < 4; i++)
                        {
                            if (i == 1)
                                id++;

                            id++;

                            SSkillList skil = Config.SkilList.Where(a => a.IdSkill == id).FirstOrDefault();
                            client.Character.Mob.Affects[Functions.GetSlotAffect(client, id)].Index = (byte)skil.TipoDeEfeito;
                            client.Character.Mob.Affects[Functions.GetSlotAffect(client, id)].Value = (byte)skil.ValorDoEfeito;
                            client.Character.Mob.Affects[Functions.GetSlotAffect(client, id)].Time = skil.TempoDoEfeito;
                        }

                        Functions.GetCurrentScore(client, false);
                        client.Send(P_333.New(p28b.Index, $"Sente-se mais forte agora {client.Character.Mob.Name}?"));
                        break;
                    }
               
            }
        }
    }
}
