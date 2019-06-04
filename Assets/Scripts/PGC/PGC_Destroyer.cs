using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Destroyer : MonoBehaviour {
    public bool Destroying;
    public bool DontDestroy;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PGC_Destroyer>() && (!collision.GetComponent<PGC_Destroyer>().Destroying)) {
            if (!collision.GetComponent<PGC_Destroyer>().DontDestroy) {
                Destroying = true;
                collision.transform.parent.parent.GetComponent<PGC_Spawner>().Spawned = false;
                collision.transform.parent.parent.gameObject.SetActive(false);
                collision.transform.parent.parent.gameObject.SetActive(true);
                if (DontDestroy) {
                    collision.transform.parent.parent.GetComponent<PGC_Spawner>().SpawnCap();
                }
                this.transform.root.GetComponent<PGC_Generator>().DecreaseRoom();
                Destroy(collision.transform.parent.gameObject);
            }
        }
    }
}
