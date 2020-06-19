using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    ///  Remove um membro ao grupo size 16
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct P_37E
    {
        public SHeader Header;
        public short ClientId;
        public short unk;

        public static P_37E New(short ClientId)
        {
            P_37E tmp = new P_37E
            {
                Header = SHeader.New(0x037E, Marshal.SizeOf<P_37E>(), 0),
                ClientId = ClientId
            };
            return tmp;
        }

        public static void controller (Client client, P_37E p37e)
        {
            foreach (int id in client.Character.PartyID)
            {
                Client c = client.Channel.Clients.Where(a => a.ClientId == id).FirstOrDefault();
                c.Character.PartyID.Remove(p37e.ClientId);
                c.Send(P_37E.New((short)p37e.ClientId));
            }

            client.Character.PartyID.Clear();
            client.Send(p37e);
        }
    }
}
