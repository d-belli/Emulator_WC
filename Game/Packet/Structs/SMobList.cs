using System;
using System.Runtime.InteropServices;
using System.Xml.Serialization;

namespace Emulator
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SMobList
    {
        public string MobName;
        public int Number;
        public int Speed;
        public SPosition PositionInicial;
        public SPosition PositionFinal;
        public int FreqStep;
        public int IdGroup;
        public string Tab;
        [XmlIgnore]
        public SMob Mob;

        // Construtores
        public static SMobList New()
        {
            SMobList tmp = new SMobList()
            {
                Mob = SMob.New(),
                Number = 0,
                Speed = 1,
                PositionInicial = SPosition.New(),
                PositionFinal = SPosition.New(),
                FreqStep = 1,
                IdGroup = 0,
                Tab = ""
            };
            return tmp;
        }
    }
}
