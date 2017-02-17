using Emzi0767.StoAcademyTools.Library.Data.Enums;

namespace Emzi0767.StoAcademyTools.Library.Data
{
    /// <summary>
    /// Represents a Build's duty officer.
    /// </summary>
    public struct StoAcademyBuildDoff
    {
        /// <summary>
        /// Gets the associated Duty Officer.
        /// </summary>
        public StoAcademyDoff DOFF { get; internal set; }

        /// <summary>
        /// Gets the Officer's rarity.
        /// </summary>
        public BuildDoffRarity Rarity { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.DOFF.Specialization, " (", this.Rarity, "; ", this.DOFF.Ability, ")");
        }
    }
}
