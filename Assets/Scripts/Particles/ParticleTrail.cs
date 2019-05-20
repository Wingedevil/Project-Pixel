using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTrail : MonoBehaviour {
    public Color StartColor;
    public Color EndColor;
    public GameObject Trail;
    public float ParticleLifeTime = 0.25f;

    private GameObject following;

    // Start is called before the first frame update
    void Start() {
        if (!PlayerPrefs.EnableParticles) {
            Destroy(this.gameObject);
        }
        following = Instantiate(Trail, this.transform.position, Quaternion.identity);
        ParticleSystem.MainModule psMain = following.GetComponent<ParticleSystem>().main;
        psMain.startColor = new ParticleSystem.MinMaxGradient(
            new Color(StartColor.r, StartColor.g, StartColor.b),
            new Color(EndColor.r, EndColor.g, EndColor.b)
            );
        psMain.startLifetime = ParticleLifeTime;
    }

    // Update is called once per frame
    void Update() {
        following.transform.position = this.transform.position;
    }

    void OnDestroy() {
        try {
            following.GetComponent<ParticleSystem>().Stop();
        } catch (MissingReferenceException) {

        }
    }
}