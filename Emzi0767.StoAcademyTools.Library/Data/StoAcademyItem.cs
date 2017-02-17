using System.Collections.Generic;
using Emzi0767.StoAcademyTools.Library.Data.Enums;

namespace Emzi0767.StoAcademyTools.Library.Data
{
    /// <summary>
    /// Represents an Item.
    /// </summary>
    public struct StoAcademyItem
    {
        /// <summary>
        /// Gets the Item's ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the Item's name.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the Item's modifiers.
        /// </summary>
        public IEnumerable<string> Modifiers { get; internal set; }

        /// <summary>
        /// Gets the Item's mark.
        /// </summary>
        public string Mark { get; internal set; }

        /// <summary>
        /// Gets the Item's rarity.
        /// </summary>
        public ItemRarity Rarity { get; internal set; }

        /// <summary>
        /// Gets the Item's display name.
        /// </summary>
        public string DisplayName { get; internal set; }

        /// <summary>
        /// Gets the Item's full name.
        /// </summary>
        public string FullName { get; internal set; }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
