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
    public class CelestialSatellite : CelestialBase
    {
        public CelestialSatelliteType CelestialType { get; set; }
        public CelestialSatellite()
            :base()
        {
        }

        public override string ToString()
        {
            string str = string.Format("{0}RP\t{1}\t{2}\t{3}",
                ResourceValue,
                StringHelpers.AddSpacesToSentence(CelestialType.ToString()),
                StringHelpers.AddSpacesToSentence(TerraformingTier.ToString()),
                StringHelpers.AddSpacesToSentence(StageOfLife.ToString()));

            if (Sentients.Count > 0)
            {
                //str += "\n\t\tSentients";
                str += "\n";
                foreach (var sentient in Sentients)
                {
                    str += string.Format("\t\t{0}\n", sentient);
                }
            }

            return str;
        }

        public static CelestialSatellite Generate(Dice die)
        {
            CelestialSatellite celestialSatellite = new CelestialSatellite();
            celestialSatellite.CelestialType = GenerateCelestialSatelliteType(die);
            celestialSatellite.TerraformingTier = GenerateTerraformationTier(die);

            if (celestialSatellite.TerraformingTier == TerraformationTier.Uninhabitable ||
                celestialSatellite.CelestialType == CelestialSatelliteType.GasCloud)
            {
                celestialSatellite.StageOfLife = LifeStage.NoLife;
            }
            else if (celestialSatellite.TerraformingTier == TerraformationTier.T1 ||
                     celestialSatellite.TerraformingTier == TerraformationTier.T2)
            {
                celestialSatellite.StageOfLife = GenerateStageOfLifeT1T2(die);
            }
            else celestialSatellite.StageOfLife = GenerateStageOfLife(die);

            // Calculate Resource Value
            celestialSatellite.ResourceValue = GenerateResourceValue(die);
            if (celestialSatellite.TerraformingTier == TerraformationTier.Uninhabitable) celestialSatellite.ResourceValue += die.Roll(1, 10) + 10;
            else if (celestialSatellite.TerraformingTier == TerraformationTier.T1) celestialSatellite.ResourceValue += die.Roll(1, 5);
            else if (celestialSatellite.TerraformingTier == TerraformationTier.T2) celestialSatellite.ResourceValue += die.Roll(1, 3);
            else if (celestialSatellite.TerraformingTier == TerraformationTier.T5) celestialSatellite.ResourceValue += die.Roll(1, 5) + 3;

            // Generate Sentient Species
            if (celestialSatellite.StageOfLife == LifeStage.SentientLife)
            {
                int numberOfSentients = GenerateNumberOfSentients(die);
                for (int i = 0; i < numberOfSentients; i++)
                {
                    celestialSatellite.Sentients.Add(SentientSpecies.Generate(die));
                }
            }

            return celestialSatellite;
        }

        #region Individual Generators
        public static CelestialSatelliteType GenerateCelestialSatelliteType(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 45) return CelestialSatelliteType.Moon;
            else if (percentage <= 80) return CelestialSatelliteType.Rings;
            else if (percentage <= 95) return CelestialSatelliteType.GasCloud;
            return CelestialSatelliteType.ArtificialBody;
        }
        #endregion
    }
}
