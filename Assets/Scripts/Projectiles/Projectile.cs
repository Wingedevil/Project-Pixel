﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public const float HIT_TIME = 0.02f;

    public bool IsPhysical;
    public bool IsMagical;
    public int Damage;
    public float lifeTime;
    public float Speed;
    public Vector3 Direction;

    protected float age;
    protected float collidedTime;

    // Start is called before the first frame update
    protected virtual void Start() {
        age = 0f;
        this.GetComponent<Rigidbody2D>().velocity += (Vector2)Vector3.Normalize(Direction) * Speed;
        this.GetComponent<SpriteRenderer>().sortingOrder = 10;
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (age >= lifeTime) {
            Destroy(gameObject);
        }
        age += Time.deltaTime;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {

    }

    protected virtual void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag != this.gameObject.tag && collidedTime >= HIT_TIME) {
            if (other.gameObject.tag == "Wall") {
                if (other.OverlapPoint(transform.position)) {
                    Destroy(this.gameObject);
                }
                return;
            }
            try {
                other.gameObject.GetComponent<Entity>().TakeDamage(new DamageMetadata(Damage, IsPhysical, IsMagical));
                Destroy(this.gameObject);
            } catch (NullReferenceException) {
                //I dont care about the other object
            }
        }
        collidedTime += Time.deltaTime;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) {
        collidedTime = 0f;
    }
}