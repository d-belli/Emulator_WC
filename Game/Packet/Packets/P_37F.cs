using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Envia uma requicao de grupo - size 44
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct P_37F
    {
        public SHeader Header;
        public byte clientClass;
        public byte PartyPos;

        public short clientLevel;
        public short clientMaxHp;
        public short clientHp;
        public short clientId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] NameBytes;

        public short unk;
        public short ClientTarget;
        public short unk2;

        public string Name
        {
            get => Functions.GetString(this.NameBytes);
            set
            {
                this.NameBytes = Config.Encoding.GetBytes(value);
                Array.Resize(ref this.NameBytes, 16);
            }
        }

        public static P_37F New(P_37F p37f)
        {
            P_37F tmp = new P_37F
            {
                Header = SHeader.New(0x037F, Marshal.SizeOf<P_37F>(), p37f.Header.ClientId),
                clientClass = p37f.clientClass,
                PartyPos = p37f.PartyPos,
                clientLevel = p37f.clientLevel,
                clientMaxHp = p37f.clientMaxHp,
                clientHp = p37f.clientHp,
                clientId = p37f.clientId,
                NameBytes = p37f.NameBytes,
                unk = p37f.unk,
                ClientTarget = p37f.ClientTarget,
                unk2 = p37f.unk2
            };
            return tmp;
        }

        public static void controller(Client client, P_37F p37f)
        {
            Client target = client.Channel.Clients.Where(a => a.ClientId == p37f.ClientTarget).FirstOrDefault();
            if (target.Character.PartyID.Count() != 0)
            {
                client.Send(P_101.New($"{target.Character.Mob.Name } ja possui um grupo"));
                return;
            }

            if (client.Character.PartyID[0] == p37f.clientId)
            {
                client.Send(P_101.New("Somente lider do grupo pode adicionar um membro."));
                return;
            }

            // max em um grupo eh 12 membros
            if (client.Character.PartyID.Count() <= 12)
            {
                int lvl = target.Character.Mob.GameStatus.Level;
                int leaderlv = client.Character.Mob.GameStatus.Level;

                if (lvl >= leaderlv - 400 && lvl < leaderlv + 400)
                    // envia o convite do grupo para o target
                    target.Send(P_37F.New(p37f));
                else
                    client.Send(P_101.New("Grupo é possível somente para jogadores com diferença de 200 níveis."));
            }
            else
                client.Send(P_101.New("O grupo está cheio."));
        }
    }
}
