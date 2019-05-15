using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour {
    public int MaxHP;
    public GameObject popupText;
    public bool offHandCanAttack;
    public float AttackSpeed = 1.0f;
    public float MoveSpeed = 1.0f;
    public GameObject Corpse;
    
    protected int curHP;

    public int CurHP {
        get => curHP;
        set => curHP = value;
    }

    public virtual void TakeDamage(DamageMetadata meta) {
        GameObject newText = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
        newText.GetComponent<PopupText>().FeedDamageMetaData(meta, this.transform.position);
        curHP -= meta.Damage;
        if (curHP <= 0) {
            Die();
        }
    }

    protected virtual void Die() {
        Instantiate(Corpse, this.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().sortingOrder = -1;
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        curHP = MaxHP;
    }

    // Update is called once per frame
    protected virtual void Update() {

    }

    public void Attack(Vector3 shotDirection) {
        if (!this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Moving")
            && !this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            return;
        }
        try {
            shotDirection = shotDirection == Vector3.zero ? new Vector3(transform.localScale.x, 0, 0) : shotDirection;
            GetComponentsInChildren<Weapon>()[0].Shoot(shotDirection, this.tag, () => StopDoing());
            if (offHandCanAttack) {
                UpdateAnimators("2H", true);
                GetComponentsInChildren<Weapon>()[1].Shoot(shotDirection, this.tag, () => StopDoing());
            } else {
                UpdateAnimators("2H", false);
            }
        } catch (Exception e) {
            Debug.Log(e);
            UpdateAnimators("2H", false);
        }
        UpdateAnimators("Attack", true);
    }

    public void Cast(string spell, Vector3 shotDirection) {
        if (!this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Moving")
            && !this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            return;
        }
        shotDirection = shotDirection == Vector3.zero ? new Vector3(transform.localScale.x, 0, 0) : shotDirection;
        GetComponentsInChildren<Weapon>()[0].Spell(shotDirection, this.tag, spell, () => StopDoing());
        UpdateAnimators(Spellbook.GetType(spell), true);
    }

    public void NotAttacking() {
        UpdateAnimators("Attack", false);
    }

    public void StopDoing() {
        UpdateAnimators("BowSpell", false);
        UpdateAnimators("OffhandSpell", false);
        UpdateAnimators("OnhandSpell", false);
        UpdateAnimators("Attack", false);
    }

    public void UpdateAnimators(string var, bool val) {
        Animator[] childrenAnimators = gameObject.GetComponentsInChildren<Animator>();
        foreach (Animator a in childrenAnimators) {
            a.SetBool(var, val);
        }
    }

    public void Flip(int direction) {
        if (this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Moving")
            || this.GetComponentsInChildren<Animator>()[0].GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
            transform.localScale = new Vector3(direction, 1, 1);
        }
    }

    private void FlipWithoutCheck(int direction) {
        transform.localScale = new Vector3(direction, 1, 1);
    }
}