using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityNPC : MonoBehaviour, IEntity {

    public enum WeaponAttackType { projectile, beam, missile }
    public enum WeaponShotSecuenceType { normal, barrage_standard, barrage_1way, barrage_2way, barrage_crosshalf, barrage_crossfull }
    public enum RecycleTag { drifter, bomber, sniper }

    [Header("General flags")]

    public RecycleTag recycleTag;
    public bool isAlly;
    public bool isBoss;
    public bool canBeDamaged;
    public bool canBeDesotryed;
    public bool registredToStage;
    [Header("Temporal Stage Data")]
    public float currentHealth;
    public float currentShield;
    public float status_frozen;
    public float status_frozenRecoveryRate;
    public float status_burning;
    [Header("Defensive parameters")]
    public float stat_health;
    public float stat_shield;
    public float stat_defense;
    [Header("Weapon parameters")]
    public float stat_damage_physical;
    public float stat_damage_cryo;
    public float stat_damage_photon;
    public float stat_damage_electric;
    public float stat_damage_nuclear;
    public float stat_effectiveness;
    public float stat_critChance;
    public float stat_critMultiplier;
    public float stat_bulletSpeed;
    public int stat_bounces;
    public int weapon_multishoot;
    public float weapon_multishootArc;
    public float weapon_randomSpread;
    public WeaponAttackType weapon_attackType;
    public WeaponData.DamageElement weapon_element;
    public WeaponShotSecuenceType weapon_shootSecuenceType;
    public WeaponData.ProjectileTrajectory weapon_trajectory;
    [Header("Other")]
    public Collider2D entity_hitbox;
    public IEnemyAI entityAI;

    private float lastShootAngle = 0;

    private const float DAMAGE_TO_FROZEN_STATUS_RATE = 0.15f;
    private const float SHOOT_SECUENCE_DELAY = 0.2f;
    private bool coRoutine_burning = false;
    private bool coRoutine_frozen = false;


    void Start()
    {
        ResetEntity();
 
    }
    public void ResetEntity() {
        entityAI = GetComponent<IEnemyAI>();
        if (entityAI != null)
            entityAI.ResetAI();
        if (!isAlly)
            StageManager.currentInstance.RegisterEnemy(this);

        currentHealth = stat_health;
        currentShield = stat_shield;
        status_frozen = 0;
        status_burning = 0;
        status_frozenRecoveryRate = 0;
    }
    #region Status Coroutines
    IEnumerator FireDamageOverTime()
    {
        coRoutine_burning = true;
        float burnAmount;

        while (status_burning > 0)
        {
            burnAmount = (int)(status_burning / 10);
            if (burnAmount < 1)
                burnAmount = 1;
            status_burning -= burnAmount;
            IngameHudManager.currentInstance.DisplayDamageNotification(burnAmount,false, Enums.DamageType.photon, transform.position);
            SubstractHealthAndShield(burnAmount, false);
            yield return new WaitForSeconds(0.5f);
        }
        coRoutine_burning = false;
    }
    IEnumerator FrozenStatus()
    {
        coRoutine_frozen = true;
        float breakTimer = 0;
        while (status_frozen > 0)
        {
            if (status_frozen >= 100)
            {
                breakTimer = 5;
                stat_defense -= 1;
                while (breakTimer > 0)
                {
                    breakTimer -= Time.deltaTime;
                    yield return null;
                }
                stat_defense += 1;
                status_frozen = 0;
            }
            else
            {
                status_frozenRecoveryRate += Time.deltaTime * 2;
                status_frozen -= Time.deltaTime * status_frozenRecoveryRate;
                if (status_frozen < 0)
                    status_frozen = 0;
            }
            yield return null;
        }
        coRoutine_frozen = false;
    }
    #endregion
    #region Shooting Functions
    public void Shoot(float angle)
    {
        lastShootAngle = angle;

        switch (weapon_shootSecuenceType)
        {
            case WeaponShotSecuenceType.normal:
                {
                    ShootNormal(angle);
                    break;
                }
            case WeaponShotSecuenceType.barrage_standard:
                {
                    StartCoroutine("ShootBarrageStandard");
                    break;
                }
            case WeaponShotSecuenceType.barrage_1way:
                {
                    StartCoroutine("ShootBarrage1Way");
                    break;
                }
            case WeaponShotSecuenceType.barrage_2way:
                {
                    StartCoroutine("ShootBarrage2Way");
                    break;
                }
            case WeaponShotSecuenceType.barrage_crossfull:
                {
                    StartCoroutine("ShootBarrageCrossfull");
                    break;
                }
            case WeaponShotSecuenceType.barrage_crosshalf:
                {
                    StartCoroutine("ShootBarrageCrosshalf");
                    break;
                }
        }
    }
    private void ShootNormal(float angle)
    {
        if (weapon_multishoot > 1)
        {
            float tmpAngle = weapon_multishootArc / 2f;
            int t = 0;
            while (t < weapon_multishoot)
            {

                CreateAttackBasedOnWeaponType(angle, tmpAngle);
                tmpAngle -= weapon_multishootArc / (weapon_multishoot - 1);
                t++;
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(angle, 0);

        }


    }
    IEnumerator ShootBarrageStandard()
    {
        int count = 0;
        float baseShootAngle = lastShootAngle;
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
        float tmpAngle = weapon_multishootArc / 2f;
        int t = 0;

        if (weapon_multishoot > 1)
        {

            while (t < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle);
                tmpAngle -= weapon_multishootArc / (weapon_multishoot - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle, 0);

        }
    }
    IEnumerator ShootBarrage2Way()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle = weapon_multishootArc / 2f;
        int t = 0;

        if (weapon_multishoot > 1)
        {

            while (t < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle);
                tmpAngle -= weapon_multishootArc / (weapon_multishoot - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }

            t--;
            tmpAngle += weapon_multishootArc / (weapon_multishoot - 1);

            while (t >= 0)
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle);
                tmpAngle += weapon_multishootArc / (weapon_multishoot - 1);
                t--;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle, 0);

        }
    }
    IEnumerator ShootBarrageCrossfull()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle1 = weapon_multishootArc / 2f;
        float tmpAngle2 = -weapon_multishootArc / 2f;
        int i = 0;

        if (weapon_multishoot > 1)
        {

            while (i < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle1);
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle2);

                tmpAngle1 -= weapon_multishootArc / (weapon_multishoot - 1);
                tmpAngle2 += weapon_multishootArc / (weapon_multishoot - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle, 0);
        }
    }
    IEnumerator ShootBarrageCrosshalf()
    {
        float baseShootAngle = lastShootAngle;
        float tmpAngle1 = weapon_multishootArc / 2f;
        float tmpAngle2 = -weapon_multishootArc / 2f;
        int i = 0;

        if (weapon_multishoot > 1)
        {

            while (i < (weapon_multishoot / 2))
            {
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle1);
                CreateAttackBasedOnWeaponType(baseShootAngle, tmpAngle2);

                tmpAngle1 -= weapon_multishootArc / (weapon_multishoot - 1);
                tmpAngle2 += weapon_multishootArc / (weapon_multishoot - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
            if (weapon_multishoot % 2 != 0) {
                CreateAttackBasedOnWeaponType(baseShootAngle, 0);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(baseShootAngle, 0);
        }
    }
    private void CreateAttackBasedOnWeaponType(float degree, float local)
    {
        switch (weapon_attackType)
        {
            case WeaponAttackType.beam:
                {
                    CreateBeam(degree, local);
                    break;
                }
            case WeaponAttackType.projectile:
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
        if (weapon_randomSpread > 0) bullet.SetBullet(transform.position, degree + Random.Range(-weapon_randomSpread, weapon_randomSpread), local, this);
        else bullet.SetBullet(transform.position, degree, local, this);
    }
    private void CreateBeam(float degree, float local)
    {
        BeamBehaviour beam;
        beam = ObjectPool.currentInstance.GetBeamFromPool();
        if (weapon_randomSpread > 0) beam.SetBeam(this, transform.position, stat_bounces, degree + Random.Range(-weapon_randomSpread, weapon_randomSpread), entity_hitbox);
        else beam.SetBeam(this, transform.position, stat_bounces, degree, entity_hitbox);
    }
    #endregion
    #region Interface implementation and Getters
    public void DealDamage(float amount, Enums.DamageType dmgType, IEntity dmgDealer)
    {
        amount *= (1 - stat_defense);
        if (amount <= 0 || !gameObject.activeInHierarchy)
            return;

        bool isCrit = false;
        if (Random.Range(1, 101) < dmgDealer.GetCritChance() && dmgType != Enums.DamageType.electricEffect)
        {
            amount *= dmgDealer.GetCritMultiplier();
            isCrit = true;
        }

        if (amount < 1) amount = 1;
        entityAI.NotifyDamageTaken(amount);

        switch (dmgType)
        {
            case Enums.DamageType.electricEffect:
                {
                    ObjectPool.currentInstance.GetSparkFromPool().SetupSpark(transform.position, 4, amount, dmgDealer);
                    break;
                }
            case Enums.DamageType.normal:
                {
                    IngameHudManager.currentInstance.DisplayDamageNotification(amount, isCrit, Enums.DamageType.normal, transform.position);
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
                    IngameHudManager.currentInstance.DisplayDamageNotification(amount, isCrit, Enums.DamageType.cryo, transform.position);
                    if (status_frozen < 100)
                    {
                        status_frozen += amount * DAMAGE_TO_FROZEN_STATUS_RATE;
                        if (status_frozen > 100)
                        {
                            status_frozen = 100;
                            IngameHudManager.currentInstance.DisplayStatusNotification(Enums.StatusEffect.frozen, transform.position);
                        }

                    }
                    status_frozenRecoveryRate = 0;
                    if (!coRoutine_frozen)
                        StartCoroutine("FrozenStatus");
                    SubstractHealthAndShield(amount, false);
                    break;
                }
            case Enums.DamageType.nuclearDamage:
                {
                    IngameHudManager.currentInstance.DisplayDamageNotification(amount, isCrit, Enums.DamageType.nuclearDamage, transform.position);
                    SubstractHealthAndShield(amount, true);
                    break;
                }
            case Enums.DamageType.electricDamage:
                {
                    IngameHudManager.currentInstance.DisplayDamageNotification(amount, isCrit, Enums.DamageType.electricDamage, transform.position);
                    SubstractHealthAndShield(amount, false);
                    break;
                }
        }
    }
    private void SubstractHealthAndShield(float dmg, bool ignoreShield)
    {
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
    public float GetPhysicalDamage() {
        return stat_damage_physical;
    }
    public float GetPhotonDamage() {
        return stat_damage_photon;
    }
    public float GetNuclearDamage() {
        return stat_damage_nuclear;
    }
    public float GetCryoDamage() {
        return stat_damage_cryo;
    }
    public float GetElectricDamage() {
        return stat_damage_electric;
    }
    public int GetBulletBounces() {
        return stat_bounces;
    }
    void DisableEntity() {
        StopAllCoroutines();
        coRoutine_burning = false;
        coRoutine_frozen = false;
        StageManager.currentInstance.UnRegisterEnemy(this);
        gameObject.SetActive(false);
    }
    public void RetreatFromStage() {
        DisableEntity();
    }
    public void Kill() {
        DisableEntity();
    }
    public bool IsAlly() {
        return isAlly;
    }
    public float GetEntityTimescale() {
        return 1 - (status_frozen / 100f);
    }
    public WeaponData.ProjectileTrajectory GetWeaponProjectileTrajectory() {
        return weapon_trajectory;
    }
    public GameObject GetGameObject() {
        return gameObject;
    }
    public float GetBulletSpeedScale() {
        return StageManager.currentInstance.difficultyBulletTimeScaleFactor * stat_bulletSpeed;
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    public float GetHealthPercent()
    {
        return currentHealth / stat_health;
    }
    public WeaponData.DamageElement GetDamageElement() {
        return weapon_element;
    }
    public float GetCritChance()
    {
        return 0;
    }

    public float GetCritMultiplier()
    {
        return 1;
    }
    #endregion
}
