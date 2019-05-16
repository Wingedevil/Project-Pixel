using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackProjectile : Projectile {
    public float KnockbackDistance = 0.3f;

    protected override void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag != this.gameObject.tag && collidedTime >= HIT_TIME) {
            if (other.gameObject.tag == "Wall") {
                if (other.OverlapPoint(transform.position)) {
                    Destroy(this.gameObject);
                }
                return;
            }
            try {
                other.gameObject.GetComponent<Entity>().TakeDamage(new DamageMetadata(Damage, IsPhysical, IsMagical));
                other.GetComponent<Rigidbody2D>().MovePosition(
                    other.transform.position
                    + Vector3.Normalize(Direction) * KnockbackDistance
                    );
                Destroy(this.gameObject);
            } catch (NullReferenceException) {
                //I dont care about the other object
            }
        }
        collidedTime += Time.deltaTime;
    }
}