﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour {
    public abstract void Interact(Entity e);
    public abstract void Uninteract(Entity e);

    // Start is called before the first frame update
    protected virtual void Start() {

    }

    // Update is called once per frame
    protected virtual void Update() {

    }
}
