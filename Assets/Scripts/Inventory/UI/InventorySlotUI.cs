using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    /// <summary>
    /// Responsibility:
    /// Represents one inventory slot in the UI.
    ///
    /// InventorySlotUI only displays information and
    /// forwards player input to InventoryUI.
    /// It never modifies the inventory directly.
    /// </summary>
    public sealed class InventorySlotUI : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text quantityText;

        private InventoryUI inventoryUI;
        private int slotIndex;

        public void Initialize(
            InventoryUI inventoryUI,
            int slotIndex)
        {
            this.inventoryUI = inventoryUI;
            this.slotIndex = slotIndex;
        }

        public void Refresh(Inventory.Data.InventorySlot slot)
        {
            if (slot.IsEmpty)
            {
                icon.enabled = false;
                quantityText.gameObject.SetActive(false);
                return;
            }

            icon.enabled = true;
            icon.sprite = slot.ItemInstance.ItemData.Icon;  

            quantityText.gameObject.SetActive(slot.Quantity > 1);
            quantityText.text = slot.Quantity.ToString();
            Debug.Log (quantityText.text);
        }

        /// <summary>
        /// Called by the Button component.
        /// </summary>
        public void OnClick()
        {
            Debug.Log($"Clicked slot {slotIndex}");

            inventoryUI.OnSlotClicked(slotIndex);
        }
    }
}