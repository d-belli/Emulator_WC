using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Envia o dinheiro para o bau- size 16
    /// </summary>
    struct P_388
    {
        public SHeader Header;
        public int Gold;

        public static P_388 New(Client client, int pValor)
        {
            P_388 tmp = new P_388
            {
                Header = SHeader.New(0x0388, Marshal.SizeOf<P_388>(), client.ClientId),
                Gold = pValor
            };
            return tmp;
        }

        public static void Controller(Client client, P_388 p388)
        {
            client.Character.Mob.Gold -= p388.Gold;
            client.Account.Storage.Gold += p388.Gold;

            client.Send(P_388.New(client, p388.Gold));
        }
    }
}
