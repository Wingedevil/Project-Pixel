using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingProjectile : Projectile {
    public int NumberOfSecondaryProjectiles = 12;
    public float SecondaryProjectileLifeTime = 1.0f;
    public int SecondaryDamage;
    public GameObject SecondaryProjectile;

    private void OnDestroy() {
        for (int i = 0; i < NumberOfSecondaryProjectiles; i++) {
            GameObject go = Instantiate(SecondaryProjectile, this.transform.position, Quaternion.identity);
            go.GetComponent<Projectile>().Direction = new Vector3(
                Mathf.Sin(Mathf.Deg2Rad * i / NumberOfSecondaryProjectiles * 360.0f), 
                Mathf.Cos(Mathf.Deg2Rad * i / NumberOfSecondaryProjectiles * 360.0f),
                0
            );
            go.transform.Rotate(0, 0, -1.0f * i / NumberOfSecondaryProjectiles * 360.0f + 90);
            go.GetComponent<Projectile>().Damage = SecondaryDamage;
            go.GetComponent<Projectile>().lifeTime = SecondaryProjectileLifeTime;
            go.tag = this.tag;
        }
    }
}