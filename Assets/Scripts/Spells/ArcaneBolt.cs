using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneBolt : Spell {
    public override GameObject Cast(Transform tf, Vector3 dir) {
        GameObject prefab = ProjectileDictionary.PROJECTILE_SPELLBOOK.GetProjectile("Magic Bolt");
        GameObject newProj = MonoBehaviour.Instantiate(prefab, tf.position + DISPLACEMENT, Quaternion.identity);
        newProj.transform.Rotate(0, 0, Mathf.Rad2Deg * Mathf.Atan2(dir.y, dir.x));
        newProj.GetComponent<Projectile>().Direction = dir;
        return newProj;
    }
}
