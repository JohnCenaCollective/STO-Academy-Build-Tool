using Emzi0767.StoAcademyTools.Library.Data.Attributes;

namespace Emzi0767.StoAcademyTools.Library.Data.Enums
{
    public enum BuildItemType : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Fore Weapon")]
        ForeWeapon = 1,

        [DisplayAs("Aft Weapon")]
        AftWeapon = 2,

        [DisplayAs("Deflector")]
        Deflector = 3,

        [DisplayAs("Secondary Deflector")]
        SecondaryDeflector = 4,

        [DisplayAs("Impulse Engine")]
        ImpulseEngine = 5,

        [DisplayAs("Warp Core")]
        CoreWarp = 6,

        [DisplayAs("Singularity Core")]
        CoreSingularity = 7,

        [DisplayAs("Shield Array")]
        Shields = 8,

        [DisplayAs("Device")]
        Device = 9,

        [DisplayAs("Engineering Console")]
        ConsoleEngineering = 10,

        [DisplayAs("Science Console")]
        ConsoleScience = 11,

        [DisplayAs("Tactical Console")]
        ConsoleTactical = 12,

        [DisplayAs("Hangar Pet")]
        Hangar = 13,

        [DisplayAs("Kit Frame")]
        Kit = 14,

        [DisplayAs("Kit Module")]
        KitModule = 15,

        [DisplayAs("Armor")]
        Armor = 16,

        [DisplayAs("Personal Shield Generator")]
        PersonalShield = 17,

        [DisplayAs("Weapon")]
        Weapon = 18,

        [DisplayAs("Device")]
        GroundDevice = 19
    }
}
