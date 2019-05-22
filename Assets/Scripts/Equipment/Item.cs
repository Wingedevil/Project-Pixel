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

    protected override void Start() {
        base.Start();
        this.GetComponent<SpriteRenderer>().sortingOrder = -3;
    }

    public override void Interact(Entity e) {
        try {
            e.MaxMP += ItemReference.GetComponent<Equipable>().MaxMP;
            e.MaxSP += ItemReference.GetComponent<Equipable>().MaxSP;
            e.MPCost *= ItemReference.GetComponent<Equipable>().MPCost;
            e.SPCost *= ItemReference.GetComponent<Equipable>().SPCost;
            e.PhysicalArmor += ItemReference.GetComponent<Equipable>().PhysicalArmor;
            e.PhysicalReduction += ItemReference.GetComponent<Equipable>().PhysicalReduction;
            e.MagicalResistance *= ItemReference.GetComponent<Equipable>().MagicalResistance;
            e.AttackSpeed *= ItemReference.GetComponent<Equipable>().AttackSpeed;
            e.SpellDamageAmp *= ItemReference.GetComponent<Equipable>().SpellDamageAmp;
        } catch (Exception ex) {
            Debug.Log(ex);
        }
    }

    public override void Uninteract(Entity e) {
        try {
            e.MaxMP -= ItemReference.GetComponent<Equipable>().MaxMP;
            e.curMP = e.curMP > e.MaxMP ? e.MaxMP : e.curMP;
            
            e.MaxSP -= ItemReference.GetComponent<Equipable>().MaxSP;
            e.curSP = e.curSP > e.MaxSP ? e.MaxSP : e.curSP;

            e.MPCost /= ItemReference.GetComponent<Equipable>().MPCost;
            e.SPCost /= ItemReference.GetComponent<Equipable>().SPCost;
            e.PhysicalArmor -= ItemReference.GetComponent<Equipable>().PhysicalArmor;
            e.PhysicalReduction -= ItemReference.GetComponent<Equipable>().PhysicalReduction;
            e.MagicalResistance /= ItemReference.GetComponent<Equipable>().MagicalResistance;
            e.AttackSpeed /= ItemReference.GetComponent<Equipable>().AttackSpeed;
            e.SpellDamageAmp /= ItemReference.GetComponent<Equipable>().SpellDamageAmp;
        } catch (Exception ex) {
            Debug.Log(ex);
        }
    }
}
