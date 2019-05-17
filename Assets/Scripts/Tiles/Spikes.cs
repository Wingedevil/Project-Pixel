using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    public GameObject Reticule;
    public GameObject[] Prereqs;

    private bool display = false;
    // Start is called before the first frame update
    void Start() {

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

    private void OnCollisionEnter2D(Collision2D collision) {
        if (display) {
            return;
        }
        foreach (GameObject go in Prereqs) {
            if (go != null) {
                Instantiate(Reticule, go.transform);
            }
        }
    }
}