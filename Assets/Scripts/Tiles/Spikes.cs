using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    public GameObject Reticule;
    public GameObject[] Prereqs;

    // Start is called before the first frame update
    void Start() {
        foreach (GameObject go in Prereqs) {
            Instantiate(Reticule, go.transform);
        }
    }

    // Update is called once per frame
    void Update() {
        for (int i = 0; i < Prereqs.Length; i++) {
            if (Prereqs[i] != null) {
                return;
            }
        }
        Destroy(this.gameObject);
    }
}