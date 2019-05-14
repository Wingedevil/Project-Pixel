using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {
    public enum Type {
        Staff,
        Sword,
        Bow,
        Shield,
        Armor
    };

    public string Name;
    public Type ItemType;
    public Sprite Thumbnail;
    public GameObject ItemReference;

    protected virtual void Awake() {
        this.GetComponent<SpriteRenderer>().sprite = Thumbnail;
    }
}
