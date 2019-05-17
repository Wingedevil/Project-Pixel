using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupText : MonoBehaviour
{
    public const float DESTROY_AT = 60.0f;

    public Color NO_DAMAGE = Color.grey;
    public Color PHYSICAL_DAMAGE = Color.yellow;
    public Color MAGICAL_DAMAGE = Color.cyan;
    public Color HYBRID_DAMAGE = Color.magenta;
    public Color PURE_DAMAGE = Color.red;

    private float RateOfRise = 0.5f;
    private float Risen = 0.0f;
    private Vector3 Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() {
        transform.position = GameObject.FindGameObjectsWithTag("MainCamera")[0].GetComponent<Camera>().WorldToScreenPoint(Target);
        transform.position += Risen * Vector3.up;
        Risen += RateOfRise;
        if (Risen >= DESTROY_AT) {
            Destroy(this.gameObject);
        }
    }

    public void FeedDamageMetaData(DamageMetadata meta, Vector3 tf) {
        Target = tf + Random.onUnitSphere * 0.2f;
        this.GetComponent<Text>().text = "-" + meta.Damage;
        if (meta.Damage == 0) {
            this.GetComponent<Text>().color = NO_DAMAGE;
        } else if (meta.IsHybrid) {
            this.GetComponent<Text>().color = HYBRID_DAMAGE;
        } else if (meta.IsPhysical) {
            this.GetComponent<Text>().color = PHYSICAL_DAMAGE;
        } else if (meta.IsMagical) {
            this.GetComponent<Text>().color = MAGICAL_DAMAGE;
        } else {
            this.GetComponent<Text>().color = PURE_DAMAGE;
        }
    }

    public void Equip(Item item, Vector3 tf) {
        Target = tf;
        this.GetComponent<Text>().text = item.Name;
        this.GetComponent<Text>().fontSize = 15;
        this.GetComponent<Text>().color = Color.green;
    }
}
