using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GammaBeamBehaviour : MonoBehaviour
{
    private float damage;
    private IEntity entityUser;
    public Transform beamScaleAndRotation;
    public SpriteRenderer beamVisual;
    public GameObject beamHitbox;

    private float lifetimeRemaining = 0;


    public void SetAs(float dmg, IEntity eUser, Vector3 initPos, float direction) {
        entityUser = eUser;
        damage = dmg;
        transform.position = initPos;
        beamScaleAndRotation.transform.rotation = Quaternion.Euler(0,0,direction);
        lifetimeRemaining = 1f;
        gameObject.SetActive(true);
    }
    public void Update()
    {
        lifetimeRemaining -= Time.deltaTime * 3;

        beamVisual.transform.localScale = new Vector3(1,1-lifetimeRemaining, 1);
        beamVisual.color = new Color(beamVisual.color.r, beamVisual.color.g, beamVisual.color.b, lifetimeRemaining);

        if (lifetimeRemaining < 0)
            gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Entity")
            return;
        IEntity lastDamagedEntity = collision.gameObject.GetComponent<IEntity>();
        if (lastDamagedEntity == null)
            return;
        if (entityUser.IsAlly() == lastDamagedEntity.IsAlly())
            return;
        lastDamagedEntity.DealDamage(damage, Enums.DamageType.gamma, entityUser);
    }
}
