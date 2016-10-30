using System;
using System.Collections.Generic;
using Emzi0767.Gaming.Sto.StoaLib.Data.Enums;

namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    /// <summary>
    /// Represents a STO Academy Build.
    /// </summary>
    public struct StoAcademyBuild
    {
        /// <summary>
        /// Gets the Build's owner.
        /// </summary>
        public string Owner { get; internal set; }

        /// <summary>
        /// Gets the Build's name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the Build's ID.
        /// </summary>
        public string ID { get; internal set; }

        /// <summary>
        /// Gets the Build's URI.
        /// </summary>
        public Uri Url { get { return new Uri(string.Concat("http://skillplanner.stoacademy.com/", this.ID)); } }

        /// <summary>
        /// Gets the Build's career.
        /// </summary>
        public BuildCareer Career { get; internal set; }

        /// <summary>
        /// Gets the Build's faction.
        /// </summary>
        public BuildFaction Faction { get; internal set; }

        /// <summary>
        /// Gets the Build's species.
        /// </summary>
        public string Species { get; internal set; }

        /// <summary>
        /// Gets the Build's space items.
        /// </summary>
        public IEnumerable<StoAcademyBuildItem> BuildItems { get; internal set; }

        /// <summary>
        /// Gets the Build's ground equipment.
        /// </summary>
        public IEnumerable<StoAcademyBuildItem> BuildEquipment { get; internal set; }

        /// <summary>
        /// Gets the Build's bridge officers.
        /// </summary>
        public IEnumerable<StoAcademyBoff> BOFFs { get; internal set; }

        /// <summary>
        /// Gets the Build's away team.
        /// </summary>
        public IEnumerable<StoAcademyBoff> AwayTeam { get; internal set; }

        /// <summary>
        /// Gets the Build's duty officers.
        /// </summary>
        public IEnumerable<StoAcademyBuildDoff> DOFFs { get; internal set; }

        /// <summary>
        /// Gets the Build's ship.
        /// </summary>
        public STOAcademyStarship Ship { get; internal set; }

        /// <summary>
        /// Gets the Build's traits.
        /// </summary>
        public IEnumerable<StoAcademyTrait> Traits { get; internal set; }

        /// <summary>
        /// Gets the Build's skills.
        /// </summary>
        public IEnumerable<StoAcademySkill> Skills { get; internal set; }

        /// <summary>
        /// Gets the Build's skill unlocks.
        /// </summary>
        public IEnumerable<StoAcademySkillUnlock> SkillUnlocks { get; internal set; }

        /// <summary>
        /// Gets the Build's primary specialization.
        /// </summary>
        public BuildSpecializationPrimary SpecializationPrimary { get; internal set; }

        /// <summary>
        /// Gets the Build's secondary specialization.
        /// </summary>
        public BuildSpecializationSecondary SpecializationSecondary { get; internal set; }

        /// <summary>
        /// Gets the Build's description.
        /// </summary>
        public string Description { get; internal set; }

        /// <summary>
        /// Gets the Build's notes, formatted with HTML.
        /// </summary>
        public string Notes { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.Owner, "'s ", this.Name, " (", this.ID, ")");
        }
    }
}
