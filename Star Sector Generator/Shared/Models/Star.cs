using Shared.Models.Enums;
using System.Collections.ObjectModel;
using Shared.DnDTools;
using System;

namespace Shared.Models
{
    public class Star : BindableBase
    {
        public ObservableCollection<CelestialObject> CelestialBodies { get; set; }
        public StarClassification Classification { get; set; }
        public RadiationLevel Radiation { get; set; }
        public StarAge Age { get; set; }

        public int TotalResources
        {
            get
            {
                int resources = 0;
                foreach (var body in CelestialBodies)
                    resources += body.TotalResources;
                return resources;
            }
        }

        public Star()
        {
            CelestialBodies = new ObservableCollection<CelestialObject>();
        }

        public static Star Generate(Dice die)
        {
            Star star = new Star();
            star.Classification = GenerateStarClassification(die);
            star.Radiation      = GenerateRadiationLevel(die);
            star.Age            = GenerateStarAge(die);

            int numberOfBodies = GenerateNumberOfCelestialBodies(die);
            for (int i = 0; i < numberOfBodies; i++)
            {
                star.CelestialBodies.Add(CelestialObject.Generate(die));
            }
            return star;
        }

        #region Individual Generators
        public static int GenerateNumberOfCelestialBodies(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10) return 0;
            else if (percentage <= 40) return die.Roll(1, 4);
            else if (percentage <= 50) return die.Roll(1, 5);
            else if (percentage <= 60) return die.Roll(1, 6);
            else if (percentage <= 70) return die.Roll(1, 7);
            else if (percentage <= 80) return die.Roll(1, 8);
            else if (percentage <= 90) return die.Roll(2, 8);
            return die.Roll(2, 10);
        }

        public static StarClassification GenerateStarClassification(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10)       return StarClassification.RedGiant;
            else if (percentage <= 20)  return StarClassification.RedNormal;
            else if (percentage <= 30)  return StarClassification.RedDwarf;
            else if (percentage <= 40)  return StarClassification.BrownNormal;
            else if (percentage <= 50)  return StarClassification.BrownDrawf;
            else if (percentage <= 60)  return StarClassification.WhiteGiant;
            else if (percentage <= 70)  return StarClassification.WhiteNormal;
            else if (percentage <= 80)  return StarClassification.WhiteDwarf;
            else if (percentage <= 90)  return StarClassification.NeutronStar;
            return StarClassification.Blackhole;
        }

        public static RadiationLevel GenerateRadiationLevel(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 25)       return RadiationLevel.Low;
            else if (percentage <= 50)  return RadiationLevel.Normal;
            else if (percentage <= 75)  return RadiationLevel.High;
            
            //else if (percentage <= 100)  
            return RadiationLevel.Extreme;
        }
        public static StarAge GenerateStarAge(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 25)       return StarAge.Newborn;
            else if (percentage <= 75)  return StarAge.Midlife;
            return StarAge.EndOfLife;
        }
        #endregion
    }
}