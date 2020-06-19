
namespace Emulator
{
    public static class PControl
    {
        // Atributos
        private static readonly string Lock = "";

        // Controlador de pacotes
        public static void Controller(Client client, byte[] data)
        {
            lock (Lock)
            {
                SHeader header = PConvert.ToStruct<SHeader>(data);

                Log.Rcv(client, header);

                if (header.PacketId == 0x03A0)
                {
                    if (header.Size != 12 || data.Length != 12)
                    {
                        client.Close();
                    }

                    return;
                }

                switch (client.Status)
                {
                    case ClientStatus.Login:
                        {
                            switch (header.PacketId)
                            {
                                case 0x20D: P_20D.Controller(client, PConvert.ToStruct<P_20D>(data)); break;  // Login

                                default: client.Close(); break;
                            }

                            break;
                        }

                    case ClientStatus.Numeric:
                        {
                            switch (header.PacketId)
                            {
                                case 0xFDE: P_FDE.Controller(client, PConvert.ToStruct<P_FDE>(data)); break;  // Senha numérica

                                default: client.Close(); break;
                            }

                            break;
                        }

                    case ClientStatus.Characters:
                        {
                            switch (header.PacketId)
                            {
                                case 0x020F: P_20F.Controller(client, PConvert.ToStruct<P_20F>(data)); break; // Criar personagem
                                case 0x0211: P_211.Controller(client, PConvert.ToStruct<P_211>(data)); break; // Apagar personagem
                                case 0x0213: P_213.Controller(client, PConvert.ToStruct<P_213>(data)); break; // Entrar no mundo

                                case 0xFDE: P_FDE.Controller(client, PConvert.ToStruct<P_FDE>(data)); break;  // Alterar senha numérica

                                default: client.Close(); break;
                            }

                            break;
                        }

                    case ClientStatus.Game:
                        {
                            switch (header.PacketId)
                            {
                                case 0x0291: P_291.Controller(client, PConvert.ToStruct<P_291>(data)); break;        // Depois que entra no mundo
                                case 0x0333: P_333.Controller(client, PConvert.ToStruct<P_333>(data)); break;        // Chat aberto
                                case 0x0366: break;                                                                  // Andar porém quando para de pressionar o mouse 
                                case 0x036C: P_36C.Controller(client, PConvert.ToStruct<P_36C>(data)); break;        // Andar
                                case 0x03AE: break;                                                                  // 5 segundos
                                case 0x0215: P_215.Controller(client, PConvert.ToStruct<P_215>(data)); break;        // volta para tela de personagem
                                case 0x0290: P_290.Controller(client); break;                                        // teletransporte
                                case 0x027B: P_27B.controller(client, PConvert.ToStruct<P_27B>(data)); break;        // Abre o NPC de venda/skill
                                case 0x0379: P_379.controller(client, PConvert.ToStruct<P_379>(data)); break;        // Compra de item do NPC
                                case 0x037A: P_37A.controller(client, PConvert.ToStruct<P_37A>(data)); break;        // Vende o item para o NPC
                                case 0x0376: P_376.controller(client, PConvert.ToStruct<P_376>(data)); break;        // Move o Item
                                case 0x0387: P_387.Controller(client, PConvert.ToStruct<P_387>(data)); break;        // Envia o dinheiro para o inventario
                                case 0x0388: P_388.Controller(client, PConvert.ToStruct<P_388>(data)); break;        // Envia o dinheiro para o bau
                                case 0x0367:
                                case 0x039D: P_39D.controller(client, PConvert.ToStruct<P_39D>(data)); break;        // Ataque ao Mobs/Players (nao esta pronto)
                                //case 0x02CB: P_2CB.controller(client, PConvert.ToStruct<P_2CB>(data)); break;      // UNKNOW
                                //case 0x036B:                                                                       // unknow
                                case 0x0277: P_277.controller(client, PConvert.ToStruct<P_277>(data)); break;        // Adiciona os pontos de status e skill
                                case 0x0378: P_378.controller(client, PConvert.ToStruct<P_378>(data)); break;        // Adiciona a skill na barra de skill
                                case 0x0373: P_373.controller(client, PConvert.ToStruct<P_373>(data)); break;        // Trata o uso de item (nao esta pronto)
                                case 0x02E4: P_2E4.controller(client, PConvert.ToStruct<P_2E4>(data)); break;        // Deleta o Item
                                case 0x0334: P_334.controller(client, PConvert.ToStruct<P_334>(data)); break;        // Comando do chat
                                case 0x0369: P_369.controller(client, PConvert.ToStruct<P_369>(data)); break;        // Respaw Mob (nao esta pronto)
                                case 0x037F: P_37F.controller(client, PConvert.ToStruct<P_37F>(data)); break;        // Envia uma requicao de grupo
                                case 0x03AB: P_3AB.controller(client, PConvert.ToStruct<P_3AB>(data)); break;        // Aceita a requisicao de grupo
                                case 0x037E: P_37E.controller(client, PConvert.ToStruct<P_37E>(data)); break;        // Remove um membro do grupo
                                case 0x028B: P_28B.controller(client, PConvert.ToStruct<P_28B>(data)); break;        // Trata os npc de quest, buff e etc

                                default: client.Send(P_101.New($"UNK:0x { header.PacketId.ToString("X").PadLeft(4, '0')}")); break;
                            }

                            break;
                        }
                }
            }
        }
    }
}