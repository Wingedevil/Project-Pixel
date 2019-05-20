using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPStream : MonoBehaviour {
    public float MP_REGEN = 10.0f;

    private PlayerEntity playerEntity;
    private int restoring;

    // Start is called before the first frame update
    void Start() {
        playerEntity = this.GetComponentInParent<PlayerEntity>();
        Resources.FindObjectsOfTypeAll<MPStreamIdentifier>()[0].gameObject.SetActive(true);
        restoring = 0;
    }

    // Update is called once per frame
    void Update() {
        if (restoring > 0) {
            playerEntity.ChangeMP(MP_REGEN * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.tag == "MP Stream") {
            restoring++;
        }
    }
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.tag == "MP Stream") {
            restoring--;
        }
    }
}