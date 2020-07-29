using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace StellarisSaveParser.Classes
{
    public class Empire
    {
        public int EmpireID { get; set; }
        public List<Color> Colors { get; set; }
        public string Name { get; set; }
        public List<int> OwnedFleetIds { get; set; } = new List<int>();
        public List<int> OwnedArmyIds { get; set; } = new List<int>();
    }
}
