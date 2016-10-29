using Emzi0767.Gaming.Sto.StoaLib.Data.Enums;

namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    /// <summary>
    /// Represents an Ability.
    /// </summary>
    public struct StoAcademyAbility
    {
        /// <summary>
        /// Gets the Ability's ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the Ability's display name.
        /// </summary>
        public string DisplayName { get; internal set; }

        /// <summary>
        /// Gets the Ability's type.
        /// </summary>
        public AbilityType Type { get; internal set; }

        /// <summary>
        /// Gets the Ability's rank.
        /// </summary>
        public AbilityRank Rank { get; internal set; }

        /// <summary>
        /// Gets the Ability's career.
        /// </summary>
        public AbilityCareer Career { get; internal set; }

        /// <summary>
        /// Gets the Ability's region.
        /// </summary>
        public AbilityRegion Region { get; internal set; }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
