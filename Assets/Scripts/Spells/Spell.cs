using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spell {
    public static Vector3 DISPLACEMENT = new Vector3(0f, -3.5f / 16f, 0f);
    public static float MELEE_DISPLACEMENT = 0.5f;
    public static float RANGED_DISPLACEMENT = 0.5f;
    public abstract GameObject Cast(Transform tf, Vector3 dir);
}
