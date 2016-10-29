using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum BuildFaction : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Federation")]
        Federation = 1,

        [DisplayAs("Klingon Empire")]
        KlingonEmpire = 2,

        [DisplayAs("Romulan Republic")]
        RomulanRepublic = 3
    }
}
