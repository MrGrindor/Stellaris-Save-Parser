using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace StellarisSaveParser.Classes
{
    public class Fleet
    {
        public int FleetId { get; set; }
        public string Name { get; set; }
        public int Owner { get; set; }
        public double MilitaryPower { get; set; }
        public int System { get; set; }
        public List<Ship> Ships { get; set; } = new List<Ship>();

    }
}
