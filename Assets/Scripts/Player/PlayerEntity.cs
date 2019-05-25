using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerEntity : Entity {
    public const float TIME_INVULNERABLE = 1.0f;
    public const float PERCENTAGE_MAX_INVULNERABLE = 0.05f;
    public const float FLASH_INTERVAL = 1f / 30f;

    private bool _MPEnabled = true;
    public bool MPEnabled {
        get => _MPEnabled;
        set {
            _MPEnabled = value;
            UpdateBars();
        }
    }

    private bool _SPEnabled = false;
    public bool SPEnabled {
        get => _SPEnabled;
        set {
            _SPEnabled = value;
            UpdateBars();
        }
    }

    public GameObject WeaponSlot;
    public GameObject OffhandSlot;
    public GameObject ArmorSlot;

    public GameObject WeaponItem;
    public GameObject OffhandItem;
    public GameObject ArmorItem;

    protected GameObject canvas;

    protected Item.Type WeaponType;
    protected Item.Type OffhandType;
    protected Item.Type ArmorType;

    protected bool vulnerable = true;
    protected bool visible = true;
    protected float flashIntervalCounter = 0.0f;
    protected float timeInvuln = 0.0f;

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
                EquipItem(interactable);
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

    public void EquipItem(GameObject item, bool instantiate = true, bool popUp = true) {
        switch (item.GetComponent<Item>().ItemType) {
            case Item.Type.Armor:
                if (instantiate) {
                    Instantiate(ArmorItem, this.transform.position, Quaternion.identity).SetActive(true);
                }
                foreach (Transform child in ArmorSlot.transform) {
                    GameObject.Destroy(child.gameObject);
                }
                Instantiate(item.GetComponent<Item>().ItemReference, ArmorSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 2;
                item.GetComponent<Interactable>().Interact(this);
                ArmorItem.GetComponent<Interactable>().Uninteract(this);
                ArmorItem = Instantiate(item, ArmorSlot.transform);
                ArmorItem.SetActive(false);
                ArmorItem.transform.position = Vector3.zero;
                if (popUp) {
                    GameObject newTextA = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newTextA.GetComponent<PopupText>().Equip(ArmorItem.GetComponent<Item>(), this.transform.position);
                }
                ArmorType = item.GetComponent<Item>().ItemType;
                Destroy(item);
                break;
            case Item.Type.Shield:
                if (instantiate) {
                    Instantiate(OffhandItem, this.transform.position, Quaternion.identity).SetActive(true);
                }
                foreach (Transform child in OffhandSlot.transform) {
                    GameObject.Destroy(child.gameObject);
                }
                Instantiate(item.GetComponent<Item>().ItemReference, OffhandSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 4;
                item.GetComponent<Interactable>().Interact(this);
                OffhandItem.GetComponent<Interactable>().Uninteract(this);
                OffhandItem = Instantiate(item, OffhandSlot.transform);
                OffhandItem.SetActive(false);
                OffhandItem.transform.position = Vector3.zero;
                if (popUp) {
                    GameObject newTextO = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newTextO.GetComponent<PopupText>().Equip(OffhandItem.GetComponent<Item>(), this.transform.position);
                }
                OffhandType = item.GetComponent<Item>().ItemType;
                Destroy(item);
                break;
            case Item.Type.Bow:
                if (instantiate) {
                    Instantiate(WeaponItem, this.transform.position, Quaternion.identity).SetActive(true);
                }
                foreach (Transform child in WeaponSlot.transform) {
                    GameObject.Destroy(child.gameObject);
                }
                Instantiate(item.GetComponent<Item>().ItemReference, WeaponSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 3;
                item.GetComponent<Interactable>().Interact(this);
                WeaponItem.GetComponent<Interactable>().Uninteract(this);
                WeaponItem = Instantiate(item, WeaponSlot.transform);
                WeaponItem.SetActive(false);
                WeaponItem.transform.position = Vector3.zero;
                if (popUp) {
                    GameObject newTextB = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newTextB.GetComponent<PopupText>().Equip(WeaponItem.GetComponent<Item>(), this.transform.position);
                }
                WeaponType = Item.Type.Bow;
                Destroy(item);
                break;
            case Item.Type.Staff:
            case Item.Type.Sword:
                if (instantiate) {
                    Instantiate(WeaponItem, this.transform.position, Quaternion.identity).SetActive(true);
                }
                foreach (Transform child in WeaponSlot.transform) {
                    GameObject.Destroy(child.gameObject);
                }
                Instantiate(item.GetComponent<Item>().ItemReference, WeaponSlot.transform).GetComponent<SpriteRenderer>().sortingOrder = 3;
                item.GetComponent<Interactable>().Interact(this);
                WeaponItem.GetComponent<Interactable>().Uninteract(this);
                WeaponItem = Instantiate(item, WeaponSlot.transform);
                WeaponItem.SetActive(false);
                WeaponItem.transform.position = Vector3.zero;
                if (popUp) {
                    GameObject newTextTR = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
                    newTextTR.GetComponent<PopupText>().Equip(WeaponItem.GetComponent<Item>(), this.transform.position);
                }
                WeaponType = item.GetComponent<Item>().ItemType;
                Destroy(item);
                break;
            default:
                break;
        }
        UpdateAnimators();
        if (OffhandItem.GetComponent<Item>().Name == "Fists of Fury"
            && WeaponItem.GetComponent<Item>().Name != "Fists of Fury") {
            offHandCanAttack = false;
        } else {
            offHandCanAttack = Ambidextrous;
        }

        if (!instantiate && !popUp) {
            curHP = MaxHP;
            curMP = MaxMP;
            curSP = MaxSP;
        }
    }

    public bool Cast(string spell, Vector3 shotDirection) {
        if (!this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Moving")
            && !this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            return false;
        }
        Spell spellSpell = Spellbook.GetSpell(spell);
        if (curMP < spellSpell.MPCost * MPCost || curSP < spellSpell.SPCost * SPCost || curHP < spellSpell.HPCost) {
            return false;
        }
        if ((spellSpell.MPCost > 0 && !MPEnabled) || (spellSpell.SPCost > 0 && !SPEnabled)) {
            return false;
        }
        shotDirection = shotDirection == Vector3.zero ? new Vector3(transform.localScale.x, 0, 0) : shotDirection;
        switch (spellSpell.Type) {
            case Spell.SpellType.StaffSpell:
                if (WeaponItem.GetComponent<Item>().ItemType != Item.Type.Staff) {
                    return false;
                }
                if (WeaponSlot.GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing())) {
                    ChangeHP(-spellSpell.HPCost);
                    ChangeMP(-spellSpell.MPCost * MPCost);
                    ChangeSP(-spellSpell.SPCost * SPCost);
                    UpdateAnimators("OnhandSpell", true);
                }
                break;
            case Spell.SpellType.SwordSpell:
                if (WeaponItem.GetComponent<Item>().ItemType != Item.Type.Sword) {
                    return false;
                }
                if (WeaponSlot.GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing())) {
                    ChangeHP(-spellSpell.HPCost);
                    ChangeMP(-spellSpell.MPCost * MPCost);
                    ChangeSP(-spellSpell.SPCost * SPCost);
                    UpdateAnimators("OnhandSpell", true);
                }
                break;
            case Spell.SpellType.BowSpell:
                if (WeaponItem.GetComponent<Item>().ItemType != Item.Type.Bow) {
                    return false;
                }
                if (WeaponSlot.GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing())) {
                    ChangeHP(-spellSpell.HPCost);
                    ChangeMP(-spellSpell.MPCost * MPCost);
                    ChangeSP(-spellSpell.SPCost * SPCost);
                    UpdateAnimators("BowSpell", true);
                }
                break;
            case Spell.SpellType.ShieldSpell:
                if (WeaponItem.GetComponent<Item>().ItemType != Item.Type.Shield) {
                    return false;
                }
                if (OffhandSlot.GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing())) {
                    ChangeHP(-spellSpell.HPCost);
                    ChangeMP(-spellSpell.MPCost * MPCost);
                    ChangeSP(-spellSpell.SPCost * SPCost);
                    UpdateAnimators("OffhandSpell", true);
                }
                break;
        }
        return true;
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
        Instantiate(Corpse, this.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().sortingOrder = -5;
        this.gameObject.SetActive(false);
        Invoke("ConcludeDeath", 1.0f);
    }

    private void ConcludeDeath() {
        Time.timeScale = 0f;
        SceneManager.LoadScene("DeathScreen", LoadSceneMode.Additive);
    }

    public override void Heal(DamageMetadata meta) {
        base.Heal(meta);
        canvas.GetComponent<UIBarDisplay>().UpdateRed(1.0f * curHP / MaxHP);
    }

    public override void TakeDamage(DamageMetadata meta) {
        if (vulnerable) {
            if (meta.Damage >= 0) {
                timeInvuln = TIME_INVULNERABLE * Mathf.Clamp(meta.Damage / (PERCENTAGE_MAX_INVULNERABLE * MaxHP), 0.1f, 1.0f);
                flashIntervalCounter = 0.0f;
                vulnerable = false;
                this.GetComponent<Collider2D>().enabled = false;
            }
            base.TakeDamage(meta);
            canvas.GetComponent<UIBarDisplay>().UpdateRed(1.0f * curHP / MaxHP);
        }
    }

    private void SetAllSpriteRender(bool state) {
        SpriteRenderer[] spriteRenderer = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderer) {
            sr.enabled = state;
        }
    }

    public void ChangeHP(float hp) {
        curHP += hp;
        DamageMetadata newMeta = new DamageMetadata((int)hp * -1, false, false);
        if (hp < 0) {
            GameObject newText = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
            newText.GetComponent<PopupText>().FeedDamageMetaData(newMeta, this.transform.position);
        }
        canvas.GetComponent<UIBarDisplay>().UpdateRed(1.0f * curHP / MaxHP);
    }

    public void ChangeMP(float mp) {
        curMP += mp;
        canvas.GetComponent<UIBarDisplay>().UpdateBlue(1.0f * curMP / MaxMP);
    }

    public void ChangeSP(float sp) {
        curSP += sp;
        canvas.GetComponent<UIBarDisplay>().UpdateGreen(1.0f * curSP / MaxSP);
    }

    public void UpdateBars() {
        int rtv = 1;
        if (MPEnabled) {
            rtv++;
        }
        if (SPEnabled) {
            rtv++;
        }
        canvas.GetComponent<UIBarDisplay>().Bars = rtv;
        canvas.GetComponent<UIBarDisplay>().DisplayMP = MPEnabled;
        canvas.GetComponent<UIBarDisplay>().UpdateBars();
    }

    protected override void Start() {
        // DontDestroyOnLoad(this.gameObject);
        base.Start();
        if (canvas == null) {
            canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];

        }
        UpdateBars();
        interactableList = new List<GameObject>();
        InstantiateItems();

        if (OffhandItem.GetComponent<Item>().Name == "Fists of Fury"
            && WeaponItem.GetComponent<Item>().Name == "Fists of Fury") {
            offHandCanAttack = true;
        }
    }

    public void InstantiateItems() {
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
        if (canvas == null) {
            canvas = GameObject.FindGameObjectsWithTag("Canvas")[0];
            UpdateBars();
        }
        canvas.GetComponent<UIBarDisplay>().UpdateRed(1.0f * curHP / MaxHP);
        if (MPEnabled) {
            canvas.GetComponent<UIBarDisplay>().UpdateBlue(1.0f * curMP / MaxMP);
        }
        if (SPEnabled) {
            canvas.GetComponent<UIBarDisplay>().UpdateGreen(1.0f * curSP / MaxSP);
        }
        if (!vulnerable) {
            if (timeInvuln <= 0.0) {
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
                timeInvuln -= Time.deltaTime;
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