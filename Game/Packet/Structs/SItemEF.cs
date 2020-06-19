using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Item effect - size 2
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SItemEF {
		// Atributos
		public byte Type;   // 0
		public byte Value;  // 1

		// Construtores
		public static SItemEF New ( ) => New ( 0 , 0 );
		public static SItemEF New ( byte Type , byte Value ) {
			SItemEF tmp = new SItemEF {
				Type = Type ,
				Value = Value
			};

			return tmp;
		}

		public static SItemEF[] New(SItemListEF[] itemListEf)
		{
			SItemEF[] tmp = new SItemEF[itemListEf.Length];
			for (int i = 0; i < itemListEf.Length; i++)
			{
				tmp[i] = SItemEF.New((byte)itemListEf[i].Index, (byte)itemListEf[i].Value);
			}

			return tmp;
		}
	}
}