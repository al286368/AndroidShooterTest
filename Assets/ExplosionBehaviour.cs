using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBehaviour : MonoBehaviour {

    public SpriteRenderer SR;
    private const float INITIAL_SCALE = 0.05f;

    private float explosion_damage;
    private float explosion_magnitude;
    private Enums.DamageType explosion_type;
    private float scaleMagnitude = 1;
    private float explosionAlpha = 1;
    private IEntity explosion_user;

    public void SetupExplosion(float damage, float magnitude, Enums.DamageType dmgtype, Vector3 initpos, IEntity user) {
        explosion_type = dmgtype;
        explosion_damage = damage;
        explosion_magnitude = magnitude;
        transform.position = initpos;
        scaleMagnitude = INITIAL_SCALE;
        explosionAlpha = 1;
        explosion_user = user;
        gameObject.SetActive(true);
    }
    private void Update()
    {
        scaleMagnitude += explosion_magnitude * Time.deltaTime;
        explosionAlpha -= Time.deltaTime * 1.5f;
        transform.localScale = scaleMagnitude * Vector3.one;
        SR.color = new Color(1,1,1, explosionAlpha);

        if (explosionAlpha <= 0) {
            gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Entity")
            return;
        IEntity lastDamagedEntity = collision.gameObject.GetComponent<IEntity>();
        if (lastDamagedEntity == null)
            return;
        if (explosion_user.IsAlly() == lastDamagedEntity.IsAlly())
            return;
        lastDamagedEntity.DealDamage(explosion_damage, explosion_type, explosion_user);
    }
}
