using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntropyAura : MonoBehaviour {
    public float Radius = 1.0f;
    public float TimeBetweenTicks = 1.0f;
    public float PercentDamagePerTick = 0.04f;

    private float timeToNextTick = 0.0f;

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (timeToNextTick <= 0.0f) {
            timeToNextTick = TimeBetweenTicks;
            foreach (Entity en in GameObject.FindObjectsOfType<Entity>()) {
                if (Vector3.Distance(en.transform.position, this.transform.position) <= Radius && en.tag != this.GetComponentInParent<Entity>().gameObject.tag) {
                    en.TakeDamage(new DamageMetadata((int)Mathf.Max(1, Mathf.Round(en.MaxHP * PercentDamagePerTick)), false, false));
                }
            }
        }
        timeToNextTick -= Time.deltaTime;
    }
}