using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipable : MonoBehaviour {
    public int MaxMP = 0;
    public int MaxSP = 0;
    public float MPCost = 1.0f;
    public float SPCost = 1.0f;
    public int PhysicalArmor;
    public int PhysicalReduction;
    public float MagicalResistance = 1.0f;
    public float AttackSpeed = 1.0f;
    public float SpellDamageAmp = 1.0f;
}