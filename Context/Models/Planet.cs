using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using StellarisSaveParser.Context.Models;

namespace StellarisSaveParser
{
    public class Planet
    {
        [Key]
        public int PlanetId { get; set; }
        public int PlanetGameId { get; set; }
        public string Name { get; set; }
        public string Planet_class { get; set; }
        public int Owner { get; set; }
        public int Controller { get; set; }
        public List<Pop> Pops { get; set; } = new List<Pop>();
        public List<Building> Buildings { get; set; } = new List<Building>();
        public List<District> Districts { get; set; } = new List<District>();
        public float Stability { get; set; }
        public float Crime { get; set; }
        public float Migration { get; set; }

    }


}
