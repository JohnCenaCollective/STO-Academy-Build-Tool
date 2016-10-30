using Emzi0767.Gaming.Sto.StoaLib.Data.Attributes;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Enums
{
    public enum BuildDoffRarity : int
    {
        [DisplayAs("N/A")]
        Unknown = 0,

        [DisplayAs("Common")]
        Common = 1,

        [DisplayAs("Uncommon")]
        Uncommon = 2,

        [DisplayAs("Rare")]
        Rare = 3,

        [DisplayAs("Very Rare")]
        VeryRare = 4,

        [DisplayAs("Ultra Rare")]
        UltraRare = 5,

        [DisplayAs("Epic")]
        Epic = 6
    }
}
