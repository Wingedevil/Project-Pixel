using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEntity : Entity {
    public const float TIME_INVULNERABLE = 1.0f;
    public const float FLASH_INTERVAL = 1f / 30f;

    public GameObject HPBar;
    public GameObject MPBar;
    public GameObject SPBar;

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
    
    private List<GameObject> interactableList;

    public bool Interact() {
        if (!(this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Idle")
            || this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Moving"))) {
            return false;
        }
        try {
            GameObject interactable = interactableList[0];
            interactableList.RemoveAt(0);
            if (interactable.GetComponent<Item>() != null) {
                switch (interactable.GetComponent<Item>().ItemType) {
                    case Item.Type.Armor:
                        Instantiate(ArmorItem, this.transform.position, Quaternion.identity).SetActive(true);
                        foreach (Transform child in ArmorSlot.transform) {
                            GameObject.Destroy(child.gameObject);
                        }
                        Instantiate(interactable.GetComponent<Item>().ItemReference, ArmorSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 2;
                        ArmorItem.GetComponent<Interactable>().Uninteract(this);
                        ArmorItem = Instantiate(interactable, ArmorSlot.transform);
                        ArmorItem.SetActive(false);
                        ArmorItem.transform.position = Vector3.zero;
                        GameObject newTextA = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                        newTextA.GetComponent<PopupText>().Equip(ArmorItem.GetComponent<Item>(), this.transform.position);
                        ArmorType = interactable.GetComponent<Item>().ItemType;
                        interactable.GetComponent<Interactable>().Interact(this);
                        Destroy(interactable);
                        break;
                    case Item.Type.Shield:
                        Instantiate(OffhandItem, this.transform.position, Quaternion.identity).SetActive(true);
                        foreach (Transform child in OffhandSlot.transform) {
                            GameObject.Destroy(child.gameObject);
                        }
                        Instantiate(interactable.GetComponent<Item>().ItemReference, OffhandSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 4;
                        OffhandItem.GetComponent<Interactable>().Uninteract(this);
                        OffhandItem = Instantiate(interactable, OffhandSlot.transform);
                        OffhandItem.SetActive(false);
                        OffhandItem.transform.position = Vector3.zero;
                        GameObject newTextO = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                        newTextO.GetComponent<PopupText>().Equip(OffhandItem.GetComponent<Item>(), this.transform.position);
                        OffhandType = interactable.GetComponent<Item>().ItemType;
                        interactable.GetComponent<Interactable>().Interact(this);
                        Destroy(interactable);
                        break;
                    case Item.Type.Bow:
                        Instantiate(WeaponItem, this.transform.position, Quaternion.identity).SetActive(true);
                        foreach (Transform child in WeaponSlot.transform) {
                            GameObject.Destroy(child.gameObject);
                        }
                        Instantiate(interactable.GetComponent<Item>().ItemReference, WeaponSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 3;
                        WeaponItem.GetComponent<Interactable>().Uninteract(this);
                        WeaponItem = Instantiate(interactable, WeaponSlot.transform);
                        WeaponItem.SetActive(false);
                        WeaponItem.transform.position = Vector3.zero;
                        GameObject newTextB = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                        newTextB.GetComponent<PopupText>().Equip(WeaponItem.GetComponent<Item>(), this.transform.position);
                        WeaponType = Item.Type.Bow;
                        interactable.GetComponent<Interactable>().Interact(this);
                        Destroy(interactable);
                        break;
                    case Item.Type.Staff:
                    case Item.Type.Sword:
                        Instantiate(WeaponItem, this.transform.position, Quaternion.identity).SetActive(true);
                        foreach (Transform child in WeaponSlot.transform) {
                            GameObject.Destroy(child.gameObject);
                        }
                        Instantiate(interactable.GetComponent<Item>().ItemReference, WeaponSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 3;
                        WeaponItem.GetComponent<Interactable>().Uninteract(this);
                        WeaponItem = Instantiate(interactable, WeaponSlot.transform);
                        WeaponItem.SetActive(false);
                        WeaponItem.transform.position = Vector3.zero;
                        GameObject newTextTR = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                        newTextTR.GetComponent<PopupText>().Equip(WeaponItem.GetComponent<Item>(), this.transform.position);
                        WeaponType = interactable.GetComponent<Item>().ItemType;
                        interactable.GetComponent<Interactable>().Interact(this);
                        Destroy(interactable);
                        break;
                    default:
                        break;
                }
                UpdateAnimators();
                return true;
            } else {
                interactable.GetComponent<Interactable>().Interact(this);
                return true;
            }
        } catch (ArgumentOutOfRangeException e) {
            //no item underneath player
            Debug.Log(e);
            return false;
        }
    }

    public void Cast(string spell, Vector3 shotDirection) {
        if (!this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Moving")
            && !this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            return;
        }
        Spell spellSpell = Spellbook.GetSpell(spell);
        if (curMP < spellSpell.MPCost || curSP < spellSpell.SPCost) {
            return;
        }
        shotDirection = shotDirection == Vector3.zero ? new Vector3(transform.localScale.x, 0, 0) : shotDirection;
        switch (spellSpell.Type) {
            case Spell.SpellType.StaffSpell:
                if (WeaponItem.GetComponent<Item>().ItemType != Item.Type.Staff) {
                    return;
                }
                if (WeaponSlot.GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing())) {
                    ChangeMP(-spellSpell.MPCost);
                    ChangeSP(-spellSpell.SPCost);
                    UpdateAnimators("OnhandSpell", true);
                }
                break;
            case Spell.SpellType.SwordSpell:
                if (WeaponItem.GetComponent<Item>().ItemType != Item.Type.Sword) {
                    return;
                }
                if (WeaponSlot.GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing())) {
                    ChangeMP(-spellSpell.MPCost);
                    ChangeSP(-spellSpell.SPCost);
                    UpdateAnimators("OnhandSpell", true);
                }
                break;
            case Spell.SpellType.BowSpell:
                if (WeaponItem.GetComponent<Item>().ItemType != Item.Type.Bow) {
                    return;
                }
                if (WeaponSlot.GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing())) {
                    ChangeMP(-spellSpell.MPCost);
                    ChangeSP(-spellSpell.SPCost);
                    UpdateAnimators("BowSpell", true);
                }
                break;
            case Spell.SpellType.ShieldSpell:
                if (WeaponItem.GetComponent<Item>().ItemType != Item.Type.Shield) {
                    return;
                }
                if (OffhandSlot.GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing())) {
                    ChangeMP(-spellSpell.MPCost);
                    ChangeSP(-spellSpell.SPCost);
                    UpdateAnimators("OffhandSpell", true);
                }
                break;
        }
    }

    protected void UpdateAnimators() {
        if (WeaponType == Item.Type.Bow) {
            UpdateAnimators("Bow", true);
        } else {
            UpdateAnimators("Bow", false);
        }
        Animator[] childrenAnimators = gameObject.GetComponentsInChildren<Animator>();
        foreach (Animator a in childrenAnimators) {
            a.Play(0, -1, 0);
        }
    }

    protected override void Die() {
        Instantiate(Corpse, this.transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }

    public override void TakeDamage(DamageMetadata meta) {
        if (vulnerable) {
            if (meta.Damage >= 0) {
                timeAlrInvuln = 0.0f;
                flashIntervalCounter = 0.0f;
                vulnerable = false;
                this.GetComponent<Collider2D>().enabled = false;
            }
            base.TakeDamage(meta);
            HPBar.GetComponent<UIBar>().ChangeSize(1.0f * curHP / MaxHP);
        }
    }

    private void SetAllSpriteRender(bool state) {
        SpriteRenderer[] spriteRenderer = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderer) {
            sr.enabled = state;
        }
    }

    public void ChangeMP(float mp) {
        curMP += mp;
        MPBar.GetComponent<UIBar>().ChangeSize(1.0f * curMP / MaxMP);
    }

    public void ChangeSP(float sp) {
        curSP += sp;
        SPBar.GetComponent<UIBar>().ChangeSize(1.0f * curSP / MaxSP);
    }

    protected override void Start() {
        base.Start();
        interactableList = new List<GameObject>();
        Instantiate(ArmorItem.GetComponent<Item>().ItemReference, ArmorSlot.transform);
        ArmorItem.GetComponent<Interactable>().Interact(this);
        ArmorSlot.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder = 2;
        Instantiate(WeaponItem.GetComponent<Item>().ItemReference, WeaponSlot.transform);
        WeaponItem.GetComponent<Interactable>().Interact(this);
        WeaponSlot.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder = 3;
        Instantiate(OffhandItem.GetComponent<Item>().ItemReference, OffhandSlot.transform);
        OffhandItem.GetComponent<Interactable>().Interact(this);
        OffhandSlot.GetComponentsInChildren<SpriteRenderer>()[0].sortingOrder = 4;
    }

    protected override void Update() {
        base.Update();
        MPBar.GetComponent<UIBar>().ChangeSize(1.0f * curMP / MaxMP);
        SPBar.GetComponent<UIBar>().ChangeSize(1.0f * curSP / MaxSP);
        if (!vulnerable) {
            if (timeAlrInvuln >= TIME_INVULNERABLE) {
                vulnerable = true;
                this.GetComponent<Collider2D>().enabled = true;
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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.GetComponent<Interactable>() != null) {
            if (!interactableList.Contains(collision.gameObject)) {
                interactableList.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.GetComponent<Interactable>() != null) {
            if (interactableList.Contains(collision.gameObject)) {
                interactableList.Remove(collision.gameObject);
            }
        }
    }
}