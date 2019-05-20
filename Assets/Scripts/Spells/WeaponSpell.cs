using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponSpell : Spell {
    public float WeaponPercentDamage;
    public int GetWeaponDamage() {
        return this.GetComponentInParent<Entity>().GetComponentsInChildren<Weapon>()[0].WeaponAttack;
    }

    public override int GetDamage() {
        return (int)Mathf.Round(this.GetComponentInParent<Entity>().SpellDamageAmp * (PrimarySpellDamage + WeaponPercentDamage * GetWeaponDamage()));
    }
}