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
        public string Owner { get; set; }
        public double MilitaryPower { get; set; }

    }
}
