using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace StellarisSaveParser
{
    public class Pop
    {
        [Key]
        public int PopId { get; set; }
        public int PopGameId { get; set; }
        public int SpeciesId { get; set; }
        public string Ethos { get; set; }
        public string Job { get; set; }
        public string Strata { get; set; }
        public int Planet { get; set; }
        public float Power { get; set; }
        public float Hapiness { get; set; }
     
    }
}
