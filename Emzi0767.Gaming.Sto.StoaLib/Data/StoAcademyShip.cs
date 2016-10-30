using System.Collections.Generic;
using Emzi0767.Gaming.Sto.StoaLib.Data.Enums;

namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    /// <summary>
    /// Represents a Starship.
    /// </summary>
    public struct STOAcademyStarship
    {
        /// <summary>
        /// Gets the Ship's ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the Ship's name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the Ship's type.
        /// </summary>
        public ShipType Type { get; internal set; }

        /// <summary>
        /// Gets the Ship's tier.
        /// </summary>
        public StoAcademyTier Tier { get; internal set; }

        /// <summary>
        /// Gets the Ship's useable factions.
        /// </summary>
        public ShipFactions Factions { get; internal set; }

        /// <summary>
        /// Gets whether the Ship has a secondary deflector slot.
        /// </summary>
        public bool HasSecondaryDeflector { get; internal set; }

        /// <summary>
        /// Gets the Ship's fore weapon slot count.
        /// </summary>
        public int ForeWeapons { get; internal set; }

        /// <summary>
        /// Gets the Ship's aft weapon slot count.
        /// </summary>
        public int AftWeapons { get; internal set; }

        /// <summary>
        /// Gets the Ship's base hull.
        /// </summary>
        public int BaseHull { get; internal set; }

        /// <summary>
        /// Gets the Ship's shield modifier.
        /// </summary>
        public float ShieldModifier { get; internal set; }

        /// <summary>
        /// Gets the Ship's base turn rate.
        /// </summary>
        public float BaseTurnRate { get; internal set; }

        /// <summary>
        /// Gets the Ship's device slot count.
        /// </summary>
        public int DeviceSlots { get; internal set; }

        /// <summary>
        /// Gets the Ship's engineering console slot count.
        /// </summary>
        public int ConsoleSlotsEngineering { get; internal set; }

        /// <summary>
        /// Gets the Ship's science console slot count.
        /// </summary>
        public int ConsoleSlotsScience { get; internal set; }

        /// <summary>
        /// Gets the Ship's tactical console slot count.
        /// </summary>
        public int ConsoleSlotsTactical { get; internal set; }

        /// <summary>
        /// Gets the Ship's hangar count.
        /// </summary>
        public int Hangars { get; internal set; }

        /// <summary>
        /// Gets the Ship's fused item.
        /// </summary>
        public StoAcademyFusedItem FusedItem { get; internal set; }

        /// <summary>
        /// Gets the Ship's bonus weapon power.
        /// </summary>
        public int BonusWeaponPower { get; internal set; }

        /// <summary>
        /// Gets the Ship's bonus shield power.
        /// </summary>
        public int BonusShieldPower { get; internal set; }
        
        /// <summary>
        /// Gets the Ship's bonus engine power.
        /// </summary>
        public int BonusEnginePower { get; internal set; }

        /// <summary>
        /// Gets the Ship's bonus auxiliary power.
        /// </summary>
        public int BonuxAuxiliaryPower { get; internal set; }

        /// <summary>
        /// Gets the Ship's bridge officer stations.
        /// </summary>
        public IEnumerable<StoAcademyBoffStation> BOFFStations { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.Name, " (", this.ForeWeapons, "/", this.AftWeapons, "; ", this.ConsoleSlotsEngineering, "/", this.ConsoleSlotsScience, "/", this.ConsoleSlotsTactical, ")");
        }
    }
}
