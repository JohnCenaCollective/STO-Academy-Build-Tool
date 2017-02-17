using Emzi0767.StoAcademyTools.Library.Data.Attributes;

namespace Emzi0767.StoAcademyTools.Library.Data.Enums
{
    public enum BoffStationRank : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Ensign")]
        Ensign = 1,

        [DisplayAs("Lieutenant")]
        Lieutenant = 2,

        [DisplayAs("Lt. Commander")]
        LieutenantCommander = 3,

        [DisplayAs("Commander")]
        Commander = 4
    }
}
