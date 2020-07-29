using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
using StellarisSaveParser.Context.Models;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using SaveParserLibrary;
using StellarisSaveParser.Classes;
using System.Drawing;
using System.Xml.Schema;

namespace StellarisSaveParser
{
    public class Parser
    {
        public Map _Map;

        public Parser(Map m)
        {
            _Map = m;
        }
        
        public void parseSave(string path)
        {
            
            
            string[] saveLines = File.ReadAllLines(path);
            int planetPos = 0;
            int galacticObjectPos = 0;
            int shippos = 0;
            int fleetpos = 0;
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
                        
                        
                        _Map.Pops.Add(pop);
                        
                        
                        
                        Console.WriteLine($"Added Pop {pop.PopGameId}");

                    }
                    
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
                        _Map.Buildings.Add(readBuildings(saveLines[x..(x + 2)]));
                        x+=3;
                    }
                    
                }
                else if (saveLines[x].StartsWith("country={"))
                {
                    
                    x++;
                    while (!saveLines[x].StartsWith("}"))
                    {
                        var countrystart = x;
                        while (!saveLines[x].StartsWith("\t}"))
                        {
                            x++;
                        }
                        var country = readEmpire(saveLines[countrystart..x]);
                        _Map.Empires.Add(country);

                        Console.WriteLine($"Added Empire {country.Name}");

                        x++;
                    }
                }
                else if (saveLines[x].StartsWith("ships={"))
                {
                    shippos = x;
                }
                else if (saveLines[x].StartsWith("fleet={"))
                {
                    fleetpos = x;
                }
                else if (saveLines[x].StartsWith("ship_design={"))
                {
                    x++;
                    while (!saveLines[x].StartsWith("}"))
                    {
                        var designstart = x;
                        while (!saveLines[x].StartsWith("\t}"))
                        {
                            x++;
                        }
                        var design = readDesign(saveLines[designstart..x]);
                        _Map.Designs.Add(design);

                        Console.WriteLine($"Added Design {design.Id} of size {design.ShipSize}");

                        x++;
                    }
                }
                else if (saveLines[x].StartsWith("army={"))
                {
                    x++;
                    while (!saveLines[x].StartsWith("}"))
                    {
                        var armystart = x;
                        while (!saveLines[x].StartsWith("\t}"))
                        {
                            x++;
                        }
                        var army = readArmy(saveLines[armystart..x]);
                        _Map.Armies.Add(army);

                        Console.WriteLine($"Added army {army.Name}");

                        x++;
                    }
                }
                

            }
            Thread.Sleep(100000);
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
                    _Map.Planets.Add(planet);
                    Console.WriteLine($"Added Planet {planet.Name}");
                }
               
            }
            //jump to system position
            y = galacticObjectPos;
            if (y == 0)
            {
                throw new Exception("Something went wrong");
            }
            else
            {
                y++;
                while (!saveLines[y].StartsWith("}"))
                {
                    var galacticobjectstart = y;
                    while (!saveLines[y].StartsWith("\t}"))
                    {
                        y++;

                    }
                    y++;
                    var galacticobject = readGalacticObject(saveLines[galacticobjectstart..y]);
                    _Map.GalacticObjects.Add(galacticobject);
                    Console.WriteLine($"Added System {galacticobject.Name}");
                }

            }

            y = shippos;
            if (y == 0)
            {
                throw new Exception("Something went wrong");
            }
            else
            {
                y++;
                while (!saveLines[y].StartsWith("}"))
                {
                    var shipstart = y;
                    while (!saveLines[y].StartsWith("\t}"))
                    {
                        y++;

                    }
                    y++;
                    var ship = readShip(saveLines[shipstart..y]);
                    _Map.Ships.Add(ship);
                    Console.WriteLine($"Added ship {ship.ShipName} of Size {ship.Type}");
                }

            }
            y = fleetpos;
            if (y == 0)
            {
                throw new Exception("Something went wrong");
            }
            else
            {
                y++;
                while (!saveLines[y].StartsWith("}"))
                {
                    var fleetstart = y;
                    while (!saveLines[y].StartsWith("\t}"))
                    {
                        y++;

                    }
                    y++;
                    var fleet = readFleet(saveLines[fleetstart..y]);
                    _Map.Fleets.Add(fleet);
                    Console.WriteLine($"Added fleet {fleet.Name} of Power {fleet.MilitaryPower}");
                }

            }
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
                            planet.Pops.Add(_Map.Pops.FirstOrDefault(p => p.PopGameId == int.Parse(pop)));
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
                            planet.Buildings.Add(_Map.Buildings.FirstOrDefault(b => b.BuildingGameId == int.Parse(building)));
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
            if(_Map.Districts.FirstOrDefault(d => d.Type == district.Type)== null)
            {
                _Map.Districts.Add(district);
                return district;
            }
            else
            {
                return _Map.Districts.FirstOrDefault(d => d.Type == district.Type);
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
                    system.Type = type;
                    continue;
                }
                else if (lines[x].StartsWith("\t\tname"))
                {
                    var name = lines[x].Split('=')[1];
                    name.Trim();
                    name.Trim('\"');
                    system.Name = name;
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
                    var planet = _Map.Planets.FirstOrDefault(p => p.PlanetGameId == planetId);
                    system.Planets.Add(planet);
                }
                
            }

            return system;
        }

        public Hyperlane readHyperlane(string[] lines)
        {


            return new Hyperlane()
            {
                TargetId = int.Parse(lines[1].Split("=")[1]),
                Distance = float.Parse(lines[2].Split("=")[1],CultureInfo.InvariantCulture)
            };
        }

        public Empire readEmpire(string[] lines)
        {
            var empire = new Empire()
            {
                EmpireID = int.Parse(lines[0].Split('=')[0])
            };

            for(var x = 0; x < lines.Length; x++)
            {
                if (lines[x].StartsWith("\t\t\tcolors"))
                {
                    x++;
                    while (lines[x].StartsWith("\t\t\t}"))
                    {
                        empire.Colors.Add(Color.FromName(lines[x]));
                        x++;
                    }
                    continue;
                }
                else if (lines[x].StartsWith("\t\tName"))
                {
                    empire.Name = lines[x].Split('=')[1];
                    continue;
                }
                else if (lines[x].StartsWith("\t\towned_fleets"))
                {
                    var fleets = lines[x + 1].Split(" ");
                    foreach(var fleet in fleets)
                    {
                        if (fleet == "")
                        {
                            continue;
                        }
                        empire.OwnedFleetIds.Add(int.Parse(fleet));
                    }
                }
                else if (lines[x].StartsWith("\t\towned_armies"))
                {
                    var armies = lines[x + 1].Split(" ");
                    foreach (var army in armies)
                    {
                        if (army == "")
                        {
                            continue;
                        }
                        empire.OwnedArmyIds.Add(int.Parse(army));
                    }
                }
            }
            return empire;
        }

        public Ship readShip(string[] lines)
        {
            var ship = new Ship()
            {
                ShipId = int.Parse(lines[0].Split('=')[0])
            };

            for (var x = 0; x < lines.Length; x++)
            {
                if (lines[x].StartsWith("\t\tfleet"))
                {
                    ship.FleetId = int.Parse(lines[x].Split('=')[1]);
                }
                if (lines[x].StartsWith("\t\tname"))
                {
                    ship.ShipName = lines[x].Split('=')[1];
                }
                if (lines[x].StartsWith("\t\tship_design"))
                {
                    var designId = int.Parse(lines[x].Split('=')[1]);
                    var design = _Map.Designs.FirstOrDefault(d => d.Id == designId);
                    ship.Type = design.ShipSize;
                }
            }

            return ship;
        }

        public Design readDesign(string[] lines)
        {
            var design = new Design()
            {
                Id = int.Parse(lines[0].Split('=')[0])
            };

            for(var x = 0; x<lines.Length; x++)
            {
                if (lines[x].StartsWith("\t\tship_size"))
                {
                    design.ShipSize = lines[x].Split('=')[1];
                    break;
                }
            }

            return design;
        }

        public Fleet readFleet(string[] lines)
        {
            var fleet = new Fleet()
            {
                FleetId = int.Parse(lines[0].Split('=')[0])
            };

            for(var x = 0; x < lines.Length; x++)
            {
                if (lines[x].StartsWith("\t\tname"))
                {
                    fleet.Name = lines[x].Split('=')[1];
                }
                else if (lines[x].StartsWith("\t\tships"))
                {
                    var ships = lines[x + 1].Split(" ");
                    foreach (var shipId in ships)
                    {
                        if(shipId == "")
                        {
                            continue;
                        }
                        fleet.Ships.Add(_Map.Ships.FirstOrDefault(s => s.ShipId == int.Parse(shipId)));
                    }
                }
                else if (lines[x].StartsWith("\t\towner"))
                {
                    fleet.Owner = int.Parse(lines[x].Split('=')[1]);
                }
                else if (lines[x].StartsWith("\t\tmovement_manager"))
                {
                    while (!lines[x].StartsWith("\t\t\tcoordinate"))
                    {
                        x++;
                    }
                    while (!lines[x].StartsWith("\t\t\t\torigin"))
                    {
                        x++;
                    }
                    fleet.System = int.Parse(lines[x].Split('=')[1]);
                }
                else if (lines[x].StartsWith("\t\tmilitary_power"))
                {
                    fleet.MilitaryPower = double.Parse(lines[x].Split('=')[1],CultureInfo.InvariantCulture);
                }
            }


            return fleet;
        }
        public Army readArmy(string[] lines)
        {
            var army = new Army()
            {
                ArmyId = int.Parse(lines[0].Split('=')[0])
            };

            for (var x = 0; x < lines.Length; x++)
            {
                if (lines[x].StartsWith("\t\tname"))
                {
                    army.Name = lines[x].Split('=')[1];
                }
                else if (lines[x].StartsWith("\t\ttype"))
                {
                    army.Type = lines[x].Split('=')[1];
                }
                else if (lines[x].StartsWith("\t\towner"))
                {
                    army.Owner = int.Parse(lines[x].Split('=')[1]);
                }
                else if (lines[x].StartsWith("\t\thome_planet"))
                {
                    army.PlanetId = int.Parse(lines[x].Split('=')[1]);
                }
            }

            return army;
        }
    }
}
