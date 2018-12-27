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
	private float weapon_trajectoryHelix;
	private float weapon_trajectoryTrack;
	private float weapon_trajectoryWave;
	private float weapon_randomSpread;
	private float weapon_multishootAperture;
    private float weapon_damage_physical;
    private float weapon_damage_cryo;
    private float weapon_damage_photon;
    private float weapon_damage_electric;
    private float weapon_damage_nuclear;
	private int weapon_multishoot;
	private int weapon_bounces;
	private ShootSecuence weapon_shootSecuence;
	private AttackType weapon_attackType;
	public static float WEAPON_BASE_DAMAGE = 100;

	public WeaponData (WeaponGenerationSetting stg)
	{
        weapon_name = "test_weapon";
        weapon_firerate = 0.8f;
        weapon_effectiveness = 1;
        weapon_critChance = 20;
        weapon_critMultiplier = 2;
        weapon_trajectoryHelix = 0f;
        weapon_trajectoryWave = 0;
        weapon_trajectoryTrack = 2f;
        weapon_randomSpread = 0;
        weapon_multishootAperture = 50;
        weapon_multishoot = 8;
        weapon_bounces = 4;
        weapon_damage_physical = 0;
        weapon_damage_photon = 0;
        weapon_damage_cryo = 65;
        weapon_damage_electric = 0;
        weapon_damage_nuclear = 0;
        weapon_projectileSpeed = 0.5f;
        weapon_shootSecuence = ShootSecuence.normal;
        weapon_attackType = AttackType.projectile;
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
	public float GetTrajectoryHelix() {
		return weapon_trajectoryHelix;
	}
	public float GetTrajectoryTracking() {
		return weapon_trajectoryTrack;
	}	
	public float GetTrajectoryWave() {
		return weapon_trajectoryWave;
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
    public float GetPhysicalDamage() {
        return weapon_damage_physical;
    }
    public float GetNuclearDamage() {
        return weapon_damage_nuclear;
    }
    public float GetPhotonDamage() {
        return weapon_damage_photon;
    }
    public float GetElectricDamage() {
        return weapon_damage_electric;
    }
    public float GetCryoDamage() {
        return weapon_damage_cryo;
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
