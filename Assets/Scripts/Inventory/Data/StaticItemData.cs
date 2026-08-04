using UnityEngine;

namespace Inventory.Data
{
    /// <summary>
    /// Responsibility:
    /// Represents predefined items created in the Unity Editor.
    ///
    /// StaticItemData is used for items that exist before the game starts,
    /// such as:
    /// - Ibuprofen
    /// - Plastic
    /// - Aluminum Foil
    /// - Base Drugs
    ///
    /// Unlike GeneratedDrugData, these are stored as ScriptableObject
    /// assets inside the project.
    /// </summary>
    [CreateAssetMenu(
        fileName = "New Static Item",
        menuName = "Inventory/Static Item"
    )]
    public sealed class StaticItemData : ItemData
    {
    }
}