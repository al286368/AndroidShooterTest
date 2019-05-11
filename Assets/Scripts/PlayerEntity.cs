using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour, IEntity {

    private ShipData playerShipData;
    private WeaponData playerWeaponData;
    public Collider2D entityHitbox;

    public float currentHealth;
    public float currentShield;
    public float status_frozen;
    public float status_burning;

    private ISpecial specialInUse;

    private bool coRoutine_burning = false;
    private bool overheat = false;

    private float tmpShootReady = 0;
    private float tmpWeaponHeat = 0;
    private float tmpHeatRecoveryStrenght = 0;
    private float tmpSpecialReady = 0;

    private float lastShootAngle = 0;
    private float shieldRecoverReady = 0;

    private const float SHOOT_SECUENCE_DELAY = 0.1f;
    private const float HEAT_RECOVERY_RAMPUP = 1.35f;
    private const float HEAT_RECOVERY_ON_OVERHEAT = 1.35f;

    void Start() {
        ResetEntity();
    }
    void Update()
    {
        UpdatePlayerParamsAndStatus();
    }
    void UpdatePlayerParamsAndStatus() {
        tmpShootReady += Time.deltaTime * GetWeaponFirerate();
        tmpSpecialReady = Mathf.MoveTowards(tmpSpecialReady, 100, Time.deltaTime * playerShipData.GetSpecialRechargeRate());


        tmpWeaponHeat = Mathf.MoveTowards(tmpWeaponHeat, 0, Time.deltaTime * playerShipData.GetHeatRecovery() * tmpHeatRecoveryStrenght);
        if (overheat && tmpWeaponHeat == 0)
        {
            tmpHeatRecoveryStrenght = HEAT_RECOVERY_ON_OVERHEAT;
            if (tmpWeaponHeat == 0)
                overheat = false;
        }
        else {
            tmpHeatRecoveryStrenght += Time.deltaTime * HEAT_RECOVERY_RAMPUP;
        }


        if (shieldRecoverReady > 0)
            shieldRecoverReady -= Time.deltaTime;
        else
            currentShield = Mathf.MoveTowards(currentShield, playerShipData.GetShipShield(), Time.deltaTime * playerShipData.GetShieldRecoveryRate());
    }
    public void IncreaseWeaponHeat(float amount) {
        tmpWeaponHeat += amount;
        tmpHeatRecoveryStrenght = 0;
        if (tmpWeaponHeat > 100) {
            tmpWeaponHeat = 100;
            overheat = true;
        }
    }
    public void UseSpecial() {
        if (SpecialReady()) {
            specialInUse.ActivateEffect(this);
            tmpSpecialReady = 0;
        }
    }
    #region Shooting Methods
    public void Shoot(float angle)
    {
        if (tmpShootReady < 1 || overheat)
            return;
        tmpShootReady = 0;

        lastShootAngle = angle;

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
    private void ShootNormal(float angle)
    {
        if (GetMultishoot() > 1)
        {
            float tmpAngle = GetMultishootArc() / 2f;
            int t = 0;
            while (t < GetMultishoot())
            {

                CreateAttackBasedOnWeaponType(angle, tmpAngle);
                tmpAngle -= GetMultishootArc() / (GetMultishoot() - 1);
                t++;
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(angle, 0);

        }
        IncreaseWeaponHeat(GetHeatPerProjectile() * GetMultishoot());
    }
    IEnumerator ShootBarrageStandard()
    {
        int count = 0;
        float baseShootAngle = lastShootAngle;
        while (count < 5)
        {
            ShootNormal(baseShootAngle);
            IncreaseWeaponHeat(GetHeatPerProjectile());
            count++;
            yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
        }
    }
    IEnumerator ShootBarrage1Way()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle = GetMultishootArc() / 2f;
        int t = 0;

        if (GetMultishoot() > 1)
        {

            while (t < GetMultishoot())
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle);
                IncreaseWeaponHeat(GetHeatPerProjectile());
                tmpAngle -= GetMultishootArc() / (GetMultishoot() - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle, 0);
            IncreaseWeaponHeat(GetHeatPerProjectile());
        }
    }
    IEnumerator ShootBarrage2Way()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle = GetMultishootArc() / 2f;
        int t = 0;

        if (GetMultishoot() > 1)
        {

            while (t < GetMultishoot())
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle);
                IncreaseWeaponHeat(GetHeatPerProjectile());
                tmpAngle -= GetMultishootArc() / (GetMultishoot() - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }

            t--;
            tmpAngle += GetMultishootArc() / (GetMultishoot() - 1);

            while (t >= 0)
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle);
                IncreaseWeaponHeat(GetHeatPerProjectile());
                tmpAngle += GetMultishootArc() / (GetMultishoot() - 1);
                t--;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle, 0);
            IncreaseWeaponHeat(GetHeatPerProjectile());

        }
    }
    IEnumerator ShootBarrageCrossfull()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle1 = GetMultishootArc() / 2f;
        float tmpAngle2 = -GetMultishootArc() / 2f;
        int i = 0;

        if (GetMultishoot() > 1)
        {

            while (i < GetMultishoot())
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle1);
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle2);
                IncreaseWeaponHeat(GetHeatPerProjectile() * 2);

                tmpAngle1 -= GetMultishootArc() / (GetMultishoot() - 1);
                tmpAngle2 += GetMultishootArc() / (GetMultishoot() - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle, 0);
            IncreaseWeaponHeat(GetHeatPerProjectile());
        }
    }
    IEnumerator ShootBarrageCrosshalf()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle1 = GetMultishootArc() / 2f;
        float tmpAngle2 = -GetMultishootArc() / 2f;
        int i = 0;

        if (GetMultishoot() > 1)
        {

            while (i < (GetMultishoot() / 2))
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle1);
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle2);
                IncreaseWeaponHeat(GetHeatPerProjectile() * 2);

                tmpAngle1 -= GetMultishootArc() / (GetMultishoot() - 1);
                tmpAngle2 += GetMultishootArc() / (GetMultishoot() - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
            if (GetMultishoot() % 2 != 0)
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, 0);
                IncreaseWeaponHeat(GetHeatPerProjectile());
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle, 0);
            IncreaseWeaponHeat(GetHeatPerProjectile());
        }
    }
    private void CreateAttackBasedOnWeaponType(float degree, float local)
    {
        switch (GetWeaponAttackType())
        {
            case WeaponData.AttackType.beam:
                {
                    CreateBeam(degree, local);
                    break;
                }
            case WeaponData.AttackType.projectile:
                {
                    CreateBullet(degree, local);
                    break;
                }
        }
    }
    private void CreateBullet(float degree, float local)
    {
        BulletBehaviour bullet;
        bullet = ObjectPool.currentInstance.GetBulletFromPool();
        if (GetWeaponRandomSpread() > 0) bullet.SetBullet(transform.position, degree + Random.Range(-GetWeaponRandomSpread(), GetWeaponRandomSpread()), local, this);
        else bullet.SetBullet(transform.position, degree, local, this);
    }
    private void CreateBeam(float degree, float local)
    {
        BeamBehaviour beam;
        beam = ObjectPool.currentInstance.GetBeamFromPool();
        if (GetWeaponRandomSpread() > 0) beam.SetBeam(this, transform.position, GetBulletBounces(), degree + Random.Range(-GetWeaponRandomSpread(), GetWeaponRandomSpread()), entityHitbox);
        else beam.SetBeam(this, transform.position, GetBulletBounces(), degree, entityHitbox);
    }
    #endregion
    #region Interface implementation and Getters
    public void DealDamage(float amount, Enums.DamageType dmgType, IEntity damageDealer)
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
            case Enums.DamageType.nuclearDamage:
                {
                    SubstractHealthAndShield(amount, true);
                    break;
                }
            case Enums.DamageType.electricDamage:
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
    public int GetBulletPiercing()
    {
        return playerWeaponData.GetPiercing();
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
    public float GetPlasmaDamage() {
        return playerWeaponData.GetPlasmaDamage();
    }
    public float GetGammaDamage() {
        return playerWeaponData.GetGammaDamage();
    }
    public int GetMultishoot()
    {
        return playerWeaponData.GetMultishoot();
    }
    public float GetMultishootArc()
    {
        return playerWeaponData.GetMultishootArc();
    }
    public WeaponData.ProjectileTrajectory GetWeaponProjectileTrajectory() {
        return playerWeaponData.GetWeaponProjectileTrajectory();
    }
    public float GetWeaponRandomSpread()
    {
        return playerWeaponData.GetRandomSpread();
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
        StageManager.currentInstance.EndStage(false);
        currentHealth = 0;
    }

    public float GetEntityTimescale()
    {
        return 1;
    }

    public void ResetEntity()
    {
        // TEST ONLY
        playerWeaponData = GlobalGameManager.currentInstance.GetPlayerSelectedWeapon();
        playerShipData = GlobalGameManager.currentInstance.GetPlayerSelectedShip(); 

        StageManager.currentInstance.RegisterPlayerEntity(this);

        currentHealth = playerShipData.GetShipEnergy();
        currentShield = playerShipData.GetShipShield();

        InstantiateSpecial();
        
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
    public int GetCurrentHealth() {
        return (int)currentHealth;
    }
    public int GetCurrentShield() {
        return (int)currentShield;
    }
    public WeaponData.DamageElement GetDamageElement() {
        return playerWeaponData.GetWeaponElement();
    }
    public float GetCritChance()
    {
        return playerWeaponData.GetCritChance();
    }
    public float GetCritMultiplier()
    {
        return playerWeaponData.GetCritMultiplier();
    }
    #endregion
    #region Player Entity Getters
    public float GetWeaponHeat() {
        return tmpWeaponHeat;
    }
    public float GetSpecialCharge() {
        return tmpSpecialReady;
    }
    public float GetShieldRecoveryPercent()
    {
        return (playerShipData.GetShieldRecoveryDelay() - shieldRecoverReady) / playerShipData.GetShieldRecoveryDelay();
    }
    public float GetHeatPerProjectile() {
        return playerWeaponData.GetHeatPerProjectile();
    }
    public bool CanShoot() {
        return !overheat;
    }
    public bool IsOverheated() {
        return overheat;
    }
    public bool SpecialReady() {
        return tmpSpecialReady >= 100;
    }
    #endregion
    #region Specials Instantiation
    public void InstantiateSpecial() {
        if (specialInUse != null)
            Destroy(specialInUse.GetGameObject());

        GameObject instSpecial;

        switch (playerShipData.GetSpecial()) {
            default:
                {
                    instSpecial = Instantiate(PrefabManager.currentInstance.special_deflectPulse) as GameObject;
                    break;
                }
        }
        instSpecial.SetActive(false);
        specialInUse = instSpecial.GetComponent<ISpecial>();
    }
    #endregion
}
