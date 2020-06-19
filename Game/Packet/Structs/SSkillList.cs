using System;
using System.Runtime.InteropServices;

namespace Emulator
{
    /// <summary>
	/// Estrutura da Skill - size 142
	/// </summary>
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct SSkillList
    {
        public short IdSkill;
        public int PrecoDePontos;
        public int TipoDaSkill;//---- 0 = buff,1 = Atack1pessoa,2 = Curas,3 = skillnoinimigo,4 = atacke area
        public int ManaInicial;
        public int Delay;
        public int DistanciaDaSkill;
        public int InstanceType;
        public int InstanceValue;
        public int TickType;
        public int TickValue;
        public int TipoDeEfeito;
        public int ValorDoEfeito;
        public int TempoDoEfeito;
        public string Act123;
        public string Act1233;
        public int InstanceAttribute;
        public int TickAttribute;
        public int Aggressive;
        public int Maxtarget;
        public int PartyCheck;
        public int AffectResist;
        public int Passive;
        public String NomeDaSkill;

    }
}
