using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour {
    public const float MP_REGEN = 2.0f;
    public const float SP_REGEN = 2.0f;

    public int MaxHP;
    public float curHP;
    public int MaxSP;
    public float curSP;
    public int MaxMP;
    public float curMP;

    public int PhysicalArmor;
    public int PhysicalReduction;
    public float MagicalResistance = 1.0f;

    public float SpellDamageAmp = 1.0f;
    public float MPCost = 1.0f;
    public float SPCost = 1.0f;

    public GameObject popupText;
    public bool offHandCanAttack;
    public float AttackSpeed = 1.0f;
    public float MoveSpeed = 1.0f;
    public GameObject Corpse;

    
    private bool alive = true;

    public virtual DamageMetadata DamageReduction(DamageMetadata meta) {
        float damage = meta.Damage;
        if (meta.IsHybrid) {
            float magDamage = damage * MagicalResistance;
            float percentageReduction = 20.0f / (20 + PhysicalArmor);
            float phyDamage = damage - PhysicalReduction;
            phyDamage = damage * percentageReduction;
            damage -= Mathf.Sqrt((1 - magDamage) * (1 - phyDamage));
        } else if (meta.IsMagical) {
            damage *= MagicalResistance;
        } else if (meta.IsPhysical) {
            float percentageReduction = 20.0f / (20 + PhysicalArmor);
            damage -= PhysicalReduction;
            damage *= percentageReduction;
        }

        DamageMetadata rtv = new DamageMetadata((int)Mathf.Round(damage), meta.IsPhysical, meta.IsMagical);
        return rtv;
    }

    public virtual void SetHPTo1() {
        if (curHP != 1) {
            GameObject newText = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
            newText.GetComponent<PopupText>().FeedText("Decay", 15, this.transform.position, Color.black);
        }
        curHP = 1;
    }

    public virtual void Heal(DamageMetadata meta) {
        if (curHP < MaxHP) {
            DamageMetadata newMeta = DamageReduction(meta);
            GameObject newText = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
            newText.GetComponent<PopupText>().FeedHealMetaData(newMeta, this.transform.position);
            if (meta.Damage > 0) {
                curHP += newMeta.Damage;
                curHP = curHP > MaxHP ? MaxHP : curHP ;
            }
        }
    }

    public virtual void TakeDamage(DamageMetadata meta) {
        DamageMetadata newMeta = DamageReduction(meta);
        GameObject newText = Instantiate(popupText, GameObject.FindGameObjectsWithTag("Canvas")[0].transform);
        newText.GetComponent<PopupText>().FeedDamageMetaData(newMeta, this.transform.position);
        if (meta.Damage > 0) {
            curHP -= newMeta.Damage;
        }
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
        curMP += curMP < MaxMP ? MP_REGEN * Time.deltaTime : 0;
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