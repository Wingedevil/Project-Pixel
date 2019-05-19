using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveCounterSpell : MonoBehaviour {
    public GameObject popupText;
    public float Radius = 1.0f;
    public float TimeBetweenTicks = 3.0f;
    public bool CounterPhysical = false;

    private float timeToNextTick = 0.0f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (timeToNextTick <= 0.0f) {
            foreach (Projectile pj in GameObject.FindObjectsOfType<Projectile>()) {
                if (Vector3.Distance(pj.transform.position, this.transform.position) <= Radius 
                    && pj.tag != this.GetComponentInParent<Entity>().gameObject.tag
                    && !pj.IsMelee
                    && (CounterPhysical ? (pj.IsMagical || pj.IsPhysical) : (pj.IsMagical && !pj.IsPhysical))
                    ) {
                    GameObject newText = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newText.GetComponent<PopupText>().FeedText("Countered", 15, pj.transform.position, Color.black);
                    pj.Kill();
                    timeToNextTick = TimeBetweenTicks;
                    return;
                }
            }
        }
        timeToNextTick -= Time.deltaTime;
    }
}