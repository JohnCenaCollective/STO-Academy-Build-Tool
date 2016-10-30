using Emzi0767.Gaming.Sto.StoaLib.Data.Enums;

namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    /// <summary>
    /// Represents a Trait.
    /// </summary>
    public struct StoAcademyTrait
    {
        /// <summary>
        /// Gets the Trait's ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the Trait's Name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the Trait's Type.
        /// </summary>
        public TraitType Type { get; internal set; }

        /// <summary>
        /// Gets the Trait's Description.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Gets the Trait's Faction.
        /// </summary>
        public TraitFaction Faction { get; internal set; }

        /// <summary>
        /// Gets the Trait's Career.
        /// </summary>
        public TraitCareer Career { get; internal set; }

        /// <summary>
        /// Gets the Trait's Species.
        /// </summary>
        public string Species { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.Name, " (", this.Type, ")");
        }
    }
}
