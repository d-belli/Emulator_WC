using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    ///  Aceita a requisicao de grupo size 30
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct P_3AB
    {
        public SHeader Header;
        public short LiderId;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] NameBytes;

        public string Name
        {
            get => Functions.GetString(this.NameBytes);
            set
            {
                this.NameBytes = Config.Encoding.GetBytes(value);
                Array.Resize(ref this.NameBytes, 16);
            }
        }

        public static void controller(Client client, P_3AB p3ab)
        {
            List<Client> clientList = client.Channel.Clients;

            // traz o lider
            Client leaderID = clientList.Where(a => a.ClientId == p3ab.LiderId).FirstOrDefault();

            // se o lider nao tem grupo adiciona o lider na primeira fila
            if (leaderID.Character.PartyID.Count == 0)
            {
                leaderID.Character.PartyID.Add(leaderID.ClientId);
                SendAddParty(leaderID, leaderID, 0);
            }

            // cliente recebe todo o grupo do lider
            client.Character.PartyID = leaderID.Character.PartyID;

            int count = client.Character.PartyID.Count();
            // varre toda a lista do novo client e adiciona os no seu grupo
            for (int i = 0; i < count; i++)
            {
                Client clientParty = clientList.Where(a => a.ClientId == client.Character.PartyID[i]).FirstOrDefault();

                // adiciona os membros do grupo ao novo cliente
                SendAddParty(client, clientParty, i);

                // adiciona o novo membro para cada cliente do grupo
                SendAddParty(clientParty, client, i);
                clientParty.Character.PartyID.Add(client.ClientId);
            }

            // se adiciona no proprio grupo
            SendAddParty(client, client, 0);
        }

        private static void SendAddParty(Client LiderId, Client ClientId, int PartyID)
        {
            SStatus status = ClientId.Character.Mob.GameStatus;

            int lider = 0;
            if (PartyID == 0)
                lider = ClientId.ClientId;
            else
                lider = 30000;

            P_37D p37d = P_37D.New((short)lider, (short)status.Level, (short)status.MaxHP, (short)status.CurHP, (short)ClientId.ClientId, ClientId.Character.Mob.NameBytes, 0);

            LiderId.Send(p37d);
        }
    }
}
