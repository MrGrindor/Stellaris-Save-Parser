using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using StellarisSaveParser.Context.Models;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;

namespace StellarisSaveParser
{
    public class Parser
    {
        public SaveParserContext _saveParserContext;

        public Parser(SaveParserContext s)
        {
            _saveParserContext = s;
        }
        public void parseSave()
        {
            _saveParserContext.Database.EnsureCreated();
            string[] saveLines = File.ReadAllLines("C:/Users/thoma/source/repos/StellarisSaveParser/StellarisSaveParser/gamestate");
            int planetPos = 0;
            int galacticObjectPos = 0;
            for (int x = 0; x < saveLines.Length; x++)
            {
                if (saveLines[x].StartsWith("pop={"))
                {
                    x++;
                    while (!saveLines[x].StartsWith("}"))
                    {
                        var popstart = x;
                        while (!saveLines[x].StartsWith("\t}"))
                        {
                            x++;
                          
                        }
                        x++;
                        var pop = readPop(saveLines[popstart..x]);
                        
                        
                        _saveParserContext.Pops.Add(pop);
                        
                        
                        
                        Console.WriteLine($"Added Pop {pop.PopGameId}");

                    }
                    _saveParserContext.SaveChanges();
                }
                else if (saveLines[x].StartsWith("galactic_object={"))
                {
                    //save system position
                    galacticObjectPos = x;
                }
                else if (saveLines[x].StartsWith("planets={"))
                {
                    //save planet position
                    planetPos = x;
                }
                else if (saveLines[x].StartsWith("buildings={"))
                {
                    x++;
                    while (!saveLines[x].StartsWith("}"))
                    {
                        _saveParserContext.Buildings.Add(readBuildings(saveLines[x..(x + 2)]));
                        x+=3;
                    }
                    _saveParserContext.SaveChanges();
                }
            }
            //jump to planet position
            int y = planetPos;
            if (y == 0)
            {
                throw new Exception("Something went wrong");
            }
            else
            {
                y += 2;
                while (!saveLines[y].StartsWith("\t}"))
                {
                    var planetstart = y;
                    while (!saveLines[y].StartsWith("\t\t}"))
                    {
                        y++;

                    }
                    y++;
                    var planet = readPlanet(saveLines[planetstart..y]);
                    _saveParserContext.Planets.Add(planet);
                    Console.WriteLine($"Added Planet {planet.Name}");
                }
                _saveParserContext.SaveChanges();
            }
            //jump to system position
            y = galacticObjectPos;
            if (y == 0)
            {
                throw new Exception("Something went wrong");
            }
            else
            {
                y ++;
                while (!saveLines[y].StartsWith("}"))
                {
                    var galacticobjectstart = y;
                    while (!saveLines[y].StartsWith("\t}"))
                    {
                        y++;

                    }
                    y++;
                    var galacticobject = readGalacticObject(saveLines[galacticobjectstart..y]);
                    _saveParserContext.GalacticObjects.Add(galacticobject);
                    Console.WriteLine($"Added System {galacticobject.name}");
                }
                _saveParserContext.SaveChanges();
            }


            _saveParserContext.SaveChanges();
        }

        public Pop readPop(string[] lines)
        {

            var pop = new Pop() { 
                PopGameId = int.Parse(lines[0].Split('=')[0])
            };
             
            foreach(string line in lines)
            {
                if (line.StartsWith("\t\tspecies_index"))
                {
                    pop.SpeciesId = int.Parse(line.Split('=')[1]);
                    continue;
                }
                else if (line.StartsWith("\t\t\tethic"))
                {
                    var ethic = line.Split('=')[1];
                    ethic.Replace("\"", "");
                    pop.Ethos = ethic;
                    continue;
                }
                else if (line.StartsWith("\t\tjob"))
                {
                    var job = line.Split('=')[1];
                    job.Trim();
                    job.Trim('\"');
                    pop.Job = job;
                    continue;
                }
                else if (line.StartsWith("\t\tcategory")){
                    var strata = line.Split('=')[1];
                    strata.Trim();
                    strata.Trim('\"');
                    pop.Strata = strata;
                    continue;
                }
                else if (line.StartsWith("\t\tplanet"))
                {
                    pop.Planet = int.Parse(line.Split('=')[1]);
                    continue;
                }
                else if (line.StartsWith("\t\tpower"))
                {
                    pop.Power = float.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
                    continue;
                }
                else if (line.StartsWith("\t\thappiness"))
                {
                    pop.Hapiness = float.Parse(line.Split('=')[1], CultureInfo.InvariantCulture);
                }
            }

            return pop;
            
        }

        public Planet readPlanet(string[] lines)
        {
            var planet = new Planet()
            {
                PlanetGameId = int.Parse(lines[0].Split('=')[0])
            };

            for (var x = 0; x < lines.Length; x++)
            {
                if (lines[x].StartsWith("\t\t\tname"))
                {
                    planet.Name = lines[x].Split('=')[1];
                    continue;
                }
                else if (lines[x].StartsWith("\t\t\tplanet_class"))
                {
                    planet.Planet_class = lines[x].Split('=')[1];
                    continue;
                }
                else if (lines[x].StartsWith("\t\t\towner"))
                {
                    planet.Owner = int.Parse(lines[x].Split('=')[1]);
                    continue;
                }
                else if (lines[x].StartsWith("\t\t\tcontroller"))
                {
                    planet.Controller = int.Parse(lines[x].Split('=')[1]);
                    continue;
                }
                else if (lines[x].StartsWith("\t\t\tpop"))
                {
                    foreach(var pop in lines[x+1].Split(' '))
                    {
                        if(pop != "")
                        {
                            planet.Pops.Add(_saveParserContext.Pops.FirstOrDefault(p => p.PopGameId == int.Parse(pop)));
                        }
                        
                    }
                    x += 2;
                }
                else if (lines[x].StartsWith("\t\t\tbuildings"))
                {
                    foreach (var building in lines[x+1].Split(' '))
                    {
                        if(building != "")
                        {
                            planet.Buildings.Add(_saveParserContext.Buildings.FirstOrDefault(b => b.BuildingGameId == int.Parse(building)));
                        }
                        
                    }
                    x += 2;
                }
                else if (lines[x].StartsWith("\t\t\tdistrict"))
                {
                    planet.Districts.Add(createDistrict(lines[x]));
                    continue;
                }
                else if (lines[x].StartsWith("\t\t\tstability"))
                {
                    planet.Stability = float.Parse(lines[x].Split('=')[1]);
                    continue;
                }
                else if (lines[x].StartsWith("\t\t\tcrime"))
                {
                    planet.Crime = float.Parse(lines[x].Split('=')[1], CultureInfo.InvariantCulture);
                    continue;
                }
                else if (lines[x].StartsWith("\t\t\tmigration"))
                {
                    planet.Migration = float.Parse(lines[x].Split('=')[1], CultureInfo.InvariantCulture);
                    continue;
                }

            }
            return planet;
        }

        public Building readBuildings(string[] lines)
        {
            return new Building()
            {
                BuildingGameId = int.Parse(lines[0].Split('=')[0]),
                Type = lines[1].Split('=')[1]
            };
            
        }

        public District createDistrict(string line)
        {
            var district = new District()
            {
                Type = line.Split('=')[1]
            };
            if(_saveParserContext.Districts.Local.FirstOrDefault(d => d.Type == district.Type)== null)
            {
                _saveParserContext.Districts.Add(district);
                return district;
            }
            else
            {
                return _saveParserContext.Districts.Local.FirstOrDefault(d => d.Type == district.Type);
            }
            
        }

        public GalacticObject readGalacticObject(string[] lines)
        {
            var system = new GalacticObject()
            {
                GalacticObjectGameId = int.Parse(lines[0].Split('=')[0])
            };

            for(var x = 0; x<lines.Length; x++)
            {
                if (lines[x].StartsWith("\t\t\tx="))
                {
                    system.PosX = float.Parse(lines[x].Split('=')[1], CultureInfo.InvariantCulture);
                    continue;
                }
                else if (lines[x].StartsWith("\t\t\ty="))
                {
                    system.PosY = float.Parse(lines[x].Split('=')[1], CultureInfo.InvariantCulture);
                    continue;
                }
                else if (lines[x].StartsWith("\t\tstar_class"))
                {
                    var type = lines[x].Split('=')[1];
                    type.Trim();
                    type.Trim('\"');
                    system.type = type;
                    continue;
                }
                else if (lines[x].StartsWith("\t\tname"))
                {
                    var name = lines[x].Split('=')[1];
                    name.Trim();
                    name.Trim('\"');
                    system.name = name;
                    continue;
                }
                else if (lines[x].StartsWith("\t\thyperlane"))
                {
                    x++;
                    while (!lines[x].StartsWith("\t\t}"))
                    {
                        if (lines[x] == " ")
                        {
                            x++;
                            continue;
                        }

                        var hyperlanestart = x;
                        while (!lines[x].StartsWith("\t\t\t}"))
                        {
                            x++;
                        }
                        system.Hyperlanes.Add(readHyperlane(lines[hyperlanestart..x]));
                        x++;
                    }
                }
                //PLANETS
                else if (lines[x].StartsWith("\t\tplanet"))
                {
                    var planetId = int.Parse(lines[x].Split('=')[1]);
                    var planet = _saveParserContext.Planets.FirstOrDefault(p => p.PlanetGameId == planetId);
                    system.Planets.Add(planet);
                }
                
            }

            return system;
        }

        public Hyperlane readHyperlane(string[] lines)
        {


            return new Hyperlane()
            {
                targetId = int.Parse(lines[1].Split("=")[1]),
                distance = float.Parse(lines[2].Split("=")[1],CultureInfo.InvariantCulture)
            };
        }
    }
}
