using StellarisSaveParser;
using StellarisSaveParser.Context.Models;
using System;
using System.Collections.Generic;

namespace SaveParserLibrary
{
    public class Map
    {
        public List<GalacticObject> GalacticObjects { get; set; } = new List<GalacticObject>();
        public List<Pop> Pops { get; set; } = new List<Pop>();
        public List<Planet> Planets { get; set; } = new List<Planet>();
        public List<Building> Buildings { get; set; } = new List<Building>();
        public List<District> Districts { get; set; } = new List<District>();
        public Parser Parser { get; set; }

        public Map(string path)
        {
            Parser = new Parser(this);
            Parser.parseSave(path);

        }

        

    }
}
