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

        public StarSector Generate()
        {
            StarSector sector = new StarSector();
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
                sector.Sector.Add(row);
            }
            return sector;
        }
    }
}
