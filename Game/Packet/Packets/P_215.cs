using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Solicita Tela de seleção de personagens - size 12
    /// </summary>

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_215
    {
        // Atributos
        public SHeader Header;          // 0 a 11		= 12

        public static P_215 New(Client client)
        {
            P_215 tmp = new P_215
            {
                Header = SHeader.New(0x0116, Marshal.SizeOf<P_215>(), client.ClientId),
            };
            return tmp;
        }
    
    public static void Controller(Client client, P_215 sHeader)
        {
            // Prepara o pacote de selecao de personagem
            P_215 p215 = P_215.New(client);
           
            // Envia os pacotes para selecao de personagem
            client.Send(p215);
           
            // Atualiza o status do cliente
            client.Status = ClientStatus.Characters;

            // Limpa o grupo
            client.Character.PartyID.Clear();

            //Remove da visao de outros clientes
            Functions.RemoveFromWorld(client);
        }
    }
}
