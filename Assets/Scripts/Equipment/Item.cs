using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : Interactable {
    public enum Type {
        Staff,
        Sword,
        Bow,
        Shield,
        Armor
    };

    public string Name;
    public Type ItemType;
    public GameObject ItemReference;

    public override void Interact() {

    }
}
