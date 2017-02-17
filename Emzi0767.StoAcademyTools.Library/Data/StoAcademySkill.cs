using Emzi0767.StoAcademyTools.Library.Data.Enums;

namespace Emzi0767.StoAcademyTools.Library.Data
{
    /// <summary>
    /// Represents a Skill.
    /// </summary>
    public struct StoAcademySkill
    {
        /// <summary>
        /// Gets the Skill's ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the Skills's text ID. This is used to identify and refer to the Skill.
        /// </summary>
        public string SkillID { get; internal set; }

        /// <summary>
        /// Gets the Skill's required skill text ID.
        /// </summary>
        public string RequiredSkillID { get; internal set; }

        /// <summary>
        /// Gets the Skill's mame.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the Skill's region.
        /// </summary>
        public SkillRegion Region { get; internal set; }

        /// <summary>
        /// Gets the Skill's career.
        /// </summary>
        public SkillCareer Career { get; internal set; }

        /// <summary>
        /// Gets the Skill's rank.
        /// </summary>
        public SkillRank Rank { get; internal set; }

        /// <summary>
        /// Gets the Skill's description.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Gets the Skill's bonus.
        /// </summary>
        public string Bonus { get; internal set; }

        /// <summary>
        /// Gets the Skill's total bonus.
        /// </summary>
        public string TotalBonus { get; internal set; }
        
        /// <summary>
        /// Gets the Skill's base bonus.
        /// </summary>
        public string BaseBonus { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.SkillID, ": ", this.Name);
        }
    }
}
