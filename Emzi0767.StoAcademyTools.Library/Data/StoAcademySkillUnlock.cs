using Emzi0767.StoAcademyTools.Library.Data.Enums;

namespace Emzi0767.StoAcademyTools.Library.Data
{
    /// <summary>
    /// Represents a Skill Unlock
    /// </summary>
    public struct StoAcademySkillUnlock
    {
        /// <summary>
        /// Gets the Unlock's ID.
        /// </summary>
        public string UnlockID { get; internal set; }

        /// <summary>
        /// Gets the Unlock's name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the Unlock's requirements.
        /// </summary>
        public SkillUnlockPointCount PointCount { get; internal set; }

        /// <summary>
        /// Gets the Unlock's career.
        /// </summary>
        public SkillUnlockCareer Career { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.UnlockID, ": ", this.Name);
        }
    }
}
