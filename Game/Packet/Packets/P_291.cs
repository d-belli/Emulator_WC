namespace Emulator
{
    /// <summary>
    /// Quando entra no mundo
    /// </summary>
    public struct P_291
    {
        public SHeader Header;
        public short unknow;
        public short unknow2;

        public static void Controller(Client client, P_291 p291)
        {
            //Atualiza seu status do client
            Functions.GetCurrentScore(client, false);

            //Atualiza a bolsa do andarilho
            client.Send(P_185.New(client));

            //Adicionar os itens do jogo tais como porta, janela e objetos
            P_26E.controller(client);
        }
    }
}
