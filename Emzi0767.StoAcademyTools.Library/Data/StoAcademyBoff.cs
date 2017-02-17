using System.Collections.Generic;
using Emzi0767.StoAcademyTools.Library.Data.Enums;

namespace Emzi0767.StoAcademyTools.Library.Data
{
    /// <summary>
    /// Represents a Bridge Officer in a Station.
    /// </summary>
    public struct StoAcademyBoff
    {
        /// <summary>
        /// Gets all Abilities for this station.
        /// </summary>
        public IEnumerable<StoAcademyAbility> Abilities { get; internal set; }

        /// <summary>
        /// Gets the Officer's career.
        /// </summary>
        public BoffCareer Career { get; internal set; }

        /// <summary>
        /// Gets the Officer's specialization.
        /// </summary>
        public BoffSpecialization Specialization { get; internal set; }

        /// <summary>
        /// Gets the Officer's rank.
        /// </summary>
        public BoffRank Rank { get; internal set; }

        /// <summary>
        /// Gets the Officer's station.
        /// </summary>
        public StoAcademyBoffStation Station { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.Rank, " ", this.Career, "/", this.Specialization);
        }
    }
}
