using System;

namespace Emzi0767.Gaming.Sto.StoaLib.Data.Attributes
{
    /// <summary>
    /// Specifies a display name for a component
    /// </summary>
    internal class DisplayAsAttribute : Attribute
    {
        /// <summary>
        /// Gets the display name for this component
        /// </summary>
        public string DisplayName { get; internal set; }

        /// <summary>
        /// Constructs a new <see cref="DisplayAsAttribute"/> with specified <see cref="DisplayName"/>.
        /// </summary>
        /// <param name="display_name">Display Name to use</param>
        public DisplayAsAttribute(string display_name)
        {
            this.DisplayName = display_name;
        }
    }
}
