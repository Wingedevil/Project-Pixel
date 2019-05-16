using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : Projectile {
    public const float FORCE_MULT = 1.0f;

    public bool LockOn = true;
    public float SearchRadius;
    public float RotationSpeed;
    public string TagToHome;

    private GameObject target;

    // Start is called before the first frame update
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
        if (target == null) {
            float minDist = float.MaxValue;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag(TagToHome)) {
                float tempDist = Vector3.Distance(go.transform.position, this.transform.position);
                if (tempDist < minDist && tempDist <= SearchRadius) {
                    target = go;
                    minDist = tempDist;
                }
            }
        }

        try {
            Vector3 dir = target.transform.position - this.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, RotationSpeed * Time.deltaTime);
        } catch (NullReferenceException) {

        }

        this.GetComponent<Rigidbody2D>().velocity = transform.right * Speed;
    }
}