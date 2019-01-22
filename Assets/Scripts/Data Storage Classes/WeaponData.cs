using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData {

	public enum WeaponGenerationSetting 
	{
		playerWeapon, proceduralEliteWeapon, proceduralBossWeapon, proceduralSpecialbossWeapon
	}
	public enum ShootSecuence
	{
		normal, barrage_standard, barrage_1way, barrage_2way, barrage_crosshalf, barrage_crossful
	}
    public enum ProjectileTrajectory
    {
        normal, helix, tracking, binarytrack, delayedTracking, wave
    }
    public enum DamageElement
	{
		physical, photon, nuclear, cryo, electric
	}
	public enum AttackType
	{
		projectile, beam
	}

	private string weapon_name;
	private float weapon_firerate;
    private float weapon_projectileSpeed;
	private float weapon_effectiveness;
	private float weapon_critChance;
	private float weapon_critMultiplier;
	private float weapon_randomSpread;
	private float weapon_multishootAperture;
    private float weapon_damage;
    private float weapon_heat_per_projectile;
	private int weapon_multishoot;
	private int weapon_bounces;
	private ShootSecuence weapon_shootSecuence;
	private AttackType weapon_attackType;
    private DamageElement weapon_element;
    private ProjectileTrajectory weapon_projectile_trajectory;
	public static float WEAPON_BASE_DAMAGE = 100;

	public WeaponData (WeaponGenerationSetting stg)
	{
        weapon_name = "test_weapon";
        weapon_firerate = 0.8f;
        weapon_effectiveness = 1;
        weapon_critChance = 20;
        weapon_critMultiplier = 2;
        weapon_randomSpread = 0;
        weapon_multishootAperture = 130;
        weapon_multishoot = 6;
        weapon_bounces = 0;
        weapon_damage = 80;
        weapon_projectileSpeed = 1f;
        weapon_heat_per_projectile = 2.8f;
        weapon_shootSecuence = ShootSecuence.normal;
        weapon_attackType = AttackType.projectile;
        weapon_element = DamageElement.nuclear;
        weapon_projectile_trajectory = ProjectileTrajectory.helix;

		switch (stg) {
		case WeaponGenerationSetting.playerWeapon:
            {
				break;
			}
		case WeaponGenerationSetting.proceduralBossWeapon:
			{
				break;
			}
		case WeaponGenerationSetting.proceduralEliteWeapon:
			{
				break;
			}
		case WeaponGenerationSetting.proceduralSpecialbossWeapon:
			{
				break;
			}
		}
	}

    #region Getters
    public DamageElement GetWeaponElement() {
        return weapon_element;
    }
	public string GetWeaponName() {
		return weapon_name;
	}
	public float GetFirerate() {
		return weapon_firerate;
	}
	public float GetEffectiveness() {
		return weapon_effectiveness;
	}
	public float GetCritChance() {
		return weapon_critChance;
	}
	public float GetCritMultiplier() {
		return weapon_critMultiplier;
	}
    public ProjectileTrajectory GetWeaponProjectileTrajectory() {
        return weapon_projectile_trajectory;
    }
	public float GetRandomSpread() {
		return weapon_randomSpread;
	}
	public float GetMultishootArc() {
		return weapon_multishootAperture;
	}
	public int GetMultishoot() {
		return weapon_multishoot;
	}
	public int GetBounces() { 
		return weapon_bounces;
	}
    public float GetHeatPerProjectile() {
        return weapon_heat_per_projectile;
    }
    public float GetPhysicalDamage() {
        if (weapon_element == DamageElement.physical)
            return weapon_damage;
        else
            return 0;
    }
    public float GetNuclearDamage() {
        if (weapon_element == DamageElement.nuclear)
            return weapon_damage;
        else
            return 0;
    }
    public float GetPhotonDamage() {
        if (weapon_element == DamageElement.photon)
            return weapon_damage;
        else
            return 0;
    }
    public float GetElectricDamage() {
        if (weapon_element == DamageElement.electric)
            return weapon_damage;
        else
            return 0;
    }
    public float GetCryoDamage() {
        if (weapon_element == DamageElement.cryo)
            return weapon_damage;
        else
            return 0;
    }
    public float GetProjectileSpeed() {
        return weapon_projectileSpeed;
    }
	public ShootSecuence GetWeaponShootSecuence() { 
		return weapon_shootSecuence;
	}
	public AttackType GetAttackType() { 
		return weapon_attackType; 
	}
	#endregion
} 
