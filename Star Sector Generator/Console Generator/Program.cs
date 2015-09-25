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


            bool generate = true;
            StarSectorGenerator generator = new StarSectorGenerator(new HexCoordinate(width, height));
            StarSector sector = new StarSector();

            bool loop = true;
            while (loop)
            {
                if (generate)
                {
                    Console.Clear();

                    // Generate Sector
                    generator = new StarSectorGenerator(new HexCoordinate(width, height));
                    sector = generator.Generate();
                    Writer.WriteLine(sector.ToString());

                    Console.WriteLine("Type 'save xml' to save output to an XML file");
                    Console.WriteLine("Type 'save txt' to save output to a text file");
                    Console.WriteLine("Type 'save' to save output to both text and XML files");
                    Console.WriteLine("Type 'exit' to exit generation");
                    Console.WriteLine("Otherwise Press enter to regenerate");
                }

                string read = Console.ReadLine();
                switch (read.ToLower())
                {
                    case "save":
                        StarSector.Save(sector);
                        StarSector.SaveToString(sector);
                        generate = false;
                        break;
                    case "save xml":
                        StarSector.Save(sector);
                        generate = false;
                        break;
                    case "save txt":
                        StarSector.SaveToString(sector);
                        generate = false;
                        break;
                    case "exit":
                        loop = false;
                        generate = false;
                        break;
                    default:
                        generate = true;
                        break;
                }
            }
        }
    }
}
