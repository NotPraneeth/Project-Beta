using System;

namespace Inventory.Data
{
    /// <summary>
    /// Responsibility:
    /// Represents the recipe used to create a generated drug.
    ///
    /// Recipes are used as dictionary keys by crafting machines
    /// for fast recipe lookup.
    /// </summary>
    public sealed class Recipe : IEquatable<Recipe>
    {
        public int ParentDrugId { get; }

        public int AdditiveId { get; }

        public Recipe(int parentDrugId, int additiveId)
        {
            ParentDrugId = parentDrugId;
            AdditiveId = additiveId;
        }

        public bool Equals(Recipe other)
        {
            if (other is null)
                return false;

            return ParentDrugId == other.ParentDrugId &&
                   AdditiveId == other.AdditiveId;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Recipe);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(ParentDrugId, AdditiveId);
        }
    }
}