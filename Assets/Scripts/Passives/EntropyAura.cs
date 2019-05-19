using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntropyAura : MonoBehaviour {
    public Vector3 DISPLACEMENT = new Vector3(0, -3f/16f, 0f);

    public GameObject explode;
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
                if (Vector3.Distance(en.transform.position, this.transform.position) <= Radius && en.tag != this.GetComponentInParent<Entity>().gameObject.tag && en.tag != "Wall") {
                    en.TakeDamage(new DamageMetadata((int)Mathf.Max(1, Mathf.Round(en.MaxHP * PercentDamagePerTick)), false, false));
                    ParticleSystem ps = Instantiate(explode, en.transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
                    ParticleSystem.MainModule psMain = ps.main;
                    ParticleSystem.VelocityOverLifetimeModule psVelo = ps.velocityOverLifetime;
                    psMain.startColor = Color.black;
                    psMain.gravityModifier = 0.05f;
                    psVelo.radial = new ParticleSystem.MinMaxCurve(0.1f, 1f);
                }
            }
        }
        timeToNextTick -= Time.deltaTime;
    }
}