using System;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Teletransporte - size 16
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_290
    {
        // Atributos
        public SHeader Header;        // 0 a 11		= 12
        public SPosition Dst;       // 12 a 16	= 4

        public static void Controller(Client client)
        {
            int posX = client.Character.Mob.LastPosition.X;
            int posY = client.Character.Mob.LastPosition.Y;

            if (posX >= 2116 && posY >= 2100 && posX <= 2119 && posY <= 2103) //Armia->Noatum  "X"
                Teleport(client, SPosition.New(1046, 1725));

            else if (posX >= 2140 && posY >= 2068 && posX <= 2143 && posY <= 2071) //Armia -> Campo de Treino
                Teleport(client, SPosition.New(2590, 2096));

            else if (posX >= 1044 && posY >= 1724 && posX <= 1047 && posY <= 1727)  //Noatum->Armia  "X"
                Teleport(client, SPosition.New(2116, 2100));

            else if (posX >= 1052 && posY >= 1708 && posX <= 1055 && posY <= 1711) //Noatum->Gelo  "X"
                Teleport(client, SPosition.New(3649, 3109));

            else if (posX >= 3648 && posY >= 3108 && posX <= 3651 && posY <= 3111) //Gelo->Noatum  "X"
                Teleport(client, SPosition.New(1054, 1710));

            else if (posX >= 1044 && posY >= 1716 && posX <= 1047 && posY <= 1719) //Noatum->Azram  "X"
                Teleport(client, SPosition.New(2480, 1716));

            else if (posX >= 2480 && posY >= 1716 && posX <= 2483 && posY <= 1720) //Azram -> Noatum"X"
                Teleport(client, SPosition.New(1044, 1717));
            
            else if (posX >= 2468 && posY >= 1716 && posX <= 2471 && posY <= 1719) // Azram -> Chefe de Treino
                Teleport(client, SPosition.New(2250, 1557));

            else if (posX >= 2452 && posY >= 1716 && posX <= 2455 && posY <= 1719) //Azram -> Perto da Agua
                Teleport(client, SPosition.New(1969, 1713));

            else if (posX >= 1044 && posY >= 1708 && posX <= 1047 && posY <= 1711) //Noatum->Erion  "X"
                Teleport(client, SPosition.New(2456, 2016));

            else if (posX >= 2456 && posY >= 2016 && posX <= 2459 && posY <= 2019) //Erion -> Noatum "X"
                Teleport(client, SPosition.New(1044, 1708));
                
            else if (posX >= 2452 && posY >= 1988 && posX <= 2455 && posY <= 1991) //Erion -> Campo de Erion
                Teleport(client, SPosition.New(1990, 1756));

            else if (posX >= 1056 && posY >= 1724 && posX <= 1059 && posY <= 1728) //Noatum -> Campo Deserto
                Teleport(client, SPosition.New(1157, 1718));
            //Adicionar teleporte para guerra qnd for domingo ao invez de campo de treino

            else if (posX >= 1312 && posY >= 1900 && posX <= 1315 && posY <= 1903) //Deserto->Kefra  "X"
                Teleport(client, SPosition.New(2366, 4072));

            else if (posX >= 2364 && posY >= 4072 && posX <= 2367 && posY <= 4075) //Kefra->Deserto  "X"
                Teleport(client, SPosition.New(1314, 1901));

            else if (posX >= 2364 && posY >= 3924 && posX <= 2367 && posY <= 3927)//Hall do Kefra to City Kefra
                Teleport(client, SPosition.New(3250, 1703));

            else if (posX >= 2362 && posY >= 3892 && posX <= 2370 && posY <= 3895)
                Teleport(client, SPosition.New(2366, 3910));

            else if (posX >= 2452 && posY >= 1716 && posX <= 2455 && posY <= 1719)
                Teleport(client, SPosition.New(1965, 1770));

            else if (posX >= 1108 && posY >= 1980 && posX <= 1111 && posY <= 1983)
                Teleport(client, SPosition.New(3650, 3109));

            else if (posX >= 744 && posY >= 3804 && posX <= 747 && posY <= 3807)
                Teleport(client, SPosition.New(912, 3811));

            else if (posX >= 744 && posY >= 3816 && posX <= 747 && posY <= 3819)
                Teleport(client, SPosition.New(1006, 3993));

            else if (posX >= 912 && posY >= 3808 && posX <= 915 && posY <= 3811) // Dungeon->Dungeon  "X"
                Teleport(client, SPosition.New(747, 3806));

            else if (posX >= 1516 && posY >= 3996 && posX <= 1519 && posY <= 3999)
                client.Send(P_101.New($"Estamos em Construçao Sistema de teleporte  !."));

            else if (posX >= 1056 && posY >= 1724 && posX <= 1059 && posY <= 1727)
            {
                int cHor = DateTime.Now.Hour;

                if (client.Character.Mob.Gold >= 500000)
                {
                    if (cHor >= 18 && cHor <= 19)
                    {
                        if (client.Character.Mob.Gold >= 500000)
                        {
                            client.Character.Mob.Gold -= 500000;
                            Teleport(client, SPosition.New(1160, 1726));
                        }
                        else
                            client.Send(P_101.New($"Custo do teleporte é de 500.000 gold."));
                    }
                    else
                    {
                        client.Character.Mob.Gold -= 500000;
                        Teleport(client, SPosition.New(3237, 1691));
                    }
                }

            }
        }

        public static void Teleport(Client client, SPosition sPosition)
        {
            //Adiciona posição a lista de posições
            client.Character.Positions.Clear();
            client.Character.Positions.Add(client.Character.Mob.LastPosition);

            P_36C p36C = P_36C.New(client.ClientId, client.Character.Mob.LastPosition, sPosition, 1);

            //envia para a nova coordenada
            client.Send(p36C);

            // Atualiza os arredores
            Functions.UpdateFromaWorld(client, p36C);

            client.Character.Positions.Add(sPosition);

            //Atualiza a ultima posicao para o destino
            client.Character.Mob.LastPosition = sPosition;
        }
    }
}
