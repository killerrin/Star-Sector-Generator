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
    public class CelestialObject : BindableBase
    {
        public ObservableCollection<CelestialSatellite> OrbitingSatellites { get; set; }
        public ObservableCollection<SentientSpecies> Sentients { get; set; }

        public CelestialBodyType CelestialType { get; set; }
        public TerraformationTier TerraformingTier { get; set; }
        public LifeStage StageOfLife { get; set; }

        public int ResourceValue { get; set; }
        public int TotalResources
        {
            get
            {
                int resources = ResourceValue;
                foreach (var satellite in OrbitingSatellites)
                    resources += satellite.ResourceValue;
                return resources;
            }
        }

        public CelestialObject()
        {
            OrbitingSatellites = new ObservableCollection<CelestialSatellite>();
            Sentients = new ObservableCollection<SentientSpecies>();
            ResourceValue = 0;
        }

        public static CelestialObject Generate(Dice die)
        {
            CelestialObject celestialObject = new CelestialObject();
            celestialObject.CelestialType = GenerateCelestialBodyType(die);
            celestialObject.TerraformingTier = GenerateTerraformationTier(die);
            celestialObject.StageOfLife = GenerateStageOfLife(die);

            // Calculate Resource Value
            celestialObject.ResourceValue = GenerateResourceValue(die);
            if (celestialObject.TerraformingTier == TerraformationTier.Uninhabitable) celestialObject.ResourceValue += die.Roll(1, 10) + 10;
            else if (celestialObject.TerraformingTier == TerraformationTier.T1) celestialObject.ResourceValue += die.Roll(1, 5);
            else if (celestialObject.TerraformingTier == TerraformationTier.T2) celestialObject.ResourceValue += die.Roll(1, 3);
            else if (celestialObject.TerraformingTier == TerraformationTier.T5) celestialObject.ResourceValue += die.Roll(1, 5) + 3;

            // Generate Sentient Species
            if (celestialObject.StageOfLife == LifeStage.SentientLife)
            {
                int numberOfSentients = GenerateNumberOfSentients(die);
                for (int i = 0; i < numberOfSentients; i++)
                {
                    celestialObject.Sentients.Add(SentientSpecies.Generate(die));
                }
            }

            // Generate Celestial Satellites
            int numberOfCelestialSatellites = GenerateNumberOfCelestialSatellites(die);
            for (int i = 0; i < numberOfCelestialSatellites; i++)
            {
                celestialObject.OrbitingSatellites.Add(CelestialSatellite.Generate(die));
            }

            return celestialObject;
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
        public static int GenerateNumberOfCelestialSatellites(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 50) return 1;
            else if (percentage <= 70) return 2;
            else if (percentage <= 90) return die.Roll(2, 2);
            return die.Roll(2, 4);
        }

        public static CelestialBodyType GenerateCelestialBodyType(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10) return CelestialBodyType.Blackhole;
            else if (percentage <= 20) return CelestialBodyType.SubStar;
            else if (percentage <= 30) return CelestialBodyType.Comet;
            else if (percentage <= 40) return CelestialBodyType.AsteroidBelt;
            else if (percentage <= 65) return CelestialBodyType.GasPlanet;
            else if (percentage <= 90) return CelestialBodyType.TerrestrialPlanet;
            return CelestialBodyType.ArtificialPlanet;
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
            return die.Roll(1, 10);
        }
        #endregion
    }
}
