using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
    public GameObject HealthPotion;
    public float HealthPotionDropRate = 0.1f;
    public GameObject Poof;
    public GameObject[] Enemies;
    public bool SpawnHealth;
    public float Interval = 5.0f;
    public float RandomMin = -2.0f;
    public float RandomMax = 2.0f;
    public int EnemiesPerInterval = 1;

    protected float time = 0.0f;

    // Start is called before the first frame update
    void Start() {
        time = Interval + Random.Range(RandomMin, RandomMax);
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (Enemies.Length <= 0) {
            return;
        }
        if (time <= 0) {
            time = Interval + Random.Range(RandomMin, RandomMax);
            SpawnEnemy(Vector3.zero);
            return;
        }
        time -= Time.deltaTime;
    }

    protected void SpawnEnemy(Vector3 delta, bool alert = true) {
        Vector3 spawnCoord = this.transform.position + delta;
        GameObject go = Instantiate(Enemies[Random.Range(0, Enemies.Length)], spawnCoord, Quaternion.identity);
        go.GetComponent<Pathfinding.AIBase>().canSearch = alert;
        GameObject ps = Instantiate(Poof, spawnCoord, Quaternion.identity);
        ParticleSystem.MainModule psMain = ps.GetComponent<ParticleSystem>().main;
        psMain.startColor = Color.white;

        if (SpawnHealth) {
            go.GetComponent<Enemy>().Drops.Add(HealthPotion);
            go.GetComponent<Enemy>().DropChances.Add(HealthPotionDropRate);
        }
    }
}