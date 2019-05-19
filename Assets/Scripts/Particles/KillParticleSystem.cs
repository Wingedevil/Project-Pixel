using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillParticleSystem : MonoBehaviour {
    private ParticleSystem ps;

    // Start is called before the first frame update
    void Start() {
        if (!PlayerPrefs.EnableParticles) {
            Destroy(this.gameObject);
        }
        ps = this.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update() {
        if (!ps.IsAlive()) {
            Destroy(ps.gameObject);
        }
    }
}