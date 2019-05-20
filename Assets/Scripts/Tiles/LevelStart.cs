using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStart : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        GameObject.FindGameObjectsWithTag("Player")[0].transform.position = this.transform.position;
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update() {

    }
}
