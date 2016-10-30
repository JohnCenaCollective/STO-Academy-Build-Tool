using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum BoffCareer : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Universal")]
        Universal = 1,

        [DisplayAs("Tactical")]
        Tactical = 2,

        [DisplayAs("Engineering")]
        Engineering = 3,

        [DisplayAs("Science")]
        Science = 4
    }
}
