﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : Entity {
    public const float FORCE_MAGNITUDE = 3.0f;
    public const float ALIVE_DISTANCE = 5.0f;

    public float RejectionRadius = 1.0f;
    public float AttackCooldown = 5.0f;
    public GameObject Player;
    public GameObject[] Drops;
    public float[] DropChances;

    protected float CurrentAttackCooldown;

    protected override void Die() {

        base.Die();
    }

    // Start is called before the first frame update
    protected new void Start() {
        base.Start();
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        CurrentAttackCooldown = AttackCooldown;
        foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>()) {
            sr.sortingOrder = -2;
        }
    }

    // Update is called once per frame
    protected new void Update() {
        base.Update();
        CurrentAttackCooldown -= CurrentAttackCooldown <= 0 ? 0 : Time.deltaTime;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
            Vector3 forceDirection = go.transform.position - this.transform.position;
            float distance = Vector3.Magnitude(forceDirection);
            if (distance <= RejectionRadius) {
                if (go.GetComponent<Entity>() is null || ReferenceEquals(go, this.gameObject)) {
                } else {
                    go.GetComponent<Rigidbody2D>().AddForce((Vector2)forceDirection * FORCE_MAGNITUDE / distance * distance);
                }
            }
        }
    }
}