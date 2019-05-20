using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarDisplay : MonoBehaviour {
    public int Bars = 1;
    public bool DisplayMP = true;
    public GameObject UI1;
    public GameObject UI2;
    public GameObject UI3;

    private GameObject red;
    private GameObject blue;
    private GameObject green;

    // Start is called before the first frame update
    void Start() {
        UpdateBars();
    }

    // Update is called once per frame
    void Update() {

    }

    public void UpdateBars() {
        switch (Bars) {
            case 1:
                UI1.SetActive(true);
                UI2.SetActive(false);
                UI3.SetActive(false);
                red = UI1.transform.Find("Red").gameObject;
                break;
            case 2:
                UI1.SetActive(false);
                UI2.SetActive(true);
                UI3.SetActive(false);
                red = UI2.transform.Find("Red").gameObject;
                blue = UI2.transform.Find("Blue").gameObject;
                green = UI2.transform.Find("Green").gameObject;
                if (DisplayMP) {
                    blue = UI2.transform.Find("Blue").gameObject;
                    green.SetActive(false);
                } else {
                    green = UI2.transform.Find("Green").gameObject;
                    blue.SetActive(false);
                }
                break;
            case 3:
                UI1.SetActive(false);
                UI2.SetActive(false);
                UI3.SetActive(true);
                red = UI3.transform.Find("Red").gameObject;
                blue = UI3.transform.Find("Blue").gameObject;
                green = UI3.transform.Find("Green").gameObject;
                break;
            default:
                break;
        }
    }

    public void UpdateRed(float frac) {
        red.GetComponent<UIBar>().ChangeSize(frac);
    }

    public void UpdateBlue(float frac) {
        blue.GetComponent<UIBar>().ChangeSize(frac);
    }

    public void UpdateGreen(float frac) {
        green.GetComponent<UIBar>().ChangeSize(frac);
    }
}