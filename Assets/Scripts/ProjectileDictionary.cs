using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileDictionary : MonoBehaviour {
    public static ProjectileDictionary PROJECTILE_SPELLBOOK;

    public GameObject[] Projectiles;

    private static Dictionary<string, int> NameToIndex;

    // Start is called before the first frame update
    void Start() {
        NameToIndex = new Dictionary<string, int> {
            { "Arcane Bolt", 0 }
        };
        PROJECTILE_SPELLBOOK = this;
    }

    // Update is called once per frame
    void Update() {

    }

    public GameObject GetProjectile(string name) {
        NameToIndex.TryGetValue(name, out int rtv);
        return Projectiles[rtv];
    }
}