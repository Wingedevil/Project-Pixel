using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCaster : BasicEnemy {
    public List<Spell> Spells;

    // Start is called before the first frame update
    protected new void Start() {
        base.Start();
        Spells = new List<Spell>();
        for (int i = 0; i < transform.childCount; i++) {
            if (transform.GetChild(i).GetComponent<Spell>()) {
                Spells.Add(transform.GetChild(i).GetComponent<Spell>());
            }
        }
    }

    // Update is called once per frame
    protected new void Update() {
        foreach (Spell sp in Spells) {
            if (!sp.IsOnCooldown() && Vector3.Magnitude(Player.transform.position - this.transform.position) <= DistanceToPlayer) {
                Vector3 originalVector = Player.transform.position - transform.position;
                Vector3 directionToPlayer = Vector3.Normalize(originalVector);
                this.GetComponentsInChildren<Weapon>()[0].Spell(directionToPlayer, this.tag, sp, () => StopDoing());
                UpdateAnimators("OnhandSpell", true);
            }
        }
        base.Update();
    }
}