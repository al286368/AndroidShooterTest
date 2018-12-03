﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour {

    public enum WeaponAttackType { projectile, beam, missile }
    public enum WeaponElement { physical, photon, nuclear, cryo, electro }
    public enum WeaponShotSecuenceType { normal, barrage_standard, barrage_1way, barrage_2way, barrage_crosshalf, barrage_crossfull }


    [Header("General flags")]
    public bool isPlayer;
    public bool isAlly;
    public bool isBoss;
    public bool canBeDamaged;
    public bool registredToStage;
    [Header("Temporal Stage Data")]
    public float currentHealth;
    public float currentShield;
    public float status_frozen;
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
    public float stat_firerate;
    public float stat_trajectoryTrack;
    public float stat_trajectoryHelix;
    public float stat_trajectoryArc;
    public int stat_bounces;
    public int weapon_multishoot;
    public float weapon_multishootArc;
    public float weapon_randomSpread;
    public WeaponAttackType weapon_attackType;
    public WeaponElement weapon_element;
    public WeaponShotSecuenceType weapon_shootSecuenceType;
    [Header("Other")]
    public Collider2D entity_hitbox;
    public IEnemyAI enemyAI;

    private float shootReady = 1;

    private const float SHOOT_SECUENCE_DELAY = 0.05f;
    private bool coRoutine_burning = false;


	// Use this for initialization
	void Start () {
        enemyAI = GetComponent<IEnemyAI>();
        if (!isAlly) {
            StageManager.currentInstance.RegisterEnemy(this);
        }
        else if (isPlayer) {
            StageManager.currentInstance.RegisterPlayerEntity(this);
        }
        if (enemyAI != null)
        {
            enemyAI.ResetAI();
        }
	}
	
	// Update is called once per frame
	void Update () {
        shootReady += Time.deltaTime * stat_firerate;
	}
    public void Shoot()
    {
        if (shootReady < 1)
            return;
        shootReady = 0;

        switch (weapon_shootSecuenceType)
        {
            case WeaponShotSecuenceType.normal:
                {
                    ShootNormal();
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
    private void ShootNormal()
    {
        if (weapon_multishoot > 1)
        {
            float tmpAngle = 90 + (weapon_multishootArc / 2f);
            int t = 0;
            while (t < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(tmpAngle);
                tmpAngle -= weapon_multishootArc / (weapon_multishoot-1);
                t++;
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(90);

        }

        
    }
    IEnumerator ShootBarrageStandard()
    {
        int count = 0;
        while (count < 5)
        {
            ShootNormal();
            count++;
            yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
        }
    }
    IEnumerator ShootBarrage1Way()
    {
        float tmpAngle = 90 + (weapon_multishootArc / 2f);
        int t = 0;

        if (weapon_multishoot > 1)
        {

            while (t < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(tmpAngle);
                tmpAngle -= weapon_multishootArc / (weapon_multishoot - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(90);

        }
    }
    IEnumerator ShootBarrage2Way()
    {
        float tmpAngle = 90 + (weapon_multishootArc / 2f);
        int t = 0;

        if (weapon_multishoot > 1)
        {

            while (t < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(tmpAngle);
                tmpAngle -= weapon_multishootArc / (weapon_multishoot - 1);
                t++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }

            t--;
            tmpAngle += weapon_multishootArc / (weapon_multishoot - 1);

            while (t >= 0)
            {
                CreateAttackBasedOnWeaponType(tmpAngle);
                tmpAngle += weapon_multishootArc / (weapon_multishoot - 1);
                t--;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(90);

        }
    }
    IEnumerator ShootBarrageCrossfull()
    {
        float tmpAngle1 = 90 + (weapon_multishootArc / 2f);
        float tmpAngle2 = 90 - (weapon_multishootArc / 2f);
        int i = 0;

        if (weapon_multishoot > 1)
        {

            while (i < weapon_multishoot)
            {
                CreateAttackBasedOnWeaponType(tmpAngle1);
                CreateAttackBasedOnWeaponType(tmpAngle2);

                tmpAngle1 -= weapon_multishootArc / (weapon_multishoot - 1);
                tmpAngle2 += weapon_multishootArc / (weapon_multishoot - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(90);
        }
    }
    IEnumerator ShootBarrageCrosshalf()
    {
        float tmpAngle1 = 90 + (weapon_multishootArc / 2f);
        float tmpAngle2 = 90 - (weapon_multishootArc / 2f);
        int i = 0;

        if (weapon_multishoot > 1)
        {

            while (i < (weapon_multishoot/2))
            {
                CreateAttackBasedOnWeaponType(tmpAngle1);
                CreateAttackBasedOnWeaponType(tmpAngle2);

                tmpAngle1 -= weapon_multishootArc / (weapon_multishoot - 1);
                tmpAngle2 += weapon_multishootArc / (weapon_multishoot - 1);
                i++;
                yield return new WaitForSeconds(SHOOT_SECUENCE_DELAY);
            }
        }
        else
        {
            CreateAttackBasedOnWeaponType(90);
        }
    }
    private void CreateAttackBasedOnWeaponType(float degree)
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
                    CreateBullet(degree);
                    break;
                }
        }
    }
    private void CreateBullet(float degree)
    {
        BulletBehaviour bullet;
        bullet = ObjectPool.currentInstance.GetBulletFromPool();
        if (weapon_randomSpread > 0) bullet.SetBullet(transform.position, degree + Random.Range(-weapon_randomSpread, weapon_randomSpread), this);
        else bullet.SetBullet(transform.position, degree, this);
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
        if (amount <= 0)
            return;
        if (amount < 1) amount = 1;

        switch (dmgType)
        {
            case Enums.DamageType.normal:
                {
                    IngameHudManager.currentInstance.DisplayNotification(amount, Enums.DamageType.normal, transform.position);
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
                    IngameHudManager.currentInstance.DisplayNotification(amount, Enums.DamageType.cryo, transform.position);
                    SubstractHealthAndShield(amount, false);
                    status_frozen += amount;
                    break;
                }
            case Enums.DamageType.nuclear:
                {
                    IngameHudManager.currentInstance.DisplayNotification(amount, Enums.DamageType.nuclear, transform.position);
                    SubstractHealthAndShield(amount, true);
                    break;
                }
            case Enums.DamageType.electric:
                {
                    IngameHudManager.currentInstance.DisplayNotification(amount, Enums.DamageType.electric, transform.position);
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
            IngameHudManager.currentInstance.DisplayNotification(burnAmount, Enums.DamageType.photon, transform.position);
            SubstractHealthAndShield(burnAmount, false);
            yield return new WaitForSeconds(0.5f);
        }
        coRoutine_burning = false;
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
                currentHealth -= currentShield;
                currentShield = 0;
            }
        }

        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }

    }
}