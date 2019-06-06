using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Generator : MonoBehaviour {
    public const int MAX_THRESHOLD = 20;
    public static int RoomUUID = 0;

    public int MinRooms = 15;
    public int MaxRooms = 50;
    public float TimeToCheck = 0.2f;
    public GameObject StartingRoom;
    public GameObject SpawningChild;

    [HideInInspector]
    public int Rooms;

    private float TimeSinceLastSpawn;

    public void ModifyRooms() {
        TimeSinceLastSpawn = 0.0f;
    }

    // Start is called before the first frame update
    void Start() {
        Generate();
    }

    // Update is called once per frame
    void Update() {
        if (TimeSinceLastSpawn >= TimeToCheck) {
            Rooms = GameObject.FindObjectsOfType<PGC_Destroyer>().Length;
            if (Rooms < MinRooms || Rooms > MaxRooms) {
                RestartGeneration();
            } else {
                AstarPath.active.Scan();
                Destroy(this);
            }
        }
        TimeSinceLastSpawn += Time.deltaTime;
    }

    private void RestartGeneration() {
        Debug.Log(Rooms);
        foreach (Transform child in SpawningChild.transform) {
            Destroy(child.gameObject);
        }
        RoomUUID = 0;
        TimeSinceLastSpawn = 0.0f;
        Rooms = 0;
        Invoke("Generate", 0.01f);
    }

    private void Generate() {
        Instantiate(StartingRoom, SpawningChild.transform).GetComponentInChildren<PGC_Destroyer>().DontDestroy = true;
        Rooms = 1;
    }
}
