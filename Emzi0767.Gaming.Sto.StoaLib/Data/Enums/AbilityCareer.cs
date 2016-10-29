using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum AbilityCareer : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Engineering")]
        Engineering = 1,

        [DisplayAs("Science")]
        Science = 2,

        [DisplayAs("Tactical")]
        Tactical = 3,

        [DisplayAs("Intelligence")]
        Intelligence = 4,

        [DisplayAs("Command")]
        Command = 5,

        [DisplayAs("Pilot")]
        Pilot = 6,

        [DisplayAs("Temporal Operative")]
        TemporalOperative = 7,

        [DisplayAs("Universal")]
        Universal = 8
    }
}
