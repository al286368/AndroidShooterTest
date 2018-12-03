using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private const float BOUNDS_Y = 10;

    private float damage_phys = 0;
    private float damage_photon = 0;
    private float damage_cryo = 0;
    private float damage_electric = 0;
    private float damage_nuclear = 0;
    private float property_speed = 30;
    private float property_bounces = 10;
    private float trajectory_helix = 0;
    private float trajectory_arc = 0;
    private float trajectory_track = 0;

    private const float BACK_TO_POOL_DELAY = 0.75f;
    private const float BOUNCE_COOLDOWN = 0.25f;
    private float tmp_bounceCooldown = 0;
    private float tmp_backToPoolDelay = 0;
    private bool waitingToRecycle = false;

    private EntityBase entity_user;

    private EntityBase currentTarget;

    public float currentDegree = 0;
    public int lr = 0;

    public TrailBehaviour trail;

    public Transform hitboxParent;




    public void SetBullet(Vector3 pos, float degree, EntityBase e_user)
    {
        entity_user = e_user;
        trail.TR.Clear();
        trail.SetTrail(0.2f, 0f, 0.25f, Color.blue, transform);
        transform.position = pos;
        currentDegree = degree;
        ResetBullet();

        damage_phys = e_user.stat_damage_physical;
        damage_photon = e_user.stat_damage_photon;
        damage_cryo = e_user.stat_damage_cryo;
        damage_electric = e_user.stat_damage_electric;
        damage_nuclear = e_user.stat_damage_nuclear;

        property_bounces = e_user.stat_bounces;

    }

	void FixedUpdate () {
        if (waitingToRecycle)
        {
            tmp_backToPoolDelay -= Time.fixedDeltaTime;
            if (tmp_backToPoolDelay < 0)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            UpdateTrajectory();
            transform.rotation = Quaternion.Euler(0, 0, currentDegree);
            transform.Translate(Vector3.right * Time.fixedDeltaTime * property_speed);
            tmp_bounceCooldown -= Time.fixedDeltaTime;
            CheckBounds();
        }

	}
    private void UpdateTrajectory() {
        if (trajectory_track > 0) {
            if (currentTarget == null)
                currentTarget = StageManager.currentInstance.GetRandomEnemy();
            else
            {
                currentDegree = Mathf.MoveTowardsAngle(currentDegree, TrackTo(currentTarget.transform), 600 * Time.fixedDeltaTime);
            }
        }
        if (trajectory_helix > 0) {
        }
        if (trajectory_arc > 0) {
        }
    }
    private void ResetBullet()
    {
        tmp_bounceCooldown = 0;
        gameObject.SetActive(true);
        waitingToRecycle = false;
        hitboxParent.gameObject.SetActive(true);
        tmp_backToPoolDelay = BACK_TO_POOL_DELAY;
    }
    private void CheckBounds()
    {
        if (Mathf.Abs(transform.position.y) > BOUNDS_Y)
        {
            RemoveBullet();
        }
        if (transform.position.x > StageManager.currentInstance.BOUNDS_MAX_X || transform.position.x < StageManager.currentInstance.BOUNDS_MIN_X)
        {
            BoundsCollision();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Entity")
            return;

        EntityBase lastDamagedEntity = collision.gameObject.GetComponent<EntityBase>();
        if (lastDamagedEntity == null)
            return;
        if (entity_user.isAlly == lastDamagedEntity.isAlly)
            return;
        HitEnemyEntity(lastDamagedEntity);
    }
    private void BoundsCollision()
    {
        if (tmp_bounceCooldown <= 0)
        {
            if (property_bounces > 0)
            {
                property_bounces--;
                tmp_bounceCooldown = BOUNCE_COOLDOWN;
                currentDegree = 180 - currentDegree;
            }
            else
            {
                RemoveBullet();
            }
        }
    }
    private void EntityCollision()
    {

        if (property_bounces > 0)
        {
            property_bounces--;
            currentDegree = Random.Range(0, 361);
        }
        else
        {
            RemoveBullet();
        }
    }
    float TrackTo(Transform targTransform)
    {
        if (targTransform == null)
            return 0;

        float difX = targTransform.position.x - transform.position.x;
        float difY = targTransform.position.y - transform.position.y;
        if (difX == 0)
        {
            if (difY > 0) { return 90; }
            else { return 270; }
        }

        if (difX > 0) { return Mathf.Atan(difY / difX) * Mathf.Rad2Deg; }
        else { return (Mathf.Atan(difY / difX) * Mathf.Rad2Deg) + 180; }
    }
    private void RemoveBullet()
    {
        waitingToRecycle = true;
        hitboxParent.gameObject.SetActive(false);
    }
    private void HitEnemyEntity(EntityBase e)
    {
        EntityCollision();
        e.DealDamage(damage_phys, Enums.DamageType.normal);
        e.DealDamage(damage_photon, Enums.DamageType.photon);
        e.DealDamage(damage_electric, Enums.DamageType.electric);
        e.DealDamage(damage_cryo, Enums.DamageType.cryo);
        e.DealDamage(damage_nuclear, Enums.DamageType.cryo);
    }
}
