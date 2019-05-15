using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity {
    public const float TIME_INVULNERABLE = 1.0f;
    public const float FLASH_INTERVAL = 1f / 30f;

    public GameObject WeaponSlot;
    public GameObject OffhandSlot;
    public GameObject ArmorSlot;

    public GameObject WeaponItem;
    public GameObject OffhandItem;
    public GameObject ArmorItem;

    protected Item.Type WeaponType;
    protected Item.Type OffhandType;
    protected Item.Type ArmorType;

    protected bool vulnerable = true;
    protected bool visible = true;
    protected float flashIntervalCounter = 0.0f;
    protected float timeAlrInvuln = 0.0f;
    
    private List<GameObject> itemList;

    public bool TryToPickUp() {
        if (!this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            return false;
        }
        try {
            GameObject itemPickedUp = itemList[0];
            itemList.RemoveAt(0);
            switch (itemPickedUp.GetComponent<Item>().ItemType) {
                case Item.Type.Armor:
                    Instantiate(ArmorItem, this.transform.position, Quaternion.identity).SetActive(true);
                    foreach (Transform child in ArmorSlot.transform) {
                        GameObject.Destroy(child.gameObject);
                    }
                    Instantiate(itemPickedUp.GetComponent<Item>().ItemReference, ArmorSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 2;
                    ;
                    ArmorItem = Instantiate(itemPickedUp, ArmorSlot.transform);
                    ArmorItem.SetActive(false);
                    ArmorItem.transform.position = Vector3.zero;
                    GameObject newTextA = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newTextA.GetComponent<PopupText>().Equip(ArmorItem.GetComponent<Item>(), this.transform.position);
                    ArmorType = itemPickedUp.GetComponent<Item>().ItemType;
                    Destroy(itemPickedUp);
                    break;
                case Item.Type.Shield:
                    Instantiate(OffhandItem, this.transform.position, Quaternion.identity).SetActive(true);
                    foreach (Transform child in OffhandSlot.transform) {
                        GameObject.Destroy(child.gameObject);
                    }
                    Instantiate(itemPickedUp.GetComponent<Item>().ItemReference, OffhandSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 4;
                    OffhandItem = Instantiate(itemPickedUp, OffhandSlot.transform);
                    OffhandItem.SetActive(false);
                    OffhandItem.transform.position = Vector3.zero;
                    GameObject newTextO = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newTextO.GetComponent<PopupText>().Equip(OffhandItem.GetComponent<Item>(), this.transform.position);
                    OffhandType = itemPickedUp.GetComponent<Item>().ItemType;
                    Destroy(itemPickedUp);
                    break;
                case Item.Type.Bow:
                    Instantiate(WeaponItem, this.transform.position, Quaternion.identity).SetActive(true);
                    foreach (Transform child in WeaponSlot.transform) {
                        GameObject.Destroy(child.gameObject);
                    }
                    Instantiate(itemPickedUp.GetComponent<Item>().ItemReference, WeaponSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 3;
                    WeaponItem = Instantiate(itemPickedUp, WeaponSlot.transform);
                    WeaponItem.SetActive(false);
                    WeaponItem.transform.position = Vector3.zero;
                    GameObject newTextB = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newTextB.GetComponent<PopupText>().Equip(WeaponItem.GetComponent<Item>(), this.transform.position);
                    WeaponType = Item.Type.Bow;
                    Destroy(itemPickedUp);
                    break;
                case Item.Type.Staff:
                case Item.Type.Sword:
                    Instantiate(WeaponItem, this.transform.position, Quaternion.identity).SetActive(true);
                    foreach (Transform child in WeaponSlot.transform) {
                        GameObject.Destroy(child.gameObject);
                    }
                    Instantiate(itemPickedUp.GetComponent<Item>().ItemReference, WeaponSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 3;
                    WeaponItem = Instantiate(itemPickedUp, WeaponSlot.transform);
                    WeaponItem.SetActive(false);
                    WeaponItem.transform.position = Vector3.zero;
                    GameObject newTextTR = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newTextTR.GetComponent<PopupText>().Equip(WeaponItem.GetComponent<Item>(), this.transform.position);
                    WeaponType = itemPickedUp.GetComponent<Item>().ItemType;
                    Destroy(itemPickedUp);
                    break;
                default:
                    break;
            }
            UpdateAnimators();
            return true;
        } catch (ArgumentOutOfRangeException) {
            //no item underneath player
            return false;
        }
    }

    protected void UpdateAnimators() {
        if (WeaponType == Item.Type.Bow) {
            UpdateAnimators("Bow", true);
        } else {
            UpdateAnimators("Bow", false);
        }
    }
    protected override void Die() {
        Instantiate(Corpse, this.transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }

    public override void TakeDamage(DamageMetadata meta) {
        if (vulnerable) {
            timeAlrInvuln = 0.0f;
            flashIntervalCounter = 0.0f;
            vulnerable = false;
            base.TakeDamage(meta);
        }
    }

    protected override void Start() {
        base.Start();
        itemList = new List<GameObject>();
        Instantiate(ArmorItem.GetComponent<Item>().ItemReference, ArmorSlot.transform);
        ArmorSlot.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder = 2;
        Instantiate(WeaponItem.GetComponent<Item>().ItemReference, WeaponSlot.transform);
        WeaponSlot.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder = 3;
        Instantiate(OffhandItem.GetComponent<Item>().ItemReference, OffhandSlot.transform);
        OffhandSlot.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder = 4;
    }

    protected override void Update() {
        base.Update();
        if (!vulnerable) {
            if (timeAlrInvuln >= TIME_INVULNERABLE) {
                vulnerable = true;
                SetAllSpriteRender(true);
            } else {
                if (flashIntervalCounter >= FLASH_INTERVAL) {
                    flashIntervalCounter = 0.0f;
                    visible = !visible;
                    SetAllSpriteRender(visible);
                } else {
                    flashIntervalCounter += Time.deltaTime;
                }
                timeAlrInvuln += Time.deltaTime;
            }
        }
    }

    private void SetAllSpriteRender(bool state) {
        SpriteRenderer[] spriteRenderer = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderer) {
            sr.enabled = state;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Item>() != null) {
            if (!itemList.Contains(collision.gameObject)) {
                itemList.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Item>() != null) {
            if (itemList.Contains(collision.gameObject)) {
                itemList.Remove(collision.gameObject);
            }
        }
    }
}