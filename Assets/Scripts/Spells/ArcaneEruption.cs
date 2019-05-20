using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneEruption : WeaponSpell {
    private const int NUMBER_OF_PRIMARY_PROJECTILES = 6;
    private const int NUMBER_OF_SECONDARY_PROJECTILES = 24;
    public int SecondarySpellDamage;
    public float SecondaryWeaponPercentDamage;
    public GameObject SecondarySpellProjectile;
    public float SecondaryProjectileLifeTime;

    public override void Cast(Transform tf, Vector3 dir, string tag) {
        if (CurrentCooldown > 0) {
            return;
        }
        CurrentCooldown = Cooldown;
        for (int i = 0; i < NUMBER_OF_PRIMARY_PROJECTILES; i++) {
            GameObject newProj = Instantiate(PrimarySpellProjectile, tf.position + DISPLACEMENT, Quaternion.identity);
            Vector3 newDir = Vector3.Normalize(dir + Random.insideUnitSphere * 1.0f);
            newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(newDir.y, newDir.x));
            newProj.GetComponent<Projectile>().Direction = Vector3.Normalize(newDir);
            newProj.GetComponent<Projectile>().Damage = GetDamage();
            newProj.tag = tag;
            if (newProj.GetComponent<HomingProjectile>()) {
                newProj.GetComponent<HomingProjectile>().TagToHome = newProj.tag == "Enemy" ? "Player" : "Enemy";
            }
        }

        for (int i = 0; i < NUMBER_OF_SECONDARY_PROJECTILES; i++) {
            GameObject go = Instantiate(SecondarySpellProjectile, tf.transform.position + DISPLACEMENT, Quaternion.identity);
            go.GetComponent<Projectile>().Direction = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * i / NUMBER_OF_SECONDARY_PROJECTILES * 360.0f),
                Mathf.Cos(Mathf.Deg2Rad * i / NUMBER_OF_SECONDARY_PROJECTILES * 360.0f),
                0
            );
            go.transform.Rotate(0, 0, -1.0f * i / NUMBER_OF_SECONDARY_PROJECTILES * 360.0f + 90);
            go.GetComponent<Projectile>().Damage = GetSecondaryDamage();
            go.GetComponent<Projectile>().lifeTime = SecondaryProjectileLifeTime;
            go.tag = tag;
        }
    }

    public int GetSecondaryDamage() {
        return (int)Mathf.Round(this.GetComponentInParent<Entity>().SpellDamageAmp * (SecondarySpellDamage + SecondaryWeaponPercentDamage * GetWeaponDamage()));
    }

    protected override void Update() {
        base.Update();
        HPCost = (int)Mathf.Round(this.GetComponentInParent<Entity>().curHP * 0.1f);
    }
}
