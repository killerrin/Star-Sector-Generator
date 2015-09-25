using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Shared.Models
{
    public class StarSector : BindableBase
    {
        public List<List<StarSystem>> Sector { get; set; }
        public StarSector()
        {
            Sector = new List<List<StarSystem>>();
        }

        public override string ToString()
        {
            string str = "";
            foreach (var sectorRow in Sector)
            {
                foreach (var system in sectorRow)
                {
                    str += string.Format("\n{0}\n", system);
                }
            }

            return str;
        }

        public static bool Save(StarSector sector)
        {
            Debug.WriteLine("Saving Star Sector");
            try
            {
                var writer = new System.Xml.Serialization.XmlSerializer(typeof(StarSector));
                var wfile = new StreamWriter("Star Sector.xml");
                writer.Serialize(wfile, sector);
                wfile.Close();
            }
            catch (Exception) { return false; }
            return true;
        }

        public static bool SaveToString(StarSector sector)
        {
            Debug.WriteLine("Saving Star Sector");
            try
            {
                var wfile = new StreamWriter("Star Sector.txt");
                wfile.WriteLine(sector.ToString());
                wfile.Close();
            }
            catch (Exception) { return false; }
            return true;
        }
    }
}