using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum BuildSpecializationSecondary : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("None")]
        None = 1,

        [DisplayAs("Intelligence")]
        Intelligence = 2,

        [DisplayAs("Command")]
        Command = 3,

        [DisplayAs("Pilot")]
        Pilot = 4,

        [DisplayAs("Temporal Operative")]
        TemporalOperative = 5,

        [DisplayAs("Commando")]
        Commando = 6,

        [DisplayAs("Strategist")]
        Strategist = 7
    }
}
