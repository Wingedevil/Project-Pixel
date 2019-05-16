using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBolt : Spell {
    public override void Cast(Transform tf, Vector3 dir, string tag) {
        if (CurrentCooldown > 0) {
            return;
        }
        CurrentCooldown = Cooldown;
        GameObject newProj = Instantiate(PrimarySpellProjectile, tf.position + DISPLACEMENT, Quaternion.identity);
        newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x));
        newProj.GetComponent<Projectile>().Direction = dir;
        newProj.GetComponent<Projectile>().Damage = PrimarySpellDamage;
        newProj.tag = tag;
    }
}
