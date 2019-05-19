using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingFury : Spell {
    public override void Cast(Transform tf, Vector3 dir, string tag){
        if (CurrentCooldown > 0) {
            return;
        }
        CurrentCooldown = Cooldown;
        GameObject newProj = Instantiate(PrimarySpellProjectile, tf.position + DISPLACEMENT, Quaternion.identity);
        Vector3 newDir = Vector3.Normalize(dir);
        newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(newDir.y, newDir.x));
        newProj.GetComponent<Projectile>().Direction = Vector3.Normalize(newDir);
        newProj.GetComponent<Projectile>().Damage = PrimarySpellDamage;
        newProj.tag = tag;
        if (newProj.GetComponent<HomingProjectile>()) {
            newProj.GetComponent<HomingProjectile>().TagToHome = newProj.tag == "Enemy" ? "Player" : "Enemy";
        }
    }
}