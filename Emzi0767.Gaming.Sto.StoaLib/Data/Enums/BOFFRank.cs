using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum BoffRank : int
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
