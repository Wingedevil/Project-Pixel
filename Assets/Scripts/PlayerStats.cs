using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerStats {
    public const int NUMBER_OF_ITEM_SLOTS = 16;

    public static ArrayList Inventory {
        get;
        private set;
    } = new ArrayList();

    public static void InventoryAddItem(GameObject add) {
        Inventory.Add(add);
    }

    public static void InventoryRemoveItem(GameObject remove) {
        Inventory.Remove(remove);
    }
}
