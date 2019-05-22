using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneWrath : WeaponSpell {
    public const int NUMBER_OF_PROJECTILES = 3;

    public override void Cast(Transform tf, Vector3 dir, string tag) {
        if (CurrentCooldown > 0) {
            return;
        }
        CurrentCooldown = Cooldown;
        for (int i = 0; i < NUMBER_OF_PROJECTILES; i++) {
            Vector3 newDir = Vector3.Normalize(dir + Random.onUnitSphere * 0.25f);
            GameObject newProj = Instantiate(PrimarySpellProjectile, tf.position + DISPLACEMENT, Quaternion.identity);
            newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(newDir.y, newDir.x));
            newProj.GetComponent<Projectile>().Direction = newDir;
            newProj.GetComponent<Projectile>().Damage = GetDamage();
            newProj.tag = tag;
        }
    }
}
