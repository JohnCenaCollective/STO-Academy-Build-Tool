namespace Emzi0767.Gaming.Sto.StoaLib.Data
{
    /// <summary>
    /// Represents a Ship Tier.
    /// </summary>
    public struct StoAcademyTier
    {
        /// <summary>
        /// Gets the Tier's ID.
        /// </summary>
        public int ID { get; internal set; }

        /// <summary>
        /// Gets the Tier's list order.
        /// </summary>
        public int Order { get; internal set; }

        /// <summary>
        /// Gets the Tier's level requirement.
        /// </summary>
        public int Level { get; internal set; }

        /// <summary>
        /// Gets the Tier's name.
        /// </summary>
        public string Name { get; internal set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
