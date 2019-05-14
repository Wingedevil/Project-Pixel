using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public GameObject testPrefab;

    GameObject player;
    PlayerEntity playerEntity;

    const float GLOBAL_SPEED_MULT = 1f;

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
        playerEntity = player.GetComponent<PlayerEntity>();
    }
    
    private Vector3 FindShotDirection() {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.UpArrow)) {
            direction += new Vector3(0, 1, 0);
        }
        if (Input.GetKey(KeyCode.DownArrow)) {
            direction += new Vector3(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.LeftArrow)) {
            direction += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.RightArrow)) {
            direction += new Vector3(1, 0, 0);
        }

        return direction;
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKey) {
            bool needToFlip = true;
            if (Input.GetKey(KeyCode.E)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
                playerEntity.Attack(shootingDir);
            } else {
                playerEntity.NotAttacking();
            }

            if (Input.GetKeyDown(KeyCode.Q)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
                playerEntity.Cast("Arcane Bolt", shootingDir);
            }
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                playerEntity.TryToPickUp();
            }

            //debug tools
            if (Input.GetKeyDown(KeyCode.C)) {
                PlayerStats.InventoryAddItem(testPrefab);
            }

            /*
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                playerEntity.UpdateAnimators("2H", false);
                playerEntity.UpdateAnimators("Bow", false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                playerEntity.UpdateAnimators("2H", false);
                playerEntity.UpdateAnimators("Bow", false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                playerEntity.UpdateAnimators("2H", true);
                playerEntity.UpdateAnimators("Bow", false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                playerEntity.UpdateAnimators("2H", false);
                playerEntity.UpdateAnimators("Bow", true);
            }
            */
            //end debug tools

            Vector3 dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W)) {
                dir += new Vector3(0, 1, 0);
            }
            if (Input.GetKey(KeyCode.S)) {
                dir += new Vector3(0, -1, 0);
            }
            if (Input.GetKey(KeyCode.A)) {
                dir += new Vector3(-1, 0, 0);
            }
            if (Input.GetKey(KeyCode.D)) {
                dir += new Vector3(1, 0, 0);
            }
            if (needToFlip) {
                if (dir.x > 0.01f) {
                    playerEntity.Flip(1);
                } else if (dir.x < -0.01f) {
                    playerEntity.Flip(-1);
                }
            }
            playerEntity.UpdateAnimators("Moving", dir != Vector3.zero);
            player.GetComponent<Rigidbody2D>().velocity = Vector3.Normalize(dir) * GLOBAL_SPEED_MULT * player.GetComponent<Entity>().MoveSpeed;
        } else {
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            playerEntity.UpdateAnimators("Moving", false);
            playerEntity.UpdateAnimators("Attack", false);
        }
    }
}
