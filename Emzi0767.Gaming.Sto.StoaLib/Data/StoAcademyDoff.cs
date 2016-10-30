using Emzi0767.Gaming.Sto.StoaLib.Data.Enums;

namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    /// <summary>
    /// Represents a Duty Officer.
    /// </summary>
    public struct StoAcademyDoff
    {
        /// <summary>
        /// Gets the DOFF's ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the DOFF's specialization.
        /// </summary>
        public DoffSpecialization Specialization { get; internal set; }

        /// <summary>
        /// Gets the DOFF's region.
        /// </summary>
        public DoffRegion Region { get; internal set; }

        /// <summary>
        /// Gets the DOFF's maximum count.
        /// </summary>
        public int MaximumCount { get; internal set; }

        /// <summary>
        /// Gets the DOFF's ability.
        /// </summary>
        public string Ability { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.Specialization, " (", this.Ability, ")");
        }
    }
}
