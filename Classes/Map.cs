using StellarisSaveParser;
using StellarisSaveParser.Classes;
using StellarisSaveParser.Context.Models;
using System;
using System.Collections.Generic;

namespace StellarisSaveParser
{
    public class Map
    {
        public List<GalacticObject> GalacticObjects { get; set; } = new List<GalacticObject>();
        public List<Pop> Pops { get; set; } = new List<Pop>();
        public List<Planet> Planets { get; set; } = new List<Planet>();
        public List<Building> Buildings { get; set; } = new List<Building>();
        public List<District> Districts { get; set; } = new List<District>();
        public List<Design> Designs { get; set; } = new List<Design>();
        public List<Ship> Ships { get; set; } = new List<Ship>();
        public List<Fleet> Fleets { get; set; } = new List<Fleet>();
        public List<Empire> Empires { get; set; } = new List<Empire>();
        public List<Army> Armies { get; set; } = new List<Army>();
        
        public Parser Parser { get; set; }

        public Map(string path)
        {
            Parser = new Parser(this);
            Parser.parseSave(path);

        }

        

    }
}
