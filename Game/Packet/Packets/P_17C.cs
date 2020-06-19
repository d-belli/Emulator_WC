using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
    /// Carrega o iventario do NPC - size 235
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct P_17C
    {
        SHeader Header;                 //0 a 11        = 12
        public ShopType ShopType;       //12 a 13       = 2
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 27)]
        public SItem[] Inventory;       //14 a 230      = 216
        public int Tax;                 //231 a 235     = 4  

        public static P_17C New(SMob mob, ShopType janela)
        {
            P_17C tmp = new P_17C
            {
                Header = SHeader.New(0x017c, Marshal.SizeOf<P_17C>(), mob.ClientId),
                ShopType = janela,
                Inventory = new SItem[27],
                Tax = 5,
            };

            int count = 0;
            SItem[] inventario = mob.Inventory.Where(a => a.Id != 0).ToArray();
            if (janela == ShopType.Skill)
                for (int i = 0; i < inventario.Length; i++)
                {
                    if (i == 8 || i == 16)
                    {
                        tmp.Inventory[i + count] = new SItem();
                        count = count + 1;
                        tmp.Inventory[i + count] = inventario[i];
                    }
                    else
                        tmp.Inventory[i + count] = inventario[i];
                }
            else
                for (int i = 0; i < inventario.Length; i++) { tmp.Inventory[i] = inventario[i]; }

            return tmp;
        }
    }
}
