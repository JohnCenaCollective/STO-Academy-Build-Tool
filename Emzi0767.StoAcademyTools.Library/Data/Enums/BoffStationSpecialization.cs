﻿using Emzi0767.StoAcademyTools.Library.Data.Attributes;

namespace Emzi0767.StoAcademyTools.Library.Data.Enums
{
    public enum BoffStationSpecialization : int
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
        TemporalOperative = 5
    }
}
