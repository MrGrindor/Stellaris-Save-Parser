using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace StellarisSaveParser.Context.Models
{
    public class Hyperlane
    {
        [Key]
        public int hyperlaneId { get; set; }
        public int targetId { get; set; }
        public float distance { get; set; }
    }
}
