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
    public class CelestialSatellite : BindableBase
    {
        public ObservableCollection<SentientSpecies> Sentients { get; set; }

        public CelestialSatelliteType CelestialType { get; set; }
        public TerraformationTier TerraformingTier { get; set; }
        public LifeStage StageOfLife { get; set; }
        public int ResourceValue { get; set; }


        public CelestialSatellite()
        {
            Sentients = new ObservableCollection<SentientSpecies>();
            ResourceValue = 0;
        }

        public static CelestialSatellite Generate(Dice die)
        {
            CelestialSatellite celestialSatellite = new CelestialSatellite();
            celestialSatellite.CelestialType = GenerateCelestialSatelliteType(die);
            celestialSatellite.TerraformingTier = GenerateTerraformationTier(die);
            celestialSatellite.StageOfLife = GenerateStageOfLife(die);

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
        public static int GenerateNumberOfSentients(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 50) return 1;
            else if (percentage <= 70) return die.Roll(1, 2);
            else if (percentage <= 80) return die.Roll(1, 3);
            else if (percentage <= 90) return die.Roll(2, 3);
            return die.Roll(2, 4);
        }

        public static CelestialSatelliteType GenerateCelestialSatelliteType(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 45) return CelestialSatelliteType.Moon;
            else if (percentage <= 90) return CelestialSatelliteType.Rings;
            return CelestialSatelliteType.ArtificialBody;
        }
        public static TerraformationTier GenerateTerraformationTier(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10) return TerraformationTier.Uninhabitable;
            else if (percentage <= 25) return TerraformationTier.T1;
            else if (percentage <= 40) return TerraformationTier.T2;
            else if (percentage <= 60) return TerraformationTier.T3;
            else if (percentage <= 80) return TerraformationTier.T4;
            return TerraformationTier.T5;
        }
        public static LifeStage GenerateStageOfLife(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10) return LifeStage.None;
            else if (percentage <= 20) return LifeStage.OrganicCompounds;
            else if (percentage <= 25) return LifeStage.SingleCellular;
            else if (percentage <= 40) return LifeStage.MultiCellular;
            else if (percentage <= 60) return LifeStage.SimpleLife;
            else if (percentage <= 80) return LifeStage.ComplexLife;
            return LifeStage.SentientLife;
        }

        public static int GenerateResourceValue(Dice die)
        {
            return die.Roll(1, 5);
        }
        #endregion
    }
}
