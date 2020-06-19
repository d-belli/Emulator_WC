using System;
using System.Linq;

namespace Emulator
{
    /// <summary>
    /// Adicona os pontos de status do personagem / Compra as skill do npc- size 20
    /// </summary>
    struct P_277
    {
        public SHeader Header;
        public short Mode;
        public short Info;
        public int unk;

        public static void controller(Client client, P_277 p227)
        {
            ref SMob mob = ref client.Character.Mob;
            if (mob.StatusPoint >= 1 && p227.Mode == 0)
            {
                short ponto = 1;
                if (mob.StatusPoint > 300)
                    ponto = 100;

                switch (p227.Info)
                {
                    case 0:
                        {
                            mob.BaseStatus.Str += ponto;
                            mob.StatusPoint -= ponto;
                            if (mob.BaseStatus.Str % 3 == 0)
                                mob.BaseStatus.Attack += ponto == 1 ? 1 : 33;
                            break;
                        }
                    case 1:
                        {
                            mob.BaseStatus.Int += ponto;
                            mob.StatusPoint -= ponto;
                            mob.BaseStatus.MaxMP += ponto == 1 ? 2 : 50;
                            break;
                        }
                    case 2:
                        {
                            mob.BaseStatus.Dex += ponto;
                            mob.StatusPoint -= ponto;
                            break;
                        }
                    case 3:
                        {
                            mob.BaseStatus.Con += ponto;
                            mob.StatusPoint -= ponto;
                            mob.BaseStatus.MaxHP += ponto == 1 ? 2 : 50;
                            break;
                        }
                }
                Functions.GetCurrentScore(client, false);
                return;
            }

            if (mob.MasterPoint >= 1 && p227.Mode == 1)
            {
                switch (p227.Info)
                {
                    case 0:
                        {
                            mob.BaseStatus.Master[0] += 1;
                            mob.MasterPoint -= 1;
                            mob.BaseStatus.Attack += 1;
                            break;
                        }
                    case 1:
                        {
                            mob.BaseStatus.Master[1] += 1;
                            mob.MasterPoint -= 1;
                            break;
                        }
                    case 2:
                        {
                            mob.BaseStatus.Master[2] += 1;
                            mob.MasterPoint -= 1;
                            break;
                        }
                    case 3:
                        {
                            mob.BaseStatus.Master[3] += 1;
                            mob.MasterPoint -= 1;
                            break;
                        }
                }
                Functions.GetCurrentScore(client, false);
            }
            if (p227.Mode == 2)
            {
                SSkillList pSkill = Config.SkilList.Where(a => a.IdSkill == p227.Info).FirstOrDefault();

                if ((p227.Info >= 5000 && p227.Info <= 5023) && (mob.Equip[0].Id != 1 && mob.Equip[0].Id != 6 &&
                    mob.Equip[0].Id != 16 && mob.Equip[0].Id != 26 && mob.Equip[0].Id != 36))
                    client.Send(P_101.New("Nao é possivel aprender skill de outras classes"));

                else if ((p227.Info >= 5024 && p227.Info <= 5047) && (mob.Equip[0].Id != 11 && mob.Equip[0].Id != 7
                    && mob.Equip[0].Id != 17 && mob.Equip[0].Id != 27 && mob.Equip[0].Id != 37))
                    client.Send(P_101.New("Nao é possivel aprender skill de outras classes"));

                else if ((p227.Info >= 5048 && p227.Info <= 5071) && (mob.Equip[0].Id != 21 && mob.Equip[0].Id != 8
                    && mob.Equip[0].Id != 18 && mob.Equip[0].Id != 28 && mob.Equip[0].Id != 38))
                    client.Send(P_101.New("Nao é possivel aprender skill de outras classes"));

                else if ((p227.Info >= 5072 && p227.Info <= 5095) && (mob.Equip[0].Id != 31 && mob.Equip[0].Id != 9
                    && mob.Equip[0].Id != 19 && mob.Equip[0].Id != 29 && mob.Equip[0].Id != 39))
                    client.Send(P_101.New("Nao é possivel aprender skill de outras classes"));

                else if (mob.BaseStatus.Level < ((SItemList)Config.Itemlist.GetValue(pSkill.IdSkill)).Level)
                    client.Send(P_101.New("Level insuficiente para comprar esta habilidade"));
                else
                {
                    int skillID = (p227.Info - 5000) % 24;

                    if (mob.SkillPoint >= pSkill.PrecoDePontos)
                    {
                        if (((int)mob.LearnedSkill & (1 << skillID)) == 0)
                        {
                            mob.LearnedSkill |= (UInt32)(1 << skillID);
                            mob.SkillPoint -= (Int16)(pSkill.PrecoDePontos);
                            client.Character.Skill.Add(pSkill.IdSkill);
                            Functions.GetCurrentScore(client, false);
                        }
                        else
                            client.Send(P_101.New("Você ja aprendeu esta skill"));
                    }
                    else
                        client.Send(P_101.New("Você não possui pontos o suficiente"));
                }
                if (p227.Info < 5000 || p227.Info > 5095)
                    client.Send(P_101.New("Ocorreu um Erro, contate o adm"));

            }
        }
    }
}
