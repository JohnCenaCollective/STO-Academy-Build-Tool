using Emzi0767.Gaming.Sto.StoaLib.Data.Enums;

namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    /// <summary>
    /// Represents a Build Item.
    /// </summary>
    public struct StoAcademyBuildItem
    {
        /// <summary>
        /// Gets the corresponding Item.
        /// </summary>
        public StoAcademyItem Item { get; internal set; }

        /// <summary>
        /// Gets the Item's slot.
        /// </summary>
        public BuildItemType ItemType { get; internal set; }

        public override string ToString()
        {
            return string.Concat(this.ItemType, ": ", this.Item.DisplayName);
        }
    }
}
