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

    private float time = 0.0f;

    // Start is called before the first frame update
    void Start() {
        time = Interval + Random.Range(RandomMin, RandomMax);
    }

    // Update is called once per frame
    void Update() {
        if (Enemies.Length <= 0) {
            return;
        }
        if (time <= 0) {
            time = Interval + Random.Range(RandomMin, RandomMax);
            SpawnEnemy();
            return;
        }
        time -= Time.deltaTime;
    }

    private void SpawnEnemy() {
        GameObject go = Instantiate(Enemies[Random.Range(0, Enemies.Length)], this.transform.position, Quaternion.identity);
        go.GetComponent<Pathfinding.AIBase>().canSearch = true;
        GameObject ps = Instantiate(Poof, go.transform.position, Quaternion.identity);
        ParticleSystem.MainModule psMain = ps.GetComponent<ParticleSystem>().main;
        psMain.startColor = Color.white;

        if (SpawnHealth) {
            go.GetComponent<Enemy>().Drops.Add(HealthPotion);
            go.GetComponent<Enemy>().DropChances.Add(HealthPotionDropRate);
        }
    }
}