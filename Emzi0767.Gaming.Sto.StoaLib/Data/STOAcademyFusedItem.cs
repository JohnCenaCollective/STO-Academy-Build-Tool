namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    /// <summary>
    /// A Fused Item. Appears on certain ships, cannot be removed.
    /// </summary>
    public struct StoAcademyFusedItem
    {
        /// <summary>
        /// Gets the ID of the item
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the Display Name of the item
        /// </summary>
        public string DisplayName { get; internal set; }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
