using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy {
    public float MoveThreshold = 0.5f;
    public float AlertDistance = 2.0f;
    public float DistanceToPlayer;
    
    private Pathfinding.AIBase aiBase;

    // Start is called before the first frame update
    protected new void Start() {
        base.Start();
        this.GetComponent<Pathfinding.AIDestinationSetter>().target = Player.transform;
        this.GetComponent<Pathfinding.AIPath>().endReachedDistance = MoveThreshold;
        this.GetComponent<Pathfinding.AIPath>().maxSpeed = MoveSpeed;
        aiBase = this.GetComponent<Pathfinding.AIBase>();
    }

    // Update is called once per frame
    protected new void Update() {
        base.Update();

        Vector3 originalVector = Player.transform.position - transform.position;
        Vector3 directionToPlayer = Vector3.Normalize(originalVector);

        if (!aiBase.canSearch && Vector3.Magnitude(originalVector) <= ActivateDistance) {
            Alert();
        }

        // Always face player, bypasses moonwalking check
        if (directionToPlayer.x > 0.01f) {
            transform.localScale = new Vector3(1, 1, 1);
        } else if (directionToPlayer.x < -0.01f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (CurrentAttackCooldown <= 0f && Vector3.Magnitude(Player.transform.position - this.transform.position) <= DistanceToPlayer) {
            Attack(directionToPlayer);
            CurrentAttackCooldown = AttackCooldown;
        }

        if (Vector3.Magnitude(aiBase.desiredVelocity) < 0.01f) {
            //this.GetComponent<Rigidbody2D>().velocity = Vector3.zero; 
            UpdateAnimators("Moving", false);
        } else {
            //this.GetComponent<Rigidbody2D>().velocity = MoveSpeed * directionToPlayer;
            UpdateAnimators("Moving", Vector3.Magnitude(originalVector) > DistanceToPlayer);
        }
    }

    protected void AlertNearby() {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Enemy")) {
            Vector3 vector = go.transform.position - this.transform.position;
            float distance = Vector3.Magnitude(vector);
            if (distance <= AlertDistance) {
                go.GetComponent<BasicEnemy>().Alert();
            }
        }
    }

    public override void TakeDamage(DamageMetadata meta) {
        Alert();
        base.TakeDamage(meta);
    }

    public void Alert() {
        if (!aiBase.canSearch) {
            aiBase.canSearch = true;
            AlertNearby();
        }
    }
}