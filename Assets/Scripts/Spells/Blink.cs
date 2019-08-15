using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink : WeaponSpell {
    public override void Cast(Transform tf, Vector3 dir, string tag) {
        if (CurrentCooldown > 0) {
            return;
        }
        CurrentCooldown = Cooldown;
        RaycastHit2D results = Physics2D.Raycast(tf.position + DISPLACEMENT, dir, 5.0f, LayerMask.NameToLayer("Player"));
        Vector3 dest = results.collider ? (tf.position + DISPLACEMENT + dir * results.distance * 0.9f) : (tf.position + DISPLACEMENT + dir * 5.0f);
        GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().MovePosition(dest);
    }
}
