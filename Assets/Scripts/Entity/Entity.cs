using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour {
    public const float MP_REGEN = 0.5f;
    public const float SP_REGEN = 0.5f;

    public int MaxHP;
    protected float curHP;
    public int MaxSP;
    protected float curSP;
    public int MaxMP;
    protected float curMP;
    public GameObject popupText;
    public bool offHandCanAttack;
    public float AttackSpeed = 1.0f;
    public float MoveSpeed = 1.0f;
    public GameObject Corpse;

    private bool alive = true;

    public virtual void TakeDamage(DamageMetadata meta) {
        GameObject newText = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
        newText.GetComponent<PopupText>().FeedDamageMetaData(meta, this.transform.position);
        curHP -= meta.Damage;
        if (curHP <= 0 && alive) {
            alive = false;
            Die();
        }
    }

    protected virtual void Die() {
        Instantiate(Corpse, this.transform.position, Quaternion.identity).GetComponent<SpriteRenderer>().sortingOrder = -5;
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    protected virtual void Start() {
        curHP = MaxHP;
        curSP = MaxSP;
        curMP = MaxMP;
    }

    // Update is called once per frame
    protected virtual void Update() {
        curMP += curMP < MaxHP ? MP_REGEN * Time.deltaTime : 0;
        curSP += curSP < MaxSP ? SP_REGEN * Time.deltaTime : 0;
        curMP = Mathf.Min(curMP, MaxMP);
        curSP = Mathf.Min(curSP, MaxSP);
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
            if (a.parameterCount > 1) {
                a.SetBool(var, val);
            }
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