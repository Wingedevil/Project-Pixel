using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PGC_Destroyer : MonoBehaviour {
    public bool Destroying;
    public bool DontDestroy;
    public int UUID;

    // Start is called before the first frame update
    void Awake() {
        UUID = PGC_Generator.RoomUUID++;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<PGC_Destroyer>() 
            //&& (!collision.GetComponent<PGC_Destroyer>().Destroying) 
            && collision.GetComponent<PGC_Destroyer>().UUID > UUID) {
            if (!collision.GetComponent<PGC_Destroyer>().DontDestroy) {
                this.transform.root.GetComponent<PGC_Generator>().ModifyRooms();
                Destroy(collision.transform.parent.gameObject);
            }
        }
    }
}
