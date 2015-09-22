using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Models
{
    public class StarSector
    {
        public List<List<StarSystem>> Sector { get; set; }
        public StarSector()
        {
            Sector = new List<List<StarSystem>>();
        }
    }
}
