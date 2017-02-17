using System;
using Emzi0767.StoAcademyTools.Library.Data.Attributes;

namespace Emzi0767.StoAcademyTools.Library.Data.Enums
{
    [Flags]
    public enum ShipFactions : int
    {
        [DisplayAs("None")]
        None = 0,

        [DisplayAs("Federation")]
        Federation = 1,

        [DisplayAs("Klingon Empire")]
        KlingonEmpire = 2,

        [DisplayAs("Romulan Republic")]
        RomulanRepublic = 4
    }
}
