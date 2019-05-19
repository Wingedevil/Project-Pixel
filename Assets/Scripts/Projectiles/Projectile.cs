using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
    public const float HIT_TIME = 0.02f;
    
    public bool IsMelee;
    public bool IsPhysical;
    public bool IsMagical;
    public int Damage;
    public float lifeTime;
    public float Speed;
    public Vector3 Direction;
    public GameObject DieParticleSystem;
    public Color DieParticleColor;

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
            Kill();
        }
        age += Time.deltaTime;
    }

    public void Kill() {
        if (IsMelee) {
            KillWithoutExplosion();
            return;
        }
        try {
            ParticleSystem ps = Instantiate(DieParticleSystem, this.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
            ParticleSystem.MainModule psMain = ps.main;
            psMain.startColor = new Color(DieParticleColor.r, DieParticleColor.g, DieParticleColor.b);
        } catch (UnassignedReferenceException) {

        }
        Destroy(this.gameObject);
    }

    public void KillWithoutExplosion() {
        Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {

    }

    protected virtual void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag != this.gameObject.tag && collidedTime >= HIT_TIME) {
            if (other.gameObject.tag == "Wall") {
                if (other.OverlapPoint(transform.position)) {
                    Kill();
                }
                return;
            }
            try {
                other.gameObject.GetComponent<Entity>().TakeDamage(new DamageMetadata(Damage, IsPhysical, IsMagical));
                Kill();
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