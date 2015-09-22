using Shared.DnDTools;
using Shared.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Shared.Models
{
    public abstract class CelestialBase : BindableBase
    {
        public ObservableCollection<SentientSpecies> Sentients { get; set; }

        public TerraformationTier TerraformingTier { get; set; }
        public LifeStage StageOfLife { get; set; }
        public int ResourceValue { get; set; }

        protected CelestialBase()
        {
            Sentients = new ObservableCollection<SentientSpecies>();
            ResourceValue = 0;
        }

        #region Individual Generators
        public static int GenerateNumberOfSentients(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 80) return 1;
            else if (percentage <= 90) return 2;
            else if (percentage <= 95) return die.Roll(1, 2) + 1;
            return die.Roll(2, 3);
        }
        public static TerraformationTier GenerateTerraformationTier(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 6) return TerraformationTier.Uninhabitable;
            else if (percentage <= 20) return TerraformationTier.T1;
            else if (percentage <= 35) return TerraformationTier.T2;
            else if (percentage <= 70) return TerraformationTier.T3;
            else if (percentage <= 85) return TerraformationTier.T4;
            return TerraformationTier.T5;
        }
        public static LifeStage GenerateStageOfLife(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 10) return LifeStage.NoLife;
            else if (percentage <= 15) return LifeStage.OrganicCompounds;
            else if (percentage <= 25) return LifeStage.SingleCellular;
            else if (percentage <= 35) return LifeStage.MultiCellular;
            else if (percentage <= 60) return LifeStage.SimpleLife;
            else if (percentage <= 96) return LifeStage.ComplexLife;
            return LifeStage.SentientLife;
        }
        public static LifeStage GenerateStageOfLifeT1T2(Dice die)
        {
            int percentage = die.Roll(1, 100);

            if (percentage <= 20) return LifeStage.NoLife;
            else if (percentage <= 30) return LifeStage.OrganicCompounds;
            else if (percentage <= 40) return LifeStage.SingleCellular;
            else if (percentage <= 75) return LifeStage.MultiCellular;
            else if (percentage <= 90) return LifeStage.SimpleLife;
            return LifeStage.ComplexLife;
        }

        public static int GenerateResourceValue(Dice die)
        {
            return die.Roll(1, 10);
        }
        #endregion
    }
}
