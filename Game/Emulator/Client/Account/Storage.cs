using System;

namespace Emulator
{
    public class Storage
    {
        public SItem[] Item { get; set; }

        public Byte[] Unknow_1732 { get; set; }

        public int Gold { get; set; }

        public Storage()
        {
            this.Item = new SItem[120];
        }

    }
}
