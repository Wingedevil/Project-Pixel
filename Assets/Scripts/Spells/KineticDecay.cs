using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticDecay : HomingProjectile {
    public float SpeedMult = 0.25f;

    protected override void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Projectile>() && !collision.GetComponent<Projectile>().IsMelee) {
            collision.GetComponent<Projectile>().Speed *= SpeedMult;
            collision.GetComponent<Rigidbody2D>().velocity *= SpeedMult;
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Projectile>() && !collision.GetComponent<Projectile>().IsMelee) {
            collision.GetComponent<Projectile>().Speed /= SpeedMult;
            collision.GetComponent<Rigidbody2D>().velocity /= SpeedMult;
        }
    }

    protected override void OnTriggerStay2D(Collider2D other) {

    }
}
