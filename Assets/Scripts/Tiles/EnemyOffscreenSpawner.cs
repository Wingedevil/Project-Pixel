using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Collider2D))]
public class EnemyOffscreenSpawner : EnemySpawner {
    public float DistanceFromPlayer = 10;
    public int MaxSpawns = 5;
    public GameObject Player;

    private int spawnsInCollider = 0;

    // Start is called before the first frame update
    void Start() {
        time = Interval + Random.Range(RandomMin, RandomMax);
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    protected override void Update() {
        if (Enemies.Length <= 0) {
            return;
        }
        if (time <= 0) {
            if (Vector3.Distance(Player.transform.position, this.transform.position) >= DistanceFromPlayer && spawnsInCollider < MaxSpawns) {
                time = Interval + Random.Range(RandomMin, RandomMax);
                SpawnEnemy(Random.insideUnitCircle, false);
                return;
            }
        } else {
            time -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            spawnsInCollider++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Enemy") {
            spawnsInCollider--;
        }
    }
}
