using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PGC_Spawner : MonoBehaviour {
    public enum Direction {
        Up,
        Down,
        Left,
        Right
    }
    public static Dictionary<Direction, string> DirectionToAnchor = new Dictionary<Direction, string>() {
        {Direction.Up, "Anchor W"},
        {Direction.Left, "Anchor A"},
        {Direction.Down, "Anchor S"},
        {Direction.Right, "Anchor D"}
    };

    [System.Serializable]
    public struct TileTuple{
        public float Chance;
        public string[] SpawnableTiles;
    }

    public Direction DirectionToSpawn;
    public bool Spawned;
    public string DirToGDC;
    public TileTuple[] SpawnableTiles;
    public GameObject CapTile;
    [HideInInspector]
    public bool Destroying;

    // Start is called before the first frame update
    void Start() {
        Invoke("Spawn", 0.1f);
    }

    // Update is called once per frame
    void Update() {

    }

    public Direction Opposite(Direction dir) {
        switch (dir) {
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
        }
        throw new System.Exception("WTF");
    }

    void Spawn() {
        if (!Spawned) {
            this.transform.root.GetComponent<PGC_Generator>().IncreaseRoom();
            float i;
            int counter;
            do {
                counter = 0;
                i = Random.Range(0f, 1f);
                while (i > 0) {
                    if (counter >= SpawnableTiles.Length) {
                        break;
                    }
                    i -= SpawnableTiles[counter++].Chance;
                }
                if (counter >= SpawnableTiles.Length) {
                    continue;
                }
            } while (SpawnableTiles[--counter].SpawnableTiles.Length == 0);
            GameObject go = Instantiate(
                Resources.Load<GameObject>(
                    DirToGDC 
                    + SpawnableTiles[counter].SpawnableTiles[Random.Range(0, SpawnableTiles[counter].SpawnableTiles.Length)]
                ),
                this.transform
            );
            go.transform.localPosition = go.transform.Find(DirectionToAnchor[DirectionToSpawn]).localPosition * -1;
            Spawned = true;
        }
    }

    public void SpawnCap() {
        GameObject go = Instantiate(CapTile, this.transform);
        go.transform.localPosition = go.transform.Find(DirectionToAnchor[DirectionToSpawn]).localPosition * -1;
        go.GetComponentInChildren<TilemapRenderer>().sortingOrder = -9;
        Spawned = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (!Spawned) {
            if (collision.GetComponent<PGC_Spawner>()) {
                return;
            }
            Destroying = true;
            foreach (Transform child in this.transform) {
                Destroy(child.gameObject);
            }
            // should i spawn a cap?
            if (collision.GetComponent<TilemapCollider2D>()) {
                SpawnCap();
            }
            Spawned = true;
        }
    }
}
