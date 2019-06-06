using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyAmbushSpawner : MonoBehaviour {
    public GameObject HealthPotion;
    public float HealthPotionDropRate = 0.1f;
    public GameObject Poof;
    public float SpawnDelay;
    public GameObject[] Enemies;

    [HideInInspector]
    public GameObject Player;

    // Start is called before the first frame update
    void Start() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PlayerEntity>()) {
            Invoke("SpawnAmbush", SpawnDelay);
        }
    }

    void SpawnAmbush() {
        foreach (GameObject go in Enemies) {
            SpawnEnemy(Random.insideUnitCircle);
        }
        Destroy(this);
    }

    protected void SpawnEnemy(Vector3 delta, bool alert = true) {
        Vector3 spawnCoord = this.transform.position + delta;
        GameObject go = Instantiate(Enemies[Random.Range(0, Enemies.Length)], spawnCoord, Quaternion.identity);
        go.GetComponent<Pathfinding.AIBase>().canSearch = alert;
        GameObject ps = Instantiate(Poof, spawnCoord, Quaternion.identity);
        ParticleSystem.MainModule psMain = ps.GetComponent<ParticleSystem>().main;
        psMain.startColor = Color.white;
    }
}