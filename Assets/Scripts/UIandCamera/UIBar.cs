using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBar : MonoBehaviour {
    private float FullLength;
    private float FullHeight;

    // Start is called before the first frame update
    void Start() {
        FullLength = this.GetComponent<RectTransform>().sizeDelta.x;
        FullHeight = this.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update() {

    }

    public void ChangeSize(float xFraction, float yFraction = 1.0f) {
        this.GetComponent<RectTransform>().sizeDelta = new Vector2(FullLength * xFraction, FullHeight * yFraction);
    }
}
