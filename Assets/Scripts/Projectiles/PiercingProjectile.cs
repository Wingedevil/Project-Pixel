using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiercingProjectile : Projectile {
    public new const float HIT_TIME = 0.05f;

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != this.gameObject.tag) {
            if (other.gameObject.tag == "Wall") {
                return;
            }
            try {
                other.gameObject.GetComponent<Entity>().TakeDamage(new DamageMetadata(Damage, IsPhysical, IsMagical));
            } catch (NullReferenceException) {
                //I dont care about the other object
            }
        }
    }
    protected override void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag != this.gameObject.tag && collidedTime >= HIT_TIME) {
            if (other.gameObject.tag == "Wall") {
                if (other.OverlapPoint(transform.position)) {
                    this.Kill();
                }
                return;
            }
        }
        collidedTime += Time.deltaTime;
    }
    protected override void OnTriggerExit2D(Collider2D collision) {
        base.OnTriggerExit2D(collision);
    }
}
