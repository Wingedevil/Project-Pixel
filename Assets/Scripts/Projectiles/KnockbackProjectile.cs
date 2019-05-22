using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackProjectile : Projectile {
    public float KnockbackDistance = 0.3f;

    private List<GameObject> other = new List<GameObject>();

    protected override void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag != this.gameObject.tag && collidedTime >= HIT_TIME) {
            if (other.gameObject.tag == "Wall") {
                if (other.OverlapPoint(transform.position)) {
                    this.Kill();
                }
                return;
            }
            try {
                other.gameObject.GetComponent<Entity>().TakeDamage(new DamageMetadata(Damage, IsPhysical, IsMagical));
                if (other.gameObject.GetComponent<AIBase>()) {
                    other.gameObject.GetComponent<AIBase>().enabled = false;
                    this.other.Add(other.gameObject);
                    Invoke("Unstun", 0.02f);
                }
                other.GetComponent<Rigidbody2D>().MovePosition(
                    other.transform.position
                    + Vector3.Normalize(Direction) * KnockbackDistance
                    );
                Invoke("Kill", 5.0f);
            } catch (NullReferenceException) {
                //I dont care about the other object
            }
        }
        collidedTime += Time.deltaTime;
    }

    protected void Unstun() {
        foreach (GameObject other in this.other) {
            try {
                this.gameObject.SetActive(false);
                other.gameObject.GetComponent<AIBase>().enabled = true;
            } catch (MissingReferenceException) {
                //I dont care about the other object
            }
        }
    }
}