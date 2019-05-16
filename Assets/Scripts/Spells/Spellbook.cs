using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spellbook : MonoBehaviour {
    private static List<GameObject> PrivateSpells;

    private void Start() {
        PrivateSpells = new List<GameObject>();
        foreach (Transform tr in this.GetComponentInChildren<Transform>()) {
            if (tr.GetComponent<Spell>() != null) {
                PrivateSpells.Add(tr.gameObject);
            }
        }
    }
    public static Spell GetSpell(string name) {
        foreach (GameObject go in PrivateSpells) {
            if (go.GetComponent<Spell>().Name == name) {
                return go.GetComponent<Spell>();
            }
        }
        return null;
    }
}