using Shared;
using Shared.DnDTools;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Generator
{
    public class Program
    {
        static Program program;
        static void Main(string[] args)
        {
            program = new Program();
            program.Run();
        }

        public Program() { }

        public void Run()
        {
            int width = 1;
            int height = 1;

            while (true)
            {
                Console.WriteLine("Sector Width");
                string wstring = Console.ReadLine();

                Console.WriteLine("Sector Height");
                string hstring = Console.ReadLine();

                if (int.TryParse(wstring, out width) &&
                    int.TryParse(hstring, out height) &&
                    width > 0 &&
                    height > 0)
                    break;
            }

            Console.Clear();

            // Generate Sector
            StarSectorGenerator generator = new StarSectorGenerator(new HexCoordinate(width, height));
            List<List<StarSystem>> sector = generator.Generate();

            foreach (var sectorRow in sector)
            {
                foreach (var system in sectorRow)
                {
                    Console.WriteLine(system);

                }
            }
            Console.ReadLine();
        }
    }
}
