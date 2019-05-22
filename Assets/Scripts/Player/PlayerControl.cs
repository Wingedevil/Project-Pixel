using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    public bool UseMouseToAttack = true;

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
        if (UseMouseToAttack) {
            Vector3 target = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
            direction = Vector3.Normalize((Vector2)target - (Vector2)player.transform.position - new Vector2(0, -5f / 16f));
        } else {
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

            if (direction == Vector3.zero) {
                GameObject closest = null;
                float closestDistance = 4.0f;
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy")) {
                    if (enemy.GetComponent<Entity>()) {
                        if (Vector3.Distance(player.transform.position, enemy.transform.position) < closestDistance) {
                            closestDistance = Vector3.Distance(player.transform.position, enemy.transform.position);
                            closest = enemy;
                        }
                    }
                }
                if (closest) {
                    direction = Vector3.Normalize(closest.transform.position - player.transform.position);
                }
            }
        }

        return direction;
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKey) {
            if (Input.GetKeyDown(KeyCode.O)) {
                playerEntity.GetComponent<SavePlayer>().SaveFile();
            }
            if (Input.GetKeyDown(KeyCode.P)) {
                playerEntity.GetComponent<LoadPlayer>().LoadFile();
            }

            bool needToFlip = true;
            if (UseMouseToAttack && Input.GetKey(KeyCode.Mouse0)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
                playerEntity.Attack(shootingDir);
            } else if (!UseMouseToAttack && Input.GetKey(KeyCode.E)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
                playerEntity.Attack(shootingDir);
            } else {
                playerEntity.NotAttacking();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (playerEntity.Cast("Arcane Storm", shootingDir) && shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha2)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (playerEntity.Cast("Arcane Missiles", shootingDir) && shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha3)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (playerEntity.Cast("Arcane Blast", shootingDir) && shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha4)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (playerEntity.Cast("Arcane Eruption", shootingDir) && shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha5)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (playerEntity.Cast("Death and Decay", shootingDir) && shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha6)) {
                Vector3 shootingDir = FindShotDirection();
                needToFlip = false;
                if (playerEntity.Cast("Arcane Wrath", shootingDir) && shootingDir.x != 0) {
                    playerEntity.Flip(Math.Sign(shootingDir.x));
                }
            }

            if (Input.GetKeyDown(KeyCode.Space)) {
                playerEntity.Interact();
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
