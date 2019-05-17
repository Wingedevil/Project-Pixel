using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Equipable {
    public static Vector3 DISPLACEMENT = new Vector3(0f, -3.5f / 16f, 0f);
    public static float MELEE_DISPLACEMENT = 0.3f;
    public static float RANGED_DISPLACEMENT = 0f;

    public int WeaponAttack;
    public bool IsMelee = false;
    public GameObject Projectile;
    public float ProjectileScale = 1.0f;

    private bool isListening = true;
    private Vector3 direction;
    private bool isSpell;
    private string spellName;
    private Spell spell;
    private string owner;
    private Action callback;

    // Start is called before the first frame update
    void Start() {
        direction = Vector3.zero;
        isSpell = false;
        spellName = "";
    }

    public void Shoot(Vector3 dir, string owner, Action callback) {
        if (isListening) {
            isListening = false;
            direction = dir;
            this.owner = owner;
            isSpell = false;
            this.callback = callback;
        }
    }

    public bool Spell(Vector3 dir, string owner, string spell, Action callback) {
        if (Spellbook.GetSpell(spell).IsOnCooldown()) {
            return false;
        }
        if (isListening) {
            isListening = false;
            direction = dir;
            this.owner = owner;
            isSpell = true;
            this.spell = null;
            this.spellName = spell;
            this.callback = callback;
            return true;
        }
        return false;
    }

    public bool Spell(Vector3 dir, string owner, Spell spell, Action callback) {
        if (spell.IsOnCooldown()) {
            return false;
        }
        if (isListening) {
            isListening = false;
            direction = dir;
            this.owner = owner;
            isSpell = true;
            this.spell = spell;
            this.callback = callback;
            return true;
        }
        return false;
    }

    private void Fire() {
        if (isSpell) {
            if (spell) {
                spell.Cast(transform, direction, owner);
            } else {
                Spellbook.GetSpell(spellName).Cast(transform, direction, owner);
            }
        } else {
            GameObject newProj = MonoBehaviour.Instantiate(Projectile, transform.position + DISPLACEMENT + (IsMelee? MELEE_DISPLACEMENT : RANGED_DISPLACEMENT) * direction, Quaternion.identity);
            newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(direction.y, direction.x));
            newProj.tag = owner;
            newProj.GetComponent<Projectile>().Direction = direction * ProjectileScale;
            newProj.GetComponent<Projectile>().Damage = WeaponAttack;
            newProj.transform.localScale *= ProjectileScale;

            if (newProj.GetComponent<HomingProjectile>()) {
                newProj.GetComponent<HomingProjectile>().TagToHome = newProj.tag == "Enemy" ? "Player" : "Enemy";
            }
        }
        direction = Vector3.zero;
        callback.Invoke();
        isListening = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
