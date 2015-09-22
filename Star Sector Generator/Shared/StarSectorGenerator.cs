using System;
using System.Collections.Generic;
using System.Text;
using Shared.DnDTools;
using Shared.Models;

namespace Shared
{
    public class StarSectorGenerator
    {
        public HexCoordinate WidthHeight = new HexCoordinate(1, 1);
        public Dice Die = new Dice();

        public StarSectorGenerator(HexCoordinate widthHeight)
        {
            WidthHeight = widthHeight;
        }

        public List<List<StarSystem>> Generate()
        {
            List<List<StarSystem>> sector = new List<List<StarSystem>>(WidthHeight.X);
            Die = new Dice();

            for (int x = 0; x < WidthHeight.X; x++)
            {
                var row = new List<StarSystem>(WidthHeight.Y);
                for (int y = 0; y < WidthHeight.Y; y++)
                {
                    var starSystem = StarSystem.Generate(Die);
                    starSystem.Coordinate = new HexCoordinate(x, y);
                    row.Add(starSystem);
                }
                sector.Add(row);
            }
            return sector;
        }

        public static bool Save(UnitSettings settings)
        {
            Debug.WriteLine("Saving UnitSettings");
            try
            {
                var writer = new System.Xml.Serialization.XmlSerializer(typeof(UnitSettings));
                var wfile = new StreamWriter("UnitStatistics.xml");
                writer.Serialize(wfile, settings);
                wfile.Close();
            }
            catch (Exception) { return false; }
            return true;
        }
    }
}
