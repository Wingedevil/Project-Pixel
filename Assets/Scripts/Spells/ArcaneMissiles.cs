using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneMissiles : Spell {
    private const int NUMBER_OF_PROJECTILES = 3;
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
            Vector3 newDir = Vector3.Normalize(dir + Random.insideUnitSphere * 1.0f);
            newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(newDir.y, newDir.x));
            newProj.GetComponent<Projectile>().Direction = Vector3.Normalize(newDir);
            newProj.GetComponent<Projectile>().Damage = PrimarySpellDamage;
            newProj.tag = tagger;
            if (newProj.GetComponent<HomingProjectile>()) {
                newProj.GetComponent<HomingProjectile>().TagToHome = newProj.tag == "Enemy" ? "Player" : "Enemy";
            }
            Invoke("SummonProjectile", INTERVAL_BETWEEN_PROJECTILES);
        }
    }
}
