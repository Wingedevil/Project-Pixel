using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour {
    public GameObject popupText;
    public GameObject Reticule;
    public GameObject[] Prereqs;
    public float PrereqTime = 0.0f;

    private float time = 0.0f;

    private bool display = false;
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (time < PrereqTime) {
            time += Time.deltaTime;
            return;
        }
        for (int i = 0; i < Prereqs.Length; i++) {
            if (Prereqs[i] != null) {
                return;
            }
        }
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (PrereqTime > 0) {
            GameObject newTextA = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
            newTextA.GetComponent<PopupText>().FeedText("" + (int)Mathf.Round(PrereqTime - time), 30, this.transform.position, Color.yellow);
        }
        if (display) {
            return;
        }
        display = true;
        foreach (GameObject go in Prereqs) {
            if (go != null) {
                Instantiate(Reticule, go.transform);
            }
        }
    }
}