using Emzi0767.StoAcademyTools.Library.Data.Enums;

namespace Emzi0767.StoAcademyTools.Library.Data
{
    /// <summary>
    /// Represents a Bridge Officer Station
    /// </summary>
    public struct StoAcademyBoffStation
    {
        /// <summary>
        /// Gets the Station's career.
        /// </summary>
        public BoffStationCareer Career { get; internal set; }

        /// <summary>
        /// Gets the Station's rank.
        /// </summary>
        public BoffStationRank Rank { get; internal set; }

        /// <summary>
        /// Gets the Station's specialization.
        /// </summary>
        public BoffStationSpecialization Specialization { get; internal set; }

        public override string ToString()
        {
            return string.Concat("Career: ", this.Career, " | Rank: ", this.Rank, " | Specialization: ", this.Specialization);
        }
    }
}
