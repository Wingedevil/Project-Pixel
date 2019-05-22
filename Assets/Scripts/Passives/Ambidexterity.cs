using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambidexterity : MonoBehaviour {

    // Start is called before the first frame update
    void Start() {
        this.GetComponentInParent<PlayerEntity>().Ambidextrous = true;
    }

    // Update is called once per frame
    void Update() {

    }
}