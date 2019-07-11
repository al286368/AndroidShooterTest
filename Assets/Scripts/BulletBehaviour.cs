using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    private const float BOUNDS_Y = 15;
    private const float BASE_SPEED = 15;
    private const float BACK_TO_POOL_DELAY = 0.75f;
    private const float BOUNCE_COOLDOWN = 0.05f;

    private float damage_phys = 0;
    private float damage_photon = 0;
    private float damage_cryo = 0;
    private float damage_electric = 0;
    private float damage_nuclear = 0;
    private float damage_plasma = 0;
    private float damage_gamma = 0;
    private float damage_graviton = 0;
    private float property_speedScale = 1;
    private int property_bounces = 0;
    private int property_piercing = 0;

    private float tmp_bounceCooldown = 0;
    private float tmp_backToPoolDelay = 0;
    private DeflecterProperties lastDeflectTouched;
    private bool waitingToRecycle = false;

    private IEntity entity_user;

    private float lifetime = 0;
    private float directionLocal = 0;
    private float directionParent = 0;

    private bool isAlly;

    public Transform hitboxParent;
    public BulletVisualsManager[] visualParents;

    [Header("Trajectory Managers (TEMP?)")]
    public TrajectoryNormal traj_normal;
    public TrajectoryDeflected traj_deflect;
    public TrajectoryHelix traj_helix;
    public TrajectoryTracking traj_tracking;
    public TrajectoryBinarytrack traj_binarytrack;


    private BulletVisualsManager BVMInUse;
    private ITrajectory trajectoryInUse;




    public void SetBullet(Vector3 pos, float parentDegree, float localDegree,IEntity e_user)
    {
        lifetime = 0;
        entity_user = e_user;
        isAlly = e_user.IsAlly();

        transform.position = pos;
        directionLocal = localDegree;
        directionParent = parentDegree;

        ResetBullet();
        SetupVisuals();
        SetNewTrajectory(e_user.GetWeaponProjectileTrajectory());

        damage_phys = e_user.GetPhysicalDamage();
        damage_photon = e_user.GetPhotonDamage();
        damage_cryo = e_user.GetCryoDamage();
        damage_electric = e_user.GetElectricDamage();
        damage_nuclear = e_user.GetNuclearDamage();
        damage_plasma = e_user.GetPlasmaDamage();
        damage_gamma = e_user.GetGammaDamage();
        damage_graviton = e_user.GetGravitonDamage();
        property_speedScale = e_user.GetBulletSpeedScale();

        property_bounces = e_user.GetBulletBounces();
        property_piercing = e_user.GetBulletPiercing();

    }
    void SetupVisuals()
    {
        for (int i = 0; i < visualParents.Length; i++)
        {
            visualParents[i].gameObject.SetActive(false);
        }

        switch (entity_user.GetDamageElement())
        {
            case WeaponData.DamageElement.pulse:
                {
                    BVMInUse = visualParents[0];
                    visualParents[0].gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.cryo:
                {
                    BVMInUse = visualParents[1];
                    visualParents[1].gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.nuclear:
                {
                    BVMInUse = visualParents[2];
                    visualParents[2].gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.electric:
                {
                    BVMInUse = visualParents[3];
                    visualParents[3].gameObject.SetActive(true);
                    break;
                }
            
            case WeaponData.DamageElement.photon:
                {
                    BVMInUse = visualParents[4];
                    visualParents[4].gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.plasma:
                {
                    BVMInUse = visualParents[5];
                    visualParents[5].gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.gamma:
                {
                    BVMInUse = visualParents[6];
                    visualParents[6].gameObject.SetActive(true);
                    break;
                }
            case WeaponData.DamageElement.graviton:
                {
                    BVMInUse = visualParents[7];
                    visualParents[7].gameObject.SetActive(true);
                    break;
                }
        }
        BVMInUse.SetTrail(isAlly);
    }
    public void SetNewTrajectory(WeaponData.ProjectileTrajectory newTrajectory) {

        traj_normal.enabled = false;
        traj_helix.enabled = false;
        traj_tracking.enabled = false;
        traj_binarytrack.enabled = false;
        traj_deflect.enabled = false;

        switch (newTrajectory) {
            case WeaponData.ProjectileTrajectory.helix:
                {
                    trajectoryInUse = traj_helix;
                    break;
                }
            case WeaponData.ProjectileTrajectory.tracking:
                {

                    trajectoryInUse = traj_tracking;
                    break;
                }
            case WeaponData.ProjectileTrajectory.binarytrack:
                {

                    trajectoryInUse = traj_binarytrack;
                    break;
                }
            case WeaponData.ProjectileTrajectory.deflected: {
                    trajectoryInUse = traj_deflect;
                    break;
                }
            default:
                {
                    trajectoryInUse = traj_normal;
                    break;
                }
        }
        trajectoryInUse.ResetTrajectory(this, directionLocal, directionParent);
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
            transform.rotation = Quaternion.Euler(0, 0, directionLocal + directionParent);
            transform.Translate(Vector3.right * Time.fixedDeltaTime * GetTimeScale() * BASE_SPEED);
            tmp_bounceCooldown -= Time.fixedDeltaTime * GetTimeScale();
            lifetime += Time.fixedDeltaTime * GetTimeScale();
            CheckBounds();
        }

	}
    private void ResetBullet()
    {
        tmp_bounceCooldown = 0;
        lastDeflectTouched = null;
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
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            WallCollision(collision);
        }
        if (collision.gameObject.tag == "Deflect")
        {
            lastDeflectTouched = collision.gameObject.GetComponent<DeflecterProperties>();

            if (lastDeflectTouched != null) {
                if (lastDeflectTouched.GetUser().IsAlly() != IsAlly()) {
                    isAlly = !isAlly;
                    SetNewTrajectory(WeaponData.ProjectileTrajectory.deflected);
                }
            }

        }
        if (collision.gameObject.tag != "Entity")
            return;
        IEntity lastDamagedEntity = collision.gameObject.GetComponent<IEntity>();
        if (lastDamagedEntity == null)
            return;
        if (isAlly == lastDamagedEntity.IsAlly())
            return;
        HitEnemyEntity(lastDamagedEntity);
    }

    private void WallCollision(Collision2D collisionInfo)
    {
        if (tmp_bounceCooldown <= 0)
        {
            if (property_bounces > 0)
            {
                Vector3 hit = collisionInfo.contacts[0].normal;
                trajectoryInUse.SendWallCollisionNotification(Mathf.Atan(hit.x / hit.y) * Mathf.Rad2Deg + 90);
                property_bounces--;
                tmp_bounceCooldown = BOUNCE_COOLDOWN;
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
            trajectoryInUse.SendEntityCollisionNotification();
            property_bounces--;
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
    private void HitEnemyEntity(IEntity e)
    {
        if (damage_nuclear > 0)
        {
            ObjectPool.currentInstance.GetExplosionFromPool().SetupExplosion(damage_nuclear, 1, Enums.DamageType.nuclearDamage, transform.position, entity_user);
        }
        if (damage_graviton > 0) {
            ObjectPool.currentInstance.GetGravitonFromPool().SetupExplosion(damage_graviton, 1, transform.position, entity_user);
        }
        if (damage_gamma > 0)
        {
            ObjectPool.currentInstance.GetGammaBeamFromPool().SetAs(damage_gamma, entity_user, transform.position, directionLocal + directionParent);
        }
        if (property_piercing > 0)
        {
            property_piercing--;
        }
        else
        {
            EntityCollision();
        }
        e.DealDamage(damage_phys, Enums.DamageType.normal, entity_user);
        e.DealDamage(damage_photon, Enums.DamageType.photon, entity_user);
        e.DealDamage(damage_electric, Enums.DamageType.electricEffect, entity_user);
        e.DealDamage(damage_cryo, Enums.DamageType.cryo, entity_user);
        e.DealDamage(damage_plasma, Enums.DamageType.plasma, entity_user);

    }
    public void SetAngle(float local, float parent) {
        directionLocal = local;
        directionParent = parent;
    }
    public bool IsWaitingToBeRemoved() {
        return waitingToRecycle;
    }
    public bool IsAlly() {
        return isAlly;
    }
    public float GetLifetime() {
        return lifetime;
    }
    public float GetTimeScale() {
        if (isAlly) {
            return property_speedScale;
        } else {
            return property_speedScale * StageManager.currentInstance.difficultyBulletTimeScaleFactor;
        }

    }
    public DeflecterProperties GetLastDeflecterTouched() {
        return lastDeflectTouched;
    }
}
