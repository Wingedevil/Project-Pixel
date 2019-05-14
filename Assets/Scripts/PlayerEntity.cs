using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : Entity {
    public const float TIME_INVULNERABLE = 1.0f;
    public const float FLASH_INTERVAL = 1f / 30f;

    public GameObject GraveStone;

    protected bool vulnerable = true;
    protected bool visible = true;
    protected float flashIntervalCounter = 0.0f;
    protected float timeAlrInvuln = 0.0f;

    private SpriteRenderer[] spriteRenderer;

    protected override void Die() {
        Instantiate(GraveStone, this.transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }

    public override void TakeDamage(DamageMetadata meta) {
        if (vulnerable) {
            timeAlrInvuln = 0.0f;
            flashIntervalCounter = 0.0f;
            vulnerable = false;
            base.TakeDamage(meta);
        }
    }

    protected override void Start() {
        base.Start();
        spriteRenderer = this.gameObject.GetComponentsInChildren<SpriteRenderer>();
    }

    protected override void Update() {
        base.Update();
        if (!vulnerable) {
            if (timeAlrInvuln >= TIME_INVULNERABLE) {
                vulnerable = true;
                SetAllSpriteRender(true);
            } else {
                if (flashIntervalCounter >= FLASH_INTERVAL) {
                    flashIntervalCounter = 0.0f;
                    visible = !visible;
                    SetAllSpriteRender(visible);
                } else {
                    flashIntervalCounter += Time.deltaTime;
                }
                timeAlrInvuln += Time.deltaTime;
            }
        }
    }

    private void SetAllSpriteRender(bool state) {
        foreach (SpriteRenderer sr in spriteRenderer) {
            sr.enabled = state;
        }
    }
}