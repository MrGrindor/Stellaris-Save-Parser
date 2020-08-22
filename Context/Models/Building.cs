using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StellarisSaveParser
{
    public class Building
    {
        [Key]
        public int BuildingId { get; set; }
        public int BuildingGameId { get; set; }
        public string Type { get; set; }
    }
}
