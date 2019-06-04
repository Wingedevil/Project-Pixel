using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Generator : MonoBehaviour {
    public const int MAX_THRESHOLD = 5;

    public int MinRooms = 12;
    public int MaxRooms = 20;
    public float TimeToCheck = 0.5f;
    public GameObject StartingRoom;
    public GameObject SpawningChild;

    [HideInInspector]
    public int Rooms;

    private float TimeSinceLastSpawn;

    // Start is called before the first frame update
    void Start() {
        Generate();
    }

    // Update is called once per frame
    void Update() {
        if (TimeSinceLastSpawn >= TimeToCheck) {
            if (Rooms < MinRooms || Rooms > MaxRooms) {
                RestartGeneration();
            } else {
                AstarPath.active.Scan();
                Destroy(this);
            }
        }
        TimeSinceLastSpawn += Time.deltaTime;
    }

    public void DecreaseRoom() {
        Rooms--;
        Debug.Log(Rooms);
        TimeSinceLastSpawn = 0.0f;
    }

    public void IncreaseRoom() {
        Rooms++;
        if (Rooms > MaxRooms + MAX_THRESHOLD) {
            RestartGeneration();
        }
        Debug.Log(Rooms);
        TimeSinceLastSpawn = 0.0f;
    }

    private void RestartGeneration() {
        foreach (Transform child in SpawningChild.transform) {
            Destroy(child.gameObject);
        }
        TimeSinceLastSpawn = 0.0f;
        Rooms = 0;
        Invoke("Generate", 0.1f);
    }

    private void Generate() {
        Instantiate(StartingRoom, SpawningChild.transform);
        Rooms = 1;
    }
}
