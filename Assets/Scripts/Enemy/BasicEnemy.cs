using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy {
    public float DistanceToPlayer;

    private Pathfinding.AIBase aiBase;

    // Start is called before the first frame update
    protected new void Start() {
        base.Start();
        this.GetComponent<Pathfinding.AIDestinationSetter>().target = Player.transform;
        this.GetComponent<Pathfinding.AIPath>().endReachedDistance = DistanceToPlayer;
        this.GetComponent<Pathfinding.AIPath>().maxSpeed = MoveSpeed;
        aiBase = this.GetComponent<Pathfinding.AIBase>();
    }

    // Update is called once per frame
    protected new void Update() {
        base.Update();

        Vector3 originalVector = Player.transform.position - transform.position;
        Vector3 directionToPlayer = Vector3.Normalize(originalVector);

        if (!aiBase.canSearch && Vector3.Magnitude(originalVector) <= ALIVE_DISTANCE) {
            aiBase.canSearch = true;
        }

        // Always face player, bypasses moonwalking check
        if (directionToPlayer.x > 0.01f) {
            transform.localScale = new Vector3(1, 1, 1);
        } else if (directionToPlayer.x < -0.01f) {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Vector3.Magnitude(aiBase.desiredVelocity) < 0.01f) {
            //this.GetComponent<Rigidbody2D>().velocity = Vector3.zero; 
            UpdateAnimators("Moving", false);
            if (CurrentAttackCooldown <= 0f) {
                Attack(directionToPlayer);
                CurrentAttackCooldown = AttackCooldown;
            }
        } else {
            //this.GetComponent<Rigidbody2D>().velocity = MoveSpeed * directionToPlayer;
            UpdateAnimators("Moving", Vector3.Magnitude(originalVector) > DistanceToPlayer);
        }
    }
}