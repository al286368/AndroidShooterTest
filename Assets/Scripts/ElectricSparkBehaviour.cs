using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricSparkBehaviour : MonoBehaviour {

    public Transform sparkVisualParent;

    private const float VISUAL_OSCILATION_RANGE = 0.4f;
    private int spark_bounces;
    private float spark_damage;
    private IEntity spark_user;
    private bool waitingToBeDisabled = false;
    private float disableDelayRemaining = 1f;
    private const float DISABLE_DELAY_TIME = 1f;
    private const float MOVE_SPEED = 40;

    private IEntity target = null;



    // Use this for initialization
    public void SetupSpark(Vector3 initPos, int bounces, float dmg, IEntity user) {
        transform.position = initPos;
        spark_bounces = bounces;
        spark_damage = dmg;
        spark_user = user;
        waitingToBeDisabled = false;
        target = null;
        gameObject.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
        if (!waitingToBeDisabled)
        {
            UpdateVisuals();
            if (target == null)
            {
                FindNewTarget();
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.GetGameObject().transform.position, Time.deltaTime * MOVE_SPEED);
                if (transform.position == target.GetGameObject().transform.position)
                {
                    target.DealDamage(spark_damage, Enums.DamageType.electricDamage, spark_user);
                    spark_bounces--;
                    if (spark_bounces >= 0)
                    {
                        FindNewTarget();
                    }
                    else
                    {
                        DisableSpark();
                    }
                        
                }
            }
        }
        else
        {
            disableDelayRemaining -= Time.deltaTime;
            if (disableDelayRemaining <= 0)
                gameObject.SetActive(false);
        }
	}
    void DisableSpark() {
        waitingToBeDisabled = true;
        disableDelayRemaining = DISABLE_DELAY_TIME;
    }
    void FindNewTarget() {
        target = StageManager.currentInstance.GetClosestRivalEntityToPoint(transform.position, spark_user, target);
        if (target == null) {
            DisableSpark();
        }

    }
    void UpdateVisuals() {
        sparkVisualParent.localPosition = new Vector3(Random.Range(-VISUAL_OSCILATION_RANGE, VISUAL_OSCILATION_RANGE), Random.Range(-VISUAL_OSCILATION_RANGE, VISUAL_OSCILATION_RANGE),0);
    }

}
