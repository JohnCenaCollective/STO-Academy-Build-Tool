using Emzi0767.StoAcademyTools.Library.Data.Attributes;

namespace Emzi0767.StoAcademyTools.Library.Data.Enums
{
    public enum TraitFaction : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Federation")]
        Federation = 1,

        [DisplayAs("Klingon Empire")]
        KlingonEmpire = 2,

        [DisplayAs("Romulan Republic")]
        RomulanRepublic = 3,

        [DisplayAs("Universal")]
        Universal = 4
    }
}
