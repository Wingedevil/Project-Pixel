using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endure : MonoBehaviour {
    public float SP_REGEN = 10.0f;

    private PlayerEntity playerEntity;

    // Start is called before the first frame update
    void Start() {
        playerEntity = this.GetComponentInParent<PlayerEntity>();
    }

    // Update is called once per frame
    void Update() {
        if (playerEntity.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            playerEntity.ChangeSP(SP_REGEN * Time.deltaTime);
        }
    }
}
