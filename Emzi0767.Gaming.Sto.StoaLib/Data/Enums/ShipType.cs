using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum ShipType : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Battlecruiser")]
        Battlecruiser = 1,

        [DisplayAs("Bird-of-Prey")]
        BirdOfPrey = 2,

        [DisplayAs("Carrier")]
        Carrier = 3,

        [DisplayAs("Carrier Warbird")]
        CarrierWarbird = 4,

        [DisplayAs("Cruiser")]
        Cruiser = 5,

        [DisplayAs("Destroyer")]
        Destroyer = 6,

        [DisplayAs("Dreadnought")]
        Dreadnought = 7,

        [DisplayAs("Dreadnought Cruiser")]
        DreadnoughtCruiser = 8,

        [DisplayAs("Escort")]
        Escort = 9,

        [DisplayAs("Heavy Battlecruiser")]
        HeavyBattlecruiser = 10,

        [DisplayAs("Heavy Warbird")]
        HeavyWarbird = 11,

        [DisplayAs("Light Warbird")]
        LightWarbird = 12,

        [DisplayAs("Raider")]
        Raider = 13,

        [DisplayAs("Raptor")]
        Raptor = 14,

        [DisplayAs("Science Destroyer")]
        ScienceDestroyer = 15,

        [DisplayAs("Science Vessel")]
        ScienceVessel = 16,

        [DisplayAs("Small Craft")]
        SmallCraft = 17,

        [DisplayAs("Special")]
        Special = 18,

        [DisplayAs("Support Vessel")]
        SupportVessel = 19,

        [DisplayAs("Warbird")]
        Warbird = 20,

        [DisplayAs("Warbird Battlecruiser")]
        WarbirdBattlecruiser = 21,

        [DisplayAs("Other")]
        Other = 22
    }
}
