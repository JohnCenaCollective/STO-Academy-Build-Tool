﻿using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum DoffRegion : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Space")]
        Space = 1,

        [DisplayAs("Ground")]
        Ground = 2,

        [DisplayAs("Space/Ground")]
        Agnostic = 3
    }
}
