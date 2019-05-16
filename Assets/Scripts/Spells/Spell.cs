using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell : MonoBehaviour {
    public static Vector3 DISPLACEMENT = new Vector3(0f, -3.5f / 16f, 0f);
    public static float MELEE_DISPLACEMENT = 0.5f;
    public static float RANGED_DISPLACEMENT = 0.5f;
    public enum SpellType {
        StaffSpell,
        SwordSpell,
        BowSpell,
        ShieldSpell
    }

    public string Name;
    public SpellType Type;
    public int MPCost = 0;
    public int SPCost = 0;
    public float Cooldown;
    public int PrimarySpellDamage;
    public GameObject PrimarySpellProjectile;

    protected float CurrentCooldown;

    public abstract void Cast(Transform tf, Vector3 dir, string tag);
    public bool IsOnCooldown() {
        return CurrentCooldown > 0;
    }
    void Start() {
        CurrentCooldown = 0.0f;
    }
    void Update() {
        CurrentCooldown -= CurrentCooldown <= 0 ? 0 : Time.deltaTime;
    }
}
