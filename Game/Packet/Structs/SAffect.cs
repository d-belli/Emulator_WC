using System.Runtime.InteropServices;

namespace Emulator {
	/// <summary>
	/// Estrutura do affect - size 8
	/// </summary>
	[StructLayout ( LayoutKind.Sequential , CharSet = CharSet.Ansi , Pack = 1 )]
	public struct SAffect {
		// Atributos
		public byte Index;    // 0			= 1
		public byte Master;   // 1			= 1
		public short Value;  // 2 a 3	= 2
		public int Time;     // 4 a 7	= 4

		// Construtores
		public static SAffect New ( ) {
			SAffect tmp = new SAffect {
				Index = 0 ,
				Master = 0 ,
				Value = 0 ,
				Time = 0
			};

			return tmp;
		}

		public static SAffect New ( SAffect other ) {
			SAffect tmp = new SAffect {
				Index = other.Index ,
				Master = other.Master ,
				Value = other.Value ,
				Time = other.Time
			};

			return tmp;
		}
		public static SAffect New ( byte index , byte master , short value , int time ) {
			SAffect tmp = new SAffect {
				Index = index ,
				Master = master ,
				Value = value ,
				Time = time
			};

			return tmp;
		}
	}
}