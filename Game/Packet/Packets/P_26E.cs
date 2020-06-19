using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Adiciona itens no jogo tais como portao, janela objetos etc
    /// </summary>
    public struct P_26E
    {
        public SHeader Header;
        public SPosition Position;

        public short ItemId;
        public SItem Item;

        public byte Rotate;
        public byte Status;
        public byte Height;
        public byte Create;

        public static P_26E New(SPosition position, SItem item, byte rotate, Gate state)
        {
            P_26E tmp = new P_26E
            {
                Header = SHeader.New(0x026E, Marshal.SizeOf<P_26E>(), 0),
                Position = position,
                ItemId = (short)(Config.HeightMap(position) + 10000),
                Item = item,
                Rotate = rotate,
                Status = (byte)state
            };
            return tmp;
        }

        public static void controller(Client client)
        {
            foreach (InitItem item in Config.InitItemList)
            {
                SItem vaItem = SItem.New(item.ItemId);
                
                //TODO: nao aparece os objetos descobrir o motivo
                client.Send(P_26E.New(SPosition.New(item.PosX, item.PosY), vaItem, (byte)item.Rotate, Gate.Closed));
            }
        }
    }
}
