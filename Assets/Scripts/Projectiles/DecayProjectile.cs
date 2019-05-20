using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecayProjectile : PiercingProjectile {
    protected override void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag != this.gameObject.tag) {
            if (other.gameObject.tag == "Wall") {
                return;
            }
            try {
                other.gameObject.GetComponent<Entity>().SetHPTo1();
            } catch (NullReferenceException) {
                //I dont care about the other object
            }
        }
    }

    protected override void OnTriggerStay2D(Collider2D collision) {
        base.OnTriggerStay2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision) {
        base.OnTriggerExit2D(collision);
    }
}