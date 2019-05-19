﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Down : Interactable {
    public string NextLevel;

    public override void Interact(Entity e) {
        SceneManager.LoadScene(NextLevel);
    }

    public override void Uninteract(Entity e) {
        throw new System.NotImplementedException();
    }
}