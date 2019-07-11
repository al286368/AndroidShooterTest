using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImplosionBehaviour : MonoBehaviour
{
    private float damage = 0;
    private float magnitude = 1;
    private IEntity eUser;

    public Transform hitboxParent;
    public Transform growingWaveParent;
    public Transform implosionParent;
    public SpriteRenderer growingWaveSR;
    public SpriteRenderer implosionSR;

    private float animation1 = 0;
    private float animation2 = 0;

    private const float DAMAGE_SCALING_PER_TARGET = 0.25f;

    private List<IEntity> targetsInRange;

    public void SetupExplosion(float dmg, float mag, Vector3 initpos, IEntity user)
    {
        damage = dmg;
        magnitude = mag;
        transform.position = initpos;
        eUser = user;
        targetsInRange = new List<IEntity>();
        animation1 = 0;
        animation2 = 0;

        growingWaveSR.color = new Color(1, 1, 1, 1);
        implosionSR.color = new Color(1,1,1,0);
        growingWaveParent.transform.localScale = Vector3.one * 0.1f;
        implosionParent.transform.localScale = Vector3.one * 1;
        hitboxParent.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }
    private void Update()
    {
        if (animation1 < 1) {
            animation1 += Time.deltaTime;
            growingWaveSR.color = new Color(1, 1, 1, 1-animation1);
            growingWaveParent.localScale = Vector3.one * animation1;
            if (animation1 > 1) {
                hitboxParent.gameObject.SetActive(true);
            }
        }
        else {
            animation2 += Time.deltaTime * 5;
            implosionSR.color = new Color(1, 1, 1, animation2);
            implosionParent.localScale = Vector3.one * (1 - animation2);
            if (animation2 > 1) {
                DamageAllTargetsInRange();
                gameObject.SetActive(false);
            }
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Entity")
            return;
        IEntity lastDetectedEntity = collision.gameObject.GetComponent<IEntity>();
        if (lastDetectedEntity == null)
            return;
        if (eUser.IsAlly() == lastDetectedEntity.IsAlly())
            return;
        targetsInRange.Add(lastDetectedEntity);
    }
    private void DamageAllTargetsInRange() {
        float dmgMultiplier = 1 + ((targetsInRange.Count-1) * DAMAGE_SCALING_PER_TARGET);
        for (int i = 0; i < targetsInRange.Count; i++) {
            targetsInRange[i].DealDamage(damage * dmgMultiplier, Enums.DamageType.graviton, eUser);
        }
    }
    public bool IsAlly() {
        return eUser.IsAlly();
    }
}
