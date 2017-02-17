using Emzi0767.StoAcademyTools.Library.Data.Attributes;

namespace Emzi0767.StoAcademyTools.Library.Data.Enums
{
    public enum BoffStationCareer : int
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
