using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum TraitType : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Active Reputation Trait")]
        ReputationActive = 1,

        [DisplayAs("Reputation Space Trait")]
        ReputationSpace = 2,

        [DisplayAs("Reputation Ground Trait")]
        ReputationGround = 3,

        [DisplayAs("Personal Space Trait")]
        PersonalSpace = 4,

        [DisplayAs("Personal Ground Trait")]
        PersonalGround = 5,

        [DisplayAs("Starship Trait")]
        Starship = 6,

        [DisplayAs("Other Trait")]
        Other = 7
    }
}
