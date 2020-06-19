using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Envia o dinheiro para o inventario - size 16
    /// </summary>
    struct P_387
    {
        public SHeader Header;
        public int Valor;

        public static P_387 New(Client client, int pValor)
        {
            P_387 tmp = new P_387
            {
                Header = SHeader.New(0x0387, Marshal.SizeOf<P_387>(), client.ClientId),
                Valor = pValor
            };
            return tmp;
        }

        public static void Controller(Client client, P_387 p387)
        {
            client.Account.Storage.Gold -= p387.Valor;
            client.Character.Mob.Gold += p387.Valor;

            client.Send(P_387.New(client, p387.Valor));
        }
    }
}
