using System;
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

    public override void Interact(Entity e) {
        try {
            e.PhysicalArmor += ItemReference.GetComponent<Equipable>().PhysicalArmor;
            e.PhysicalReduction += ItemReference.GetComponent<Equipable>().PhysicalReduction;
            e.MagicalResistance *= ItemReference.GetComponent<Equipable>().MagicalResistance;
        } catch (Exception ex) {
            Debug.Log(ex);
        }
    }

    public override void Uninteract(Entity e) {
        try {
            e.PhysicalArmor -= ItemReference.GetComponent<Equipable>().PhysicalArmor;
            e.PhysicalReduction -= ItemReference.GetComponent<Equipable>().PhysicalReduction;
            e.MagicalResistance /= ItemReference.GetComponent<Equipable>().MagicalResistance;
        } catch (Exception ex) {
            Debug.Log(ex);
        }
    }
}
