using System.Collections.Generic;

namespace Inventory.Data
{
    /// <summary>
    /// Responsibility:
    /// Represents a procedurally generated drug discovered
    /// during gameplay.
    ///
    /// Generated drugs are created at runtime the first time
    /// a player discovers a new recipe.
    /// </summary>
    public sealed class GeneratedDrugData : ItemData
    {
        /// <summary>
        /// Recipe used to create this drug.
        /// Stored for save files, UI and recipe history.
        /// </summary>
        public Recipe Recipe { get; private set; }

        /// <summary>
        /// Intrinsic value of this drug.
        /// Used as the baseline for economy calculations.
        /// </summary>
        public float FairPrice { get; private set; }

        /// <summary>
        /// Selling price chosen by the player.
        /// This price is shared across the player's business.
        /// </summary>
        public float SellingPrice { get; private set; }

        /// <summary>
        /// Properties possessed by this drug.
        /// Regions compare these against their preferred
        /// properties when generating demand.
        /// </summary>
        public List<DrugEffect> Effects { get; private set; } = new();
    }
}