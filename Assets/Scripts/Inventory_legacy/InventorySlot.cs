//using UnityEngine;

//[System.Serializable]
//public class InventorySlot
//{
//    public ItemData Item { get; private set; }

//    public int Quantity { get; private set; }

//    public bool IsEmpty => Item == null;

//    public bool IsFull =>
//        !IsEmpty &&
//        Item.Stackable &&
//        Quantity >= Item.MaxStack;

//    public int RemainingSpace =>
//        IsEmpty ? 0 : Item.MaxStack - Quantity;

//    public bool CanStack(ItemData item)
//    {
//        return !IsEmpty &&
//               Item == item &&
//               Item.Stackable &&
//               !IsFull;
//    }

//    public void Set(ItemData item, int quantity)
//    {
//        Item = item;
//        Quantity = quantity;
//    }

//    public void AddQuantity(int amount)
//    {
//        Quantity += amount;
//    }

//    public void RemoveQuantity(int amount)
//    {
//        Quantity -= amount;

//        if (Quantity <= 0)
//            Clear();
//    }

//    public void Clear()
//    {
//        Item = null;
//        Quantity = 0;
//    }
//}