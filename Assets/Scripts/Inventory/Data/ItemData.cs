using UnityEngine;

namespace Inventory.Data
{
    /// <summary>
    /// Responsibility:
    /// Defines an immutable item type.
    ///
    /// ItemData answers the question:
    /// "What is this item?"
    ///
    /// It does NOT contain runtime information such as:
    /// - Quality
    /// - Quantity
    /// - Inventory location
    ///
    /// Static items are created as ScriptableObject assets.
    /// Generated drugs are created at runtime using
    /// ScriptableObject.CreateInstance().
    /// </summary>
    public abstract class ItemData : ScriptableObject
    {
        /// <summary>
        /// Unique identifier for this item definition.
        /// </summary>
        [SerializeField]
        private int id;
        public int Id => id;

        /// <summary>
        /// Player-facing item name.
        /// </summary>
        [SerializeField]
        private string displayName;
        public string DisplayName => displayName;

        /// <summary>
        /// Player-facing item description.
        /// </summary>
        [SerializeField]
        [TextArea]
        private string description;
        public string Description => description;

        /// <summary>
        /// Icon displayed in inventory and UI.
        /// </summary>
        [SerializeField]
        private Sprite icon;
        public Sprite Icon => icon;

        /// <summary>
        /// Prefab spawned when this item exists in the world.
        /// </summary>
        [SerializeField]
        private GameObject worldPrefab;
        public GameObject WorldPrefab => worldPrefab;

        /// <summary>
        /// Maximum number of identical items that may occupy
        /// a single inventory slot.
        /// </summary>
        [SerializeField]
        [Min(1)]
        private int maxStackSize = 1;
        public int MaxStackSize => maxStackSize;

        /// <summary>
        /// Category used by gameplay systems such as crafting
        /// stations and packaging machines.
        /// </summary>
        [SerializeField]
        private ItemType itemType;
        public ItemType ItemType => itemType;
    }
}