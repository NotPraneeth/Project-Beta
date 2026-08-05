using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Inventory.Data;

namespace Inventory.UI
{
    public sealed class InventorySlotUI :
        MonoBehaviour,
        IBeginDragHandler,
        IDragHandler
    {
        [SerializeField]
        private Image icon;

        [SerializeField]
        private TMP_Text quantityText;

        private InventoryUI inventoryUI;
        private int slotIndex;

        public int SlotIndex => slotIndex;

        public void Initialize(
            InventoryUI inventoryUI,
            int slotIndex)
        {
            this.inventoryUI = inventoryUI;
            this.slotIndex = slotIndex;
        }

        public void Refresh(InventorySlot slot)
        {
            if (slot.IsEmpty)
            {
                icon.enabled = false;
                quantityText.gameObject.SetActive(false);
                return;
            }

            icon.enabled = true;
            icon.sprite = slot.ItemInstance.ItemData.Icon;

            quantityText.text = slot.Quantity.ToString();
            quantityText.gameObject.SetActive(slot.Quantity > 1);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            inventoryUI.BeginDrag(slotIndex);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}