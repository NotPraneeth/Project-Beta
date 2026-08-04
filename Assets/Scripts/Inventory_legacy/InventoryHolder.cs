//using UnityEngine;

///// <summary>
///// Unity component that owns an Inventory instance.
///// Attach this to any GameObject that should be able to store items.
///// </summary>
//public class InventoryHolder : MonoBehaviour
//{
//    //============================================================
//    // Fields & Properties
//    //============================================================

//    [Header("Inventory Settings")]
//    [SerializeField]
//    [Min(1)]
//    private int capacity = 20;

//    /// <summary>
//    /// The inventory owned by this GameObject.
//    /// </summary>
//    public Inventory Inventory { get; private set; }

//    //============================================================
//    // Unity Events
//    //============================================================

//    private void Awake()
//    {
//        Inventory = new Inventory(capacity);
//    }
//}