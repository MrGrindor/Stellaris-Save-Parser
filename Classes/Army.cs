using System;
using System.Collections.Generic;
using System.Text;

namespace StellarisSaveParser.Classes
{
    public class Army
    {
        public int ArmyId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Owner { get; set; }
        public int PlanetId { get; set; }
    }
}
