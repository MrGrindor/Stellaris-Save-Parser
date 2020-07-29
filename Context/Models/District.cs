using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace StellarisSaveParser.Context.Models
{
    public class District
    {
        [Key]
        public int DistrictId { get; set; }
        public string Type { get; set; }
    }
}
