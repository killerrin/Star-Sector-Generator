using Shared.Models.Enums;
using System.Collections.ObjectModel;
using Shared.DnDTools;
using System;

namespace Shared.Models
{
    public class Star : BindableBase
    {
        public ObservableCollection<CelestialBody> CelestialBodies { get; set; }
        public StarClassification Classification { get; set; }
        public StarAge Age { get; set; }
        public RadiationLevel Radiation { get; set; }

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
            CelestialBodies = new ObservableCollection<CelestialBody>();
        }

        public override string ToString()
        {
            string str = "\n";
            str += string.Format("{0} \t{1} \t{2} \n", 
                        StringHelpers.AddSpacesToSentence(Classification.ToString()),
                        StringHelpers.AddSpacesToSentence(Age.ToString()),
                        StringHelpers.AddSpacesToSentence(Radiation.ToString()));

            foreach (var body in CelestialBodies)
            {
                str += string.Format("{0}\n", body);
            }

            return str;
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
                star.CelestialBodies.Add(CelestialBody.Generate(die));
            }
            return star;
        }

        #region Individual Generators
        public static int GenerateNumberOfCelestialBodies(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10) return 0;
            else if (percentage <= 40) return die.Roll(1, 4);
            else if (percentage <= 70) return die.Roll(1, 5);
            else if (percentage <= 95) return die.Roll(1, 6);
            return die.Roll(2, 6);
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

            if (percentage <= 25)       return RadiationLevel.LowRadiation;
            else if (percentage <= 50)  return RadiationLevel.NormalRadiation;
            else if (percentage <= 75)  return RadiationLevel.HighRadiation;
            
            //else if (percentage <= 100)  
            return RadiationLevel.ExtremeRadiation;
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