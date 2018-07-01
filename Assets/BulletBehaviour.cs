using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private const float BOUNDS_X = 10;
    private const float BOUNDS_Y = 10;

    private float property_damage = 0;
    private float property_speed = 0;
    private float property_bounces = 0;

    private float temp_lifetime = 0;




	void Update () {
        transform.Translate(2 * Time.deltaTime, 0, 0);
	}
    private void CheckBounds()
    {
        if (Mathf.Abs(transform.position.y) > BOUNDS_Y)
        {
            gameObject.SetActive(false);
        }
        if (Mathf.Abs(transform.position.x) > BOUNDS_X)
        {
            if (property_bounces > 0)
            {

            }
            else
            {

            }
        }
    }
}
