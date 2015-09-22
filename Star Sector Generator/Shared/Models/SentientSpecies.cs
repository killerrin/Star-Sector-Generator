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
    public class SentientSpecies : BindableBase
    {
        public ObservableCollection<AnimalClassification> Classifications { get; set; }
        public ObservableCollection<CivilizationTraits> Traits { get; set; }
        public CivilizationTechLevel TechLevel { get; set; }

        public SentientSpecies()
        {
            Classifications = new ObservableCollection<AnimalClassification>();
            Traits = new ObservableCollection<CivilizationTraits>();
        }
        public override string ToString()
        {
            string str = string.Format("TL{0} {1}\t",
                (int)TechLevel,
                StringHelpers.AddSpacesToSentence(TechLevel.ToString()));

            foreach (var trait in Traits)
            {
                str += string.Format("{0} ",
                    StringHelpers.AddSpacesToSentence(trait.ToString()));
            }
            str += "\t";//\n\t\t";
            foreach (var classification in Classifications)
            {
                str += string.Format("{0} ",
                    StringHelpers.AddSpacesToSentence(classification.ToString()));
            }

            return str;
        }

        public static SentientSpecies Generate(Dice die)
        {
            SentientSpecies sentient = new SentientSpecies();
            sentient.TechLevel = GenerateCivilizationTechLevel(die);

            #region Generate Species Physiology
            int maxNumberOfAnimalClassifications = GenerateMaxNumberOfAnimalClassifications(die);
            while (true)
            {
                if (sentient.Classifications.Count >= maxNumberOfAnimalClassifications) break;
                AnimalClassification classification = GenerateAnimalClassification(die);

                bool acceptableClassification = true;
                foreach (var c in sentient.Classifications)
                {
                    if (classification == c) { acceptableClassification = false; break; }
                }

                if (acceptableClassification)
                {
                    sentient.Classifications.Add(classification);
                    if (die.Roll(1, 20) < 10) break;
                }
            }
            #endregion

            #region Generate Civilization Traits
            while (true)
            {
                if (sentient.Traits.Count >= 3) break;
                CivilizationTraits trait = GenerateCivilizationTrait(die);

                bool acceptableTrait = true;
                if (sentient.Traits.Count == 0) { }
                else
                {
                    foreach (var t in sentient.Traits)
                    {
                        if (trait == t) { acceptableTrait = false; break; }
                        else if ((trait == CivilizationTraits.Imperialist && t == CivilizationTraits.PeaceKeepers) ||
                                 (trait == CivilizationTraits.PeaceKeepers && t == CivilizationTraits.Imperialist))
                        { acceptableTrait = false; break; }

                        else if ((trait == CivilizationTraits.Communist && t == CivilizationTraits.Capitalist) ||
                                 (trait == CivilizationTraits.Capitalist && t == CivilizationTraits.Communist))
                        { acceptableTrait = false; break; }
                    }
                }

                if (acceptableTrait) sentient.Traits.Add(trait);
            }
            #endregion

            return sentient;
        }

        #region Individual Generators
        public static int GenerateMaxNumberOfAnimalClassifications(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 75) return 1;
            else if (percentage <= 80) return die.Roll(1, 3);
            return die.Roll(2, 4);
        }
        public static AnimalClassification GenerateAnimalClassification(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10)       return AnimalClassification.Amphibian;
            else if (percentage <= 20)  return AnimalClassification.Avian;
            else if (percentage <= 30)  return AnimalClassification.Aquatic;
            else if (percentage <= 50)  return AnimalClassification.Reptillian;
            else if (percentage <= 75)  return AnimalClassification.Mammal;
            else if (percentage <= 85)  return AnimalClassification.Rock;
            else if (percentage <= 95)  return AnimalClassification.Energy;
            else if (percentage <= 98)  return AnimalClassification.Exotic;
            return AnimalClassification.SpaceBased;
        }

        public static CivilizationTraits GenerateCivilizationTrait(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10)       return CivilizationTraits.Capitalist;
            else if (percentage <= 20)  return CivilizationTraits.Communist;
            else if (percentage <= 30)  return CivilizationTraits.Corporate;
            else if (percentage <= 40)  return CivilizationTraits.Explorer;
            else if (percentage <= 50)  return CivilizationTraits.Imperialist;
            else if (percentage <= 60)  return CivilizationTraits.PeaceKeepers;
            else if (percentage <= 70)  return CivilizationTraits.Philosophical;
            else if (percentage <= 80)  return CivilizationTraits.Scientist;
            else if (percentage <= 90)  return CivilizationTraits.Theocratic;
            return CivilizationTraits.Warmonger;
        }

        public static CivilizationTechLevel GenerateCivilizationTechLevel(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10)       return CivilizationTechLevel.Cavemen;
            else if (percentage <= 20)  return CivilizationTechLevel.SpaceAge;
            else if (percentage <= 30)  return CivilizationTechLevel.BronzeAge;
            else if (percentage <= 40)  return CivilizationTechLevel.IronAge;

            else if (percentage <= 50)  return CivilizationTechLevel.IndustrialRevolution;
            else if (percentage <= 60)  return CivilizationTechLevel.AtomicAge;
            else if (percentage <= 70)  return CivilizationTechLevel.SpaceAge;
            else if (percentage <= 90)  return CivilizationTechLevel.DigitalAge;
            return CivilizationTechLevel.InterstellarAge;
        }
        #endregion
    }
}
