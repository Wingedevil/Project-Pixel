using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zen : MonoBehaviour {
    public float SP_REGEN = 2.0f;

    private PlayerEntity playerEntity;

    // Start is called before the first frame update
    void Start() {
        playerEntity = this.GetComponentInParent<PlayerEntity>();
    }

    // Update is called once per frame
    void Update() {
        if (playerEntity
            .GetComponent<Rigidbody2D>()
            .velocity.magnitude <= 0.01f) {
            playerEntity.ChangeSP(SP_REGEN * Time.deltaTime);
        }
    }
}
