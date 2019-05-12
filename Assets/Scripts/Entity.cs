using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public int MaxHP;
    public bool offHandCanAttack;
    public float AttackSpeed = 1.0f;
    public float MoveSpeed = 1.0f;
    
    private int curHP;

    public int CurHP {
        get => curHP;
        set => curHP = value;
    }

    public virtual void TakeDamage(int hp) {
        curHP -= hp;
        if (curHP <= 0) {
            Die();
        }
    }

    protected virtual void Die() {
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    protected void Start() {
        curHP = MaxHP;
    }

    // Update is called once per frame
    protected void Update() {

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
        } catch (Exception) {
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