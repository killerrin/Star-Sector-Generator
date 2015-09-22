using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.DnDTools;

namespace Shared.Models
{
    public class StarSystem : BindableBase
    {
        public HexCoordinate Coordinate { get; set; }

        public ObservableCollection<Star> Stars { get; set; }
        public int TotalResources
        {
            get
            {
                int resources = 0;
                foreach (var star in Stars)
                    resources += star.TotalResources;
                return resources;
            }
        }

        public StarSystem()
        {
            Stars = new ObservableCollection<Star>();
            Coordinate = new HexCoordinate();
        }

        public override string ToString()
        {
            return string.Format("Hex: {0} \nTotal Resources: {1}", Coordinate, TotalResources);
        }

        public static StarSystem Generate(Dice die)
        {
            StarSystem system = new StarSystem();
            int numberOfStars = GenerateNumberOfStars(die);

            for (int i = 0; i < numberOfStars; i++)
            {
                system.Stars.Add(Star.Generate(die));
            }

            return system;
        }

        #region Individual Generators
        public static int GenerateNumberOfStars(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 15) return 0;
            else if (percentage <= 80) return 1;

            return die.Roll(1, 5);
        }
        #endregion
    }
}
