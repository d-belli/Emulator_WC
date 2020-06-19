using System.Collections.Generic;
using System.Xml.Serialization;

namespace Emulator
{
    public class Character
    {
        // Atributos
        public SMob Mob;
        public List<int> Skill; // lista Ids de skill aprendidas
        [XmlIgnore]
        public List<int> PartyID;
        public List<SPosition> Positions { get; private set; }

        // Construtor
        public Character()
        {
            this.Mob = default;
            this.Skill = new List<int>();
            this.PartyID = new List<int>();
            this.Positions = new List<SPosition>();
        }
        public Character(SMob mob)
        {
            this.Mob = mob;
            this.Skill = new List<int>();
            this.PartyID = new List<int>();
            this.Positions = new List<SPosition>();
        }
    }
}