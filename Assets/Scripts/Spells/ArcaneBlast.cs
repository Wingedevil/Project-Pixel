using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBlast : WeaponSpell {
    private const int NUMBER_OF_SECONDARY_PROJECTILES = 12;
    public int SecondarySpellDamage;
    public float SecondaryWeaponPercentDamage;
    public GameObject SecondarySpellProjectile;

    public override void Cast(Transform tf, Vector3 dir, string tag) {
        if (CurrentCooldown > 0) {
            return;
        }
        CurrentCooldown = Cooldown;
        GameObject newProj = Instantiate(PrimarySpellProjectile, tf.position + DISPLACEMENT, Quaternion.identity);
        newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x));
        newProj.GetComponent<Projectile>().Direction = dir;
        newProj.GetComponent<Projectile>().Damage = GetDamage();
        newProj.tag = tag;
        newProj.GetComponent<ExplodingProjectile>().SecondaryDamage = GetSecondaryDamage();
        newProj.GetComponent<ExplodingProjectile>().SecondaryProjectile = SecondarySpellProjectile;
    }

    public int GetSecondaryDamage() {
        return (int)Mathf.Round(this.GetComponentInParent<Entity>().SpellDamageAmp * (SecondarySpellDamage + SecondaryWeaponPercentDamage * GetWeaponDamage()));
    }
}
