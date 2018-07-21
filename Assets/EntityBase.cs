using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour {

    public enum WeaponAttackType { projectile, beam, missile }
    public enum WeaponElement { physical, photon, nuclear, cryo, electro }
    public enum WeaponShotSecuenceType { normal, barrage_standard, barrage_1way, barrage_2way, barrage_crosshalf, barrage_crossfull }

    [Header("Defensive parameters")]
    public float stat_health;
    public float stat_shield;
    public float stat_defense;
    [Header("Weapon parameters")]
    public float stat_damage;
    public float stat_critChance;
    public float stat_critMultiplier;
    public float stat_firerate;
    public int stat_bounces;
    public int weapon_multishoot;
    public float weapon_multishootArc;
    public float weapon_randomSpread;
    public WeaponAttackType weapon_attackType;
    public WeaponElement weapon_element;
    public WeaponShotSecuenceType weapon_shootSecuenceType;

    private const float SHOOT_SECUENCE_DELAY = 0.05f;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();
	}
    public void Shoot()
    {
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
        if (weapon_randomSpread > 0) bullet.SetBullet(this, transform.position, degree + Random.Range(-weapon_randomSpread, weapon_randomSpread));
        else bullet.SetBullet(this, transform.position, degree);
    }
    private void CreateBeam(float degree)
    {
        BeamBehaviour beam;
        beam = ObjectPool.currentInstance.GetBeamFromPool();
        if (weapon_randomSpread > 0) beam.SetShape(transform.position, stat_bounces, degree + Random.Range(-weapon_randomSpread, weapon_randomSpread), 0.2f);
        else beam.SetShape(transform.position, stat_bounces, degree, 0.2f);
    }
}
