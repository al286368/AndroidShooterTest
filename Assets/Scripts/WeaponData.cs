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
	private float weapon_effectiveness;
	private float weapon_critChance;
	private float weapon_critMultiplier;
	private float weapon_trajectoryHelix;
	private float weapon_trajectoryTrack;
	private float weapon_trajectoryArc;
	private float weapon_randomSpread;
	private float weapon_multishootAperture;
	private int weapon_multishoot;
	private int weapon_bounces;
	private ShootSecuence weapon_shootSecuence;
	private DamageElement weapon_baseElement;
	private DamageElement weapon_secondaryElement;
	private AttackType weapon_attackType;
	public static float WEAPON_BASE_DAMAGE = 100;

	public WeaponData (WeaponGenerationSetting stg)
	{
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
	public float GetTrajectoryArc() {
		return weapon_trajectoryArc;
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
	public ShootSecuence GetWeaponShootSecuence() { 
		return weapon_shootSecuence;
	}
	public DamageElement GetWeaponBaseElement() { 
		return weapon_baseElement;
	}
	public AttackType GetAttackType() { 
		return weapon_attackType; 
	}
	#endregion
} 
