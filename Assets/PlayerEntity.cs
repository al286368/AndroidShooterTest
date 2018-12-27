using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, IEntity {

    private ShipData playerShipData;
    private WeaponData playerWeaponData;
    public float currentHealth;
    public float currentShield;
    public float status_frozen;
    public float status_burning;
    public Collider2D entityHitbox;
    private bool coRoutine_burning = false;

    private const float SHOOT_SECUENCE_DELAY = 0.05f;
    private float shootReady = 0;
    private float lastShootAngle = 0;
    private float shieldRecoverReady = 0;

    void Start() {
        ResetEntity();
    }
    void Update()
    {
        shootReady += Time.deltaTime * GetWeaponFirerate();
        if (shieldRecoverReady > 0)
            shieldRecoverReady -= Time.deltaTime;
        else {
            currentShield = Mathf.MoveTowards(currentShield, playerShipData.GetShipShield(), Time.deltaTime * playerShipData.GetShieldRecoveryRate());
        }
    }
    #region Shooting Methods
    private int GetLrForIndex(int index) {
        if (GetMultishoot() == 1)
            return 0;
        if (GetMultishoot() % 2 != 0 && index + 1 == (GetMultishoot() / 2)+1)
            return 0;
        else if (index < GetMultishoot() / 2f)
            return -1;
        else
            return 1;
    }
    private void ShootNormal(float angle)
    {
        float baseShootAngle = angle;
        if (GetMultishoot() > 1)
        {
            float tmpAngle = baseShootAngle + (GetMultishootArc() / 2f);
            int t = 0;
            while (t < GetMultishoot())
            {
                CreateAttackBasedOnWeaponType(tmpAngle, GetLrForIndex(t));
                tmpAngle -= GetMultishootArc() / (GetMultishoot() - 1);
                t++;
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle);

        }


    }
    IEnumerator ShootBarrageStandard()
    {
        float baseShootAngle = lastShootAngle;
        int count = 0;
        while (count < 5)
        {
            ShootNormal(baseShootAngle);
            count++;
            yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
        }
    }
    IEnumerator ShootBarrage1Way()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle = baseShootAngle + (GetMultishootArc() / 2f);
        int t = 0;

        if (GetMultishoot() > 1)
        {

            while (t < GetMultishoot())
            {
                CreateAttackBasedOnWeaponType(tmpAngle, GetLrForIndex(t));
                tmpAngle -= GetMultishootArc() / (GetMultishoot() - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle);

        }
    }
    IEnumerator ShootBarrage2Way()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle = baseShootAngle + (GetMultishootArc() / 2f);
        int t = 0;

        if (GetMultishoot() > 1)
        {

            while (t < GetMultishoot())
            {
                CreateAttackBasedOnWeaponType(tmpAngle, GetLrForIndex(t));
                tmpAngle -= GetMultishootArc() / (GetMultishoot() - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }

            t--;
            tmpAngle += GetMultishootArc() / (GetMultishoot() - 1);

            while (t >= 0)
            {
                CreateAttackBasedOnWeaponType(tmpAngle, GetLrForIndex(t));
                tmpAngle += GetMultishootArc() / (GetMultishoot() - 1);
                t--;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle);

        }
    }
    IEnumerator ShootBarrageCrossfull()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle1 = baseShootAngle + (GetMultishootArc() / 2f);
        float tmpAngle2 = baseShootAngle - (GetMultishootArc() / 2f);
        int i = 0;

        if (GetMultishoot() > 1)
        {

            while (i < GetMultishoot())
            {
                CreateAttackBasedOnWeaponType(tmpAngle1, GetLrForIndex(i));
                CreateAttackBasedOnWeaponType(tmpAngle2, -GetLrForIndex(i));

                tmpAngle1 -= GetMultishootArc() / (GetMultishoot() - 1);
                tmpAngle2 += GetMultishootArc() / (GetMultishoot() - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle);
        }
    }
    IEnumerator ShootBarrageCrosshalf()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle1 = baseShootAngle + (GetMultishootArc() / 2f);
        float tmpAngle2 = baseShootAngle - (GetMultishootArc() / 2f);
        int i = 0;

        if (GetMultishoot() > 1)
        {

            while (i < (GetMultishoot() / 2))
            {
                CreateAttackBasedOnWeaponType(tmpAngle1, GetLrForIndex(i));
                CreateAttackBasedOnWeaponType(tmpAngle2, -GetLrForIndex(i));

                tmpAngle1 -= GetMultishootArc() / (GetMultishoot() - 1);
                tmpAngle2 += GetMultishootArc() / (GetMultishoot() - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
            if (GetMultishoot() % 2 != 0)
            {
                CreateAttackBasedOnWeaponType(baseShootAngle);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle);
        }
    }
    private void CreateAttackBasedOnWeaponType(float degree, int lr = 0)
    {
        switch (GetWeaponAttackType())
        {
            case WeaponData.AttackType.beam:
                {
                    CreateBeam(degree);
                    break;
                }
            case WeaponData.AttackType.projectile:
                {
                    CreateBullet(degree, lr);
                    break;
                }
        }
    }
    private void CreateBullet(float degree, int lr = 0)
    {
        BulletBehaviour bullet;
        bullet = ObjectPool.currentInstance.GetBulletFromPool();
        if (GetWeaponRandomSpread() > 0) bullet.SetBullet(transform.position, degree + Random.Range(-GetWeaponRandomSpread(), GetWeaponRandomSpread()), this, lr);
        else bullet.SetBullet(transform.position, degree, this, lr);
    }
    private void CreateBeam(float degree)
    {
        BeamBehaviour beam;
        beam = ObjectPool.currentInstance.GetBeamFromPool();
        if (GetWeaponRandomSpread() > 0) beam.SetBeam(this, transform.position, GetWeaponBounces(), degree + Random.Range(-GetWeaponRandomSpread(), GetWeaponRandomSpread()), entityHitbox);
        else beam.SetBeam(this, transform.position, GetWeaponBounces(), degree, entityHitbox);
    }
    #endregion
    #region Interface implementation and Getters
    public void DealDamage(float amount, Enums.DamageType dmgType)
    {
        if (amount <= 0)
            return;
        if (amount < 1) amount = 1;

        switch (dmgType)
        {
            case Enums.DamageType.normal:
                {
                    SubstractHealthAndShield(amount, false);
                    break;
                }
            case Enums.DamageType.photon:
                {
                    if (!coRoutine_burning)
                        StartCoroutine("FireDamageOverTime");
                    status_burning += amount;
                    break;
                }
            case Enums.DamageType.cryo:
                {
                    SubstractHealthAndShield(amount, false);
                    status_frozen += amount;
                    break;
                }
            case Enums.DamageType.nuclear:
                {
                    SubstractHealthAndShield(amount, true);
                    break;
                }
            case Enums.DamageType.electric:
                {
                    SubstractHealthAndShield(amount, false);
                    break;
                }
        }
    }
    private void SubstractHealthAndShield(float dmg, bool ignoreShield)
    {
        shieldRecoverReady = playerShipData.GetShieldRecoveryDelay();
        if (ignoreShield)
        {
            currentHealth -= dmg;
        }
        else
        {
            currentShield -= dmg;
            if (currentShield < 0)
            {
                currentHealth += currentShield;
                currentShield = 0;
            }
        }

        if (currentHealth <= 0)
        {
            Kill();
        }

    }
    public int GetBulletBounces()
    {
        return playerWeaponData.GetBounces();
    }

    public float GetCryoDamage()
    {
        return playerWeaponData.GetCryoDamage();
    }

    public float GetElectricDamage()
    {
        return playerWeaponData.GetElectricDamage();
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public float GetNuclearDamage()
    {
        return playerWeaponData.GetNuclearDamage();
    }

    public float GetPhotonDamage()
    {
        return playerWeaponData.GetPhotonDamage();
    }

    public float GetPhysicalDamage()
    {
        return playerWeaponData.GetPhysicalDamage();
    }
    public int GetMultishoot()
    {
        return playerWeaponData.GetMultishoot();
    }
    public float GetMultishootArc()
    {
        return playerWeaponData.GetMultishootArc();
    }
    public float GetTrajectoryHelix() {
        return playerWeaponData.GetTrajectoryHelix();
    }
    public float GetTrajectoryWave() {
        return playerWeaponData.GetTrajectoryWave();
    }
    public float GetTrajectoryTracking() {
        return playerWeaponData.GetTrajectoryTracking();
    }
    public float GetWeaponRandomSpread()
    {
        return playerWeaponData.GetRandomSpread();
    }
    public int GetWeaponBounces()
    {
        return playerWeaponData.GetBounces();
    }
    public float GetBulletSpeedScale() {
        return playerWeaponData.GetProjectileSpeed();
    }
    public float GetWeaponFirerate() {
        return playerWeaponData.GetFirerate();
    }
    public WeaponData.AttackType GetWeaponAttackType()
    {
        return playerWeaponData.GetAttackType();
    }
    public WeaponData.ShootSecuence GetWeaponShootSecuence()
    {
        return playerWeaponData.GetWeaponShootSecuence();
    }
    public bool IsAlly()
    {
        return true;
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }

    public void Shoot(float angle)
    {
        if (shootReady < 1)
            return;
        lastShootAngle = angle;
        shootReady = 0;

        switch (GetWeaponShootSecuence())
        {
            case WeaponData.ShootSecuence.normal:
                {
                    ShootNormal(angle);
                    break;
                }
            case WeaponData.ShootSecuence.barrage_standard:
                {
                    StartCoroutine("ShootBarrageStandard");
                    break;
                }
            case WeaponData.ShootSecuence.barrage_1way:
                {
                    StartCoroutine("ShootBarrage1Way");
                    break;
                }
            case WeaponData.ShootSecuence.barrage_2way:
                {
                    StartCoroutine("ShootBarrage2Way");
                    break;
                }
            case WeaponData.ShootSecuence.barrage_crossful:
                {
                    StartCoroutine("ShootBarrageCrossfull");
                    break;
                }
            case WeaponData.ShootSecuence.barrage_crosshalf:
                {
                    StartCoroutine("ShootBarrageCrosshalf");
                    break;
                }
        }
    }

    public float GetEntityTimescale()
    {
        return 1;
    }

    public void ResetEntity()
    {
        // TEST ONLY
        playerWeaponData = new WeaponData(WeaponData.WeaponGenerationSetting.playerWeapon);
        playerShipData = new ShipData();
        StageManager.currentInstance.RegisterPlayerEntity(this);

        currentHealth = playerShipData.GetShipEnergy();
        currentShield = playerShipData.GetShipShield();
        
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public float GetHealthPercent()
    {
        return currentHealth / playerShipData.GetShipEnergy();
    }
    public float GetShieldPercent() {
        return currentShield / playerShipData.GetShipShield();
    }
    public float GetShieldRecoveryPercent() {
        return (playerShipData.GetShieldRecoveryDelay()-shieldRecoverReady) / playerShipData.GetShieldRecoveryDelay();
    }
    public int GetCurrentHealth() {
        return (int)currentHealth;
    }
    public int GetCurrentShield() {
        return (int)currentShield;
    }
    #endregion
}
