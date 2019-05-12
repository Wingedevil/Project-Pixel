using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public SpriteRenderer[] itemImages = new SpriteRenderer[PlayerStats.NUMBER_OF_ITEM_SLOTS];
    public GameObject[] items = new GameObject[PlayerStats.NUMBER_OF_ITEM_SLOTS];

    public void AddItem(GameObject add) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == null) {
                items[i] = add;
                itemImages[i].sprite = add.GetComponent<Item>().Thumbnail;
                itemImages[i].enabled = true;
                return;
            }
        }
    }

    public void RemoveItem(GameObject remove) {
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == remove) {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
                return;
            }
        }
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        ArrayList inv = PlayerStats.Inventory;
        for (int i = 0; i < inv.Count; i++) {
            if (items[i] != null) {
                items[i] = null;
                itemImages[i].sprite = null;
                itemImages[i].enabled = false;
            }
        }
    }
}