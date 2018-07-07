using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private const float BOUNDS_Y = 10;

    private float property_damage = 0;
    private float property_speed = 10;
    private float property_bounces = 10;

    private float temp_lifetime = 0;

    private const float BOUNCE_COOLDOWN = 0.5f;
    private float tmp_bounce_cooldown = 0;

    public float currentDegree = 0;




	void FixedUpdate () {
        transform.Translate(Vector3.right * Time.fixedDeltaTime * property_speed);
        transform.rotation = Quaternion.Euler(0,0,currentDegree);
        tmp_bounce_cooldown -= Time.fixedDeltaTime * property_speed;
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
        print("lateral bounce");
        currentDegree = 180 - currentDegree;
    }
    private void RemoveBullet()
    {
        gameObject.SetActive(false);
    }
}
