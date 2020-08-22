using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using StellarisSaveParser.Context.Models;

namespace StellarisSaveParser
{
    public class GalacticObject
    {
        [Key]
        public int GalacticObjectID { get; set; }
        public int GalacticObjectGameId { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public List<Planet> Planets { get; set; } = new List<Planet>();
        public List<Hyperlane> Hyperlanes { get; set; } = new List<Hyperlane>();
    }
}
