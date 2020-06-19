namespace Emulator
{
    public struct InitItem
    {
        public short PosX;
        public short PosY;
        public short ItemId;
        public short Rotate;

        public static InitItem New(int index, int posX, int posY, int rotate)
        {
            InitItem tmp = new InitItem
            {
                PosX = (short)posX,
                PosY = (short)posY,
                ItemId = (short)index,
                Rotate = (short)rotate
            };
            return tmp;
        }
    }
}
