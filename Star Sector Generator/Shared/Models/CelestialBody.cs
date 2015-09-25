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
    public class CelestialBody : CelestialBase
    {
        public ObservableCollection<CelestialSatellite> OrbitingSatellites { get; set; }
        public CelestialBodyType CelestialType { get; set; }

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

        public CelestialBody()
            :base()
        {
            OrbitingSatellites = new ObservableCollection<CelestialSatellite>();
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
                //str += "\n\tSentients";
                str += "\n";
                foreach (var sentient in Sentients)
                {
                    str += string.Format("\t{0}\n", sentient);
                }
            }

            if (OrbitingSatellites.Count > 0)
            {
                //str += "\n\tSatellites";
                str += "\n";
                foreach (var satellite in OrbitingSatellites)
                {
                    str += string.Format("\t{0}\n", satellite);
                }
            }

            return str;
        }

        public static CelestialBody Generate(Dice die)
        {
            CelestialBody celestialObject = new CelestialBody();
            celestialObject.CelestialType = GenerateCelestialBodyType(die);

            // Terraforming Tier
            if (celestialObject.CelestialType == CelestialBodyType.Blackhole ||
                celestialObject.CelestialType == CelestialBodyType.SubStar ||
                celestialObject.CelestialType == CelestialBodyType.Wormhole ||
                celestialObject.CelestialType == CelestialBodyType.Comet)
            {
                celestialObject.TerraformingTier = TerraformationTier.Uninhabitable;
            }
            else celestialObject.TerraformingTier = GenerateTerraformationTier(die);

            // Stage of Life
            if (celestialObject.TerraformingTier == TerraformationTier.Uninhabitable ||
                celestialObject.CelestialType == CelestialBodyType.Blackhole ||
                celestialObject.CelestialType == CelestialBodyType.SubStar ||
                celestialObject.CelestialType == CelestialBodyType.Wormhole ||
                celestialObject.CelestialType == CelestialBodyType.Comet)
            {
                celestialObject.StageOfLife = LifeStage.NoLife;
            }
            else if (celestialObject.TerraformingTier == TerraformationTier.T1 ||
                     celestialObject.TerraformingTier == TerraformationTier.T2 ||
                     celestialObject.CelestialType == CelestialBodyType.GasPlanet)
            {
                celestialObject.StageOfLife = GenerateStageOfLifeT1T2(die);
            }
            else celestialObject.StageOfLife = GenerateStageOfLife(die);

            // Calculate Resource Value
            celestialObject.ResourceValue = GenerateResourceValue(die);
            if (celestialObject.CelestialType == CelestialBodyType.Blackhole || celestialObject.CelestialType == CelestialBodyType.Wormhole)
            {
                celestialObject.ResourceValue = 0;
            }
            else if (celestialObject.TerraformingTier == TerraformationTier.Uninhabitable) celestialObject.ResourceValue += die.Roll(1, 10) + 10;
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
            if (celestialObject.CelestialType == CelestialBodyType.Comet) { }
            else if (celestialObject.CelestialType == CelestialBodyType.Wormhole) { }
            //else if (celestialObject.CelestialType == CelestialBodyType.AsteroidBelt) { }
            else
            {
                int numberOfCelestialSatellites = GenerateNumberOfCelestialSatellites(die);
                for (int i = 0; i < numberOfCelestialSatellites; i++)
                {
                    celestialObject.OrbitingSatellites.Add(CelestialSatellite.Generate(die));
                }
            }

            return celestialObject;
        }

        #region Individual Generators
        public static int GenerateNumberOfCelestialSatellites(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 60) return 1;
            else if (percentage <= 80) return 2;
            else if (percentage <= 95) return die.Roll(1, 4);
            return die.Roll(2, 4);
        }

        public static CelestialBodyType GenerateCelestialBodyType(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 7) return CelestialBodyType.Blackhole;
            else if (percentage <= 15) return CelestialBodyType.SubStar;
            else if (percentage <= 25) return CelestialBodyType.Comet;
            else if (percentage <= 40) return CelestialBodyType.AsteroidBelt;
            else if (percentage <= 65) return CelestialBodyType.GasPlanet;
            else if (percentage <= 95) return CelestialBodyType.TerrestrialPlanet;
            return CelestialBodyType.ArtificialPlanet;
        }
        #endregion
    }
}
