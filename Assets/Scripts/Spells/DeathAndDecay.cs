using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathAndDecay : Spell {
    public const int NUMBER_OF_PROJECTILES = 24;

    public override void Cast(Transform tf, Vector3 dir, string tag) {
        if (CurrentCooldown > 0) {
            return;
        }
        CurrentCooldown = Cooldown;
        for (int i = 0; i < NUMBER_OF_PROJECTILES; i++) {
            GameObject go = Instantiate(PrimarySpellProjectile, tf.transform.position + DISPLACEMENT, Quaternion.identity);
            go.GetComponent<Projectile>().Direction = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * i / NUMBER_OF_PROJECTILES * 360.0f),
                Mathf.Cos(Mathf.Deg2Rad * i / NUMBER_OF_PROJECTILES * 360.0f),
                0
            );
            go.transform.Rotate(0, 0, -1.0f * i / NUMBER_OF_PROJECTILES * 360.0f + 90);
            go.GetComponent<Projectile>().Damage = GetDamage();
            go.tag = tag;
        }
    }

    protected override void Update() {
        base.Update();
        HPCost = (int)Mathf.Round(this.GetComponentInParent<Entity>().curHP * 0.5f);
        MPCost = (int)Mathf.Round(this.GetComponentInParent<Entity>().curMP * 0.5f);
    }
}