using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingProjectile : Projectile {
    public int Heal;
    public bool Pierce = true;

    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == this.gameObject.tag) {
            try {
                other.gameObject.GetComponent<Entity>().Heal(new DamageMetadata(Heal, false, false));
                if (!Pierce) {
                    Destroy(this.gameObject);
                }
            } catch (NullReferenceException) {
                //I dont care about the other object
            }
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision) {
        
    }
}