using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneStorm : Spell {
    private const int NUMBER_OF_PROJECTILES = 25;
    private int numberOfProjectiles;
    private const float INTERVAL_BETWEEN_PROJECTILES = 0.02f;
    private Transform tf;
    private Vector3 dir;
    private string tagger;

    public override void Cast(Transform tf, Vector3 dir, string tagger) {
        if (CurrentCooldown > 0) {
            return;
        }
        CurrentCooldown = Cooldown;
        numberOfProjectiles = NUMBER_OF_PROJECTILES;
        this.tf = tf;
        this.dir = dir;
        this.tagger = tagger;
        SummonProjectile();
    }

    private void SummonProjectile() {
        if (numberOfProjectiles-- > 0) {
            GameObject newProj = Instantiate(PrimarySpellProjectile, tf.position + DISPLACEMENT, Quaternion.identity);
            Vector3 newDir = Vector3.Normalize(dir + Random.insideUnitSphere * 0.2f);
            newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(newDir.y, newDir.x));
            newProj.GetComponent<Projectile>().Direction = Vector3.Normalize(newDir + Random.insideUnitSphere * 0.2f);
            newProj.GetComponent<Projectile>().Damage = PrimarySpellDamage;
            newProj.tag = tagger;
            Invoke("SummonProjectile", INTERVAL_BETWEEN_PROJECTILES);
        }
    }
}
