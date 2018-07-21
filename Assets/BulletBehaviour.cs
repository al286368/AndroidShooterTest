using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private const float BOUNDS_Y = 10;

    private float property_damage = 0;
    private float property_speed = 30;
    private float property_bounces = 10;

    private float temp_lifetime = 0;

    private const float BOUNCE_COOLDOWN = 0.25f;
    private float tmp_bounce_cooldown = 0;

    public float currentDegree = 0;

    public TrailRenderer TR;



    public void SetBullet(EntityBase e_user, Vector3 pos, float degree)
    {
        TR.Clear();
        property_damage = e_user.stat_damage;
        property_bounces = e_user.stat_bounces;
        transform.position = pos;
        currentDegree = degree;
        tmp_bounce_cooldown = 0;
        gameObject.SetActive(true);
    }
	void FixedUpdate () {
        transform.Translate(Vector3.right * Time.fixedDeltaTime * property_speed);
        transform.rotation = Quaternion.Euler(0,0,currentDegree);
        tmp_bounce_cooldown -= Time.fixedDeltaTime;
        CheckBounds();
	}
    private void CheckBounds()
    {
        if (Mathf.Abs(transform.position.y) > BOUNDS_Y)
        {
            RemoveBullet();
        }
        if (transform.position.x > StageManager.currentInstance.BOUNDS_MAX_X || transform.position.x < StageManager.currentInstance.BOUNDS_MIN_X)
        {
            if (tmp_bounce_cooldown <= 0)
            {
                if (property_bounces > 0)
                {
                    property_bounces--;
                    tmp_bounce_cooldown = BOUNCE_COOLDOWN;
                    LateralBounce();
                }
                else
                {
                    RemoveBullet();
                }
            }

        }
    }
    private void LateralBounce()
    {
        currentDegree = 180 - currentDegree;
    }
    private void RemoveBullet()
    {
        gameObject.SetActive(false);
    }
}
