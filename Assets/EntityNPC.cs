using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityNPC : MonoBehaviour, IEntity {

    public enum WeaponAttackType { projectile, beam, missile }
    public enum WeaponElement { physical, photon, nuclear, cryo, electro }
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
    public float stat_trajectoryTrack;
    public float stat_trajectoryHelix;
    public float stat_trajectoryWave;
    public float stat_bulletSpeed;
    public int stat_bounces;
    public int weapon_multishoot;
    public float weapon_multishootArc;
    public float weapon_randomSpread;
    public WeaponAttackType weapon_attackType;
    public WeaponElement weapon_element;
    public WeaponShotSecuenceType weapon_shootSecuenceType;
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


    void Update()
    {
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
    private int GetLrForIndex(int index)
    {
        if (weapon_multishoot == 1)
            return 0;
        if (weapon_multishoot % 2 != 0 && index + 1 == (weapon_multishoot / 2) + 1)
            return 0;
        else if (index <weapon_multishoot / 2f)
            return -1;
        else
            return 1;
    }
    private void ShootNormal(float angle)
    {
        if (weapon_multishoot > 1)
        {
            float tmpAngle = angle + (weapon_multishootArc / 2f);
            int t = 0;
            while (t < weapon_multishoot)
            {

                CreateAttackBasedOnWeaponType(tmpAngle, GetLrForIndex(t));
                tmpAngle -= weapon_multishootArc / (weapon_multishoot - 1);
                t++;
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(angle);

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
        float tmpAngle = baseShootAngle + (weapon_multishootArc / 2f);
        int t = 0;

        if (weapon_multishoot > 1)
        {

            while (t < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(tmpAngle, GetLrForIndex(t));
                tmpAngle -= weapon_multishootArc / (weapon_multishoot - 1);
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
        float tmpAngle = baseShootAngle + (weapon_multishootArc / 2f);
        int t = 0;

        if (weapon_multishoot > 1)
        {

            while (t < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(tmpAngle, GetLrForIndex(t));
                tmpAngle -= weapon_multishootArc / (weapon_multishoot - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }

            t--;
            tmpAngle += weapon_multishootArc / (weapon_multishoot - 1);

            while (t >= 0)
            {
                CreateAttackBasedOnWeaponType(tmpAngle, GetLrForIndex(t));
                tmpAngle += weapon_multishootArc / (weapon_multishoot - 1);
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
        float tmpAngle1 = baseShootAngle + (weapon_multishootArc / 2f);
        float tmpAngle2 = baseShootAngle - (weapon_multishootArc / 2f);
        int i = 0;

        if (weapon_multishoot > 1)
        {

            while (i < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(tmpAngle1, GetLrForIndex(i));
                CreateAttackBasedOnWeaponType(tmpAngle2, -GetLrForIndex(i));

                tmpAngle1 -= weapon_multishootArc / (weapon_multishoot - 1);
                tmpAngle2 += weapon_multishootArc / (weapon_multishoot - 1);
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
        float tmpAngle1 = baseShootAngle + (weapon_multishootArc / 2f);
        float tmpAngle2 = baseShootAngle - (weapon_multishootArc / 2f);
        int i = 0;

        if (weapon_multishoot > 1)
        {

            while (i < (weapon_multishoot / 2))
            {
                CreateAttackBasedOnWeaponType(tmpAngle1, GetLrForIndex(i));
                CreateAttackBasedOnWeaponType(tmpAngle2, -GetLrForIndex(i));

                tmpAngle1 -= weapon_multishootArc / (weapon_multishoot - 1);
                tmpAngle2 += weapon_multishootArc / (weapon_multishoot - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
            if (weapon_multishoot % 2 != 0) {
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
        switch (weapon_attackType)
        {
            case WeaponAttackType.beam:
                {
                    CreateBeam(degree);
                    break;
                }
            case WeaponAttackType.projectile:
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
        if (weapon_randomSpread > 0) bullet.SetBullet(transform.position, degree + Random.Range(-weapon_randomSpread, weapon_randomSpread), this, lr);
        else bullet.SetBullet(transform.position, degree, this, lr);
    }
    private void CreateBeam(float degree)
    {
        BeamBehaviour beam;
        beam = ObjectPool.currentInstance.GetBeamFromPool();
        if (weapon_randomSpread > 0) beam.SetBeam(this, transform.position, stat_bounces, degree + Random.Range(-weapon_randomSpread, weapon_randomSpread), entity_hitbox);
        else beam.SetBeam(this, transform.position, stat_bounces, degree, entity_hitbox);
    }
    public void DealDamage(float amount, Enums.DamageType dmgType)
    {
        amount *= (1 - stat_defense);
        if (amount <= 0)
            return;
        if (amount < 1) amount = 1;
        entityAI.NotifyDamageTaken(amount);

        switch (dmgType)
        {
            case Enums.DamageType.normal:
                {
                    IngameHudManager.currentInstance.DisplayDamageNotification(amount, Enums.DamageType.normal, transform.position);
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
                    IngameHudManager.currentInstance.DisplayDamageNotification(amount, Enums.DamageType.cryo, transform.position);
                    if (status_frozen < 100) {
                        status_frozen += amount * DAMAGE_TO_FROZEN_STATUS_RATE;
                        if (status_frozen > 100) {
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
            case Enums.DamageType.nuclear:
                {
                    IngameHudManager.currentInstance.DisplayDamageNotification(amount, Enums.DamageType.nuclear, transform.position);
                    SubstractHealthAndShield(amount, true);
                    break;
                }
            case Enums.DamageType.electric:
                {
                    IngameHudManager.currentInstance.DisplayDamageNotification(amount, Enums.DamageType.electric, transform.position);
                    SubstractHealthAndShield(amount, false);
                    break;
                }
        }
    }
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
            IngameHudManager.currentInstance.DisplayDamageNotification(burnAmount, Enums.DamageType.photon, transform.position);
            SubstractHealthAndShield(burnAmount, false);
            yield return new WaitForSeconds(0.5f);
        }
        coRoutine_burning = false;
    }
    IEnumerator FrozenStatus() {
        coRoutine_frozen = true;
        float breakTimer = 0;
        while (status_frozen > 0)
        {
            if (status_frozen >= 100) {
                breakTimer = 5;
                stat_defense -= 1;
                while (breakTimer > 0) {
                    breakTimer -= Time.deltaTime;
                    yield return null;
                }
                stat_defense += 1;
                status_frozen = 0;
            }
            else {
                status_frozenRecoveryRate += Time.deltaTime * 2;
                status_frozen -= Time.deltaTime * status_frozenRecoveryRate;
                if (status_frozen < 0)
                    status_frozen = 0;
            }
            yield return null;
        }
        coRoutine_frozen = false;
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
    #region Interface Implementation
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
    public float GetTrajectoryHelix() {
        return stat_trajectoryHelix;
    }
    public float GetTrajectoryWave() {
        return stat_trajectoryWave;
    }
    public float GetTrajectoryTracking() {
        return stat_trajectoryTrack;
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
    #endregion
}
