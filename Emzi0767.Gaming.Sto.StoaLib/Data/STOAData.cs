using System.Collections.ObjectModel;

namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    internal struct StoaData
    {
        public ReadOnlyDictionary<int, StoAcademyAbility> AllAbilities { get; set; }
        public ReadOnlyDictionary<int, StoAcademyDOFF> AllDOFFs { get; set; }
        public ReadOnlyDictionary<int, StoAcademyFusedItem> AllFusedItems { get; set; }
        public ReadOnlyDictionary<int, STOAcademyStarship> AllShips { get; set; }
        public ReadOnlyDictionary<string, StoAcademySkill> AllSkills { get; set; }
        public ReadOnlyDictionary<string, StoAcademySkillUnlock> AllSkillUnlocks { get; set; }
        public ReadOnlyDictionary<int, StoAcademyTier> AllTiers { get; set; }
        public ReadOnlyDictionary<int, StoAcademyTrait> AllTraits { get; set; }

        public override string ToString()
        {
            return string.Concat("Abilities: ", this.AllAbilities.Count, 
                " | DOFFs: ", this.AllDOFFs.Count, 
                " | Fused Items: ", this.AllFusedItems.Count, 
                " | Ships: ", this.AllShips.Count, 
                " | Skills: ", this.AllSkills.Count,
                " | Skill Unlocks: ", this.AllSkillUnlocks.Count,
                " | Tiers: ", this.AllTiers.Count, 
                " | Traits: ", this.AllTraits.Count);
        }
    }
}
