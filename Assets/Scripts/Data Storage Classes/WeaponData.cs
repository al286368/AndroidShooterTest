using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData {

	public enum WeaponGenerationSetting 
	{
		testWeapon, randomWeapon
	}
	public enum ShootSecuence
	{
		normal, barrage_standard, barrage_1way, barrage_2way, barrage_crosshalf, barrage_crossful
	}
    public enum ProjectileTrajectory
    {
        normal, deflected, helix, tracking, binarytrack, delayedTracking, wave
    }
    public enum DamageElement
	{
		pulse, photon, nuclear, cryo, electric, plasma, gamma
	}
	public enum AttackType
	{
		projectile, beam
	}
    public enum Rarity
    {
        common, rare, exotic, hiTech, prototype
    }

	private string weapon_name;
    private int weapon_upgLevel = 1;
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
    private Rarity weapon_rarity;

	public static float WEAPON_BASE_DAMAGE = 100;
    public static float WEAPON_BASE_SCALING_PER_LEVEL = 2.5f;
    public static float WEAPON_BASE_FIRERATE = 5;

    public static float ELEM_PULSE_DMGMULT = 1f;
    public static float ELEM_PHOTON_DMGMULT = 0.3f;
    public static float ELEM_NUCLEAR_DMGMULT = 0.75f;
    public static float ELEM_CRYO_DMGMULT = 0.5f;
    public static float ELEM_ELECTRIC_DMGMULT = 0.3f;
    public static float ELEM_PLASMA_DMGMULT = 0.65f;
    public static float ELEM_GAMMA_DMGMULT = 1.25f;



    public WeaponData (float rewardMultiplier)
	{
        SetDemoStats(rewardMultiplier);
	}
    private void SetDemoStats(float rewardM) {

        weapon_name = "test_weapon";
        weapon_attackType = AttackType.projectile;
        weapon_effectiveness = 1;
        weapon_bounces = 0;
        weapon_projectileSpeed = 1f;

        SetRandomAvailableElement();
        SetRandomRarity(rewardM);
        SetRandomLevel(rewardM);
        SetRandomShootSecuence();
        SetRandomTrajectoryAndMultishoot();
        SetDamage();
        SetDpsStats();
    }
    #region Generation Functions
    public void SetRandomAvailableElement() {
        int variation = Random.Range(1, 7);
        switch (variation)
        {
            case 2:
                {
                    weapon_element = DamageElement.photon;
                    break;
                }
            case 3:
                {
                    weapon_element = DamageElement.nuclear;
                    break;
                }
            case 4:
                {
                    weapon_element = DamageElement.cryo;
                    break;
                }
            case 5:
                {
                    weapon_element = DamageElement.electric;
                    break;
                }
            case 6:
                {
                    weapon_element = DamageElement.plasma;
                    break;
                }
            case 7:
                {
                    weapon_element = DamageElement.gamma;
                    break;
                }
            default:
                {
                    weapon_element = DamageElement.pulse;
                    break;
                }
        }
    }
    public void SetRandomRarity(float rewardQuality) {
        float rarityMultiplier = rewardQuality * 100;
        weapon_rarity = Rarity.common;
        if (Random.Range(1, 101) < rarityMultiplier)
        {
            weapon_rarity = Rarity.rare;
            rarityMultiplier *= 0.5f;
        }
        else { return; }
        if (Random.Range(1, 101) < rarityMultiplier)
        {
            weapon_rarity = Rarity.exotic;
            rarityMultiplier *= 0.5f;
        }
        else { return; }
        if (Random.Range(1, 101) < rarityMultiplier)
        {
            weapon_rarity = Rarity.hiTech;
            rarityMultiplier *= 0.5f;
        }
        else { return; }
        if (Random.Range(1, 101) < rarityMultiplier)
        {
            weapon_rarity = Rarity.prototype;
        }
    }
    public void SetRandomLevel(float rewardQuality) {
        int tmpLevel = 1;

        tmpLevel += (int)((rewardQuality * 10 * Random.Range(1f, 1.5f)));

        if (tmpLevel > 50)
            tmpLevel = 50;
        weapon_upgLevel = tmpLevel;
    }
    public void SetRandomShootSecuence() {
        int variation;
        if (Random.Range(1, 101) < 60)
        {
            variation = 0;
        }
        else {
            variation = Random.Range(1, 5);
        }
        switch (variation) {
            case 1:
                {
                    weapon_shootSecuence = ShootSecuence.barrage_1way;
                    break;
                }
            case 2:
                {
                    weapon_shootSecuence = ShootSecuence.barrage_2way;
                    break;
                }
            case 3:
                {
                    weapon_shootSecuence = ShootSecuence.barrage_crossful;
                    break;
                }
            case 4:
                {
                    weapon_shootSecuence = ShootSecuence.barrage_crosshalf;
                    break;
                }
            case 5:
                {
                    weapon_shootSecuence = ShootSecuence.barrage_standard;
                    break;
                }
            default:
                {
                    weapon_shootSecuence = ShootSecuence.normal;
                    break;
                }
        }
    }
    public void SetRandomTrajectoryAndMultishoot() {
        int variation;
        switch (weapon_shootSecuence) {
            case ShootSecuence.barrage_1way:
                {
                    variation = Random.Range(1, 4);
                    if (variation == 1) { weapon_projectile_trajectory = ProjectileTrajectory.normal; }
                    else if (variation == 2) { weapon_projectile_trajectory = ProjectileTrajectory.tracking; }
                    else { weapon_projectile_trajectory = ProjectileTrajectory.binarytrack; }
                    weapon_multishoot = Random.Range(4, 8);
                    break;
                }
            case ShootSecuence.barrage_2way:
                {
                    variation = Random.Range(1, 4);
                    if (variation == 1) { weapon_projectile_trajectory = ProjectileTrajectory.normal; }
                    else if (variation == 2) { weapon_projectile_trajectory = ProjectileTrajectory.tracking; }
                    else { weapon_projectile_trajectory = ProjectileTrajectory.binarytrack; }
                    weapon_multishoot = Random.Range(4, 8);
                    break;
                }
            case ShootSecuence.barrage_crossful:
                {
                    variation = Random.Range(1, 5);
                    if (variation == 1) { weapon_projectile_trajectory = ProjectileTrajectory.normal; }
                    else if (variation == 2) { weapon_projectile_trajectory = ProjectileTrajectory.tracking; }
                    else if (variation == 3) { weapon_projectile_trajectory = ProjectileTrajectory.helix; }
                    else { weapon_projectile_trajectory = ProjectileTrajectory.binarytrack; }
                    weapon_multishoot = Random.Range(5, 9);
                    break;
                }
            case ShootSecuence.barrage_crosshalf:
                {
                    variation = Random.Range(1, 5);
                    if (variation == 1) { weapon_projectile_trajectory = ProjectileTrajectory.normal; }
                    else if (variation == 2) { weapon_projectile_trajectory = ProjectileTrajectory.tracking; }
                    else if (variation == 3) { weapon_projectile_trajectory = ProjectileTrajectory.helix; }
                    else { weapon_projectile_trajectory = ProjectileTrajectory.binarytrack; }
                    weapon_multishoot = Random.Range(5, 9);
                    break;
                }
            case ShootSecuence.barrage_standard:
                {
                    variation = Random.Range(1, 4);
                    if (variation == 1) { weapon_projectile_trajectory = ProjectileTrajectory.normal; }
                    else if (variation == 2) { weapon_projectile_trajectory = ProjectileTrajectory.tracking; }
                    else { weapon_projectile_trajectory = ProjectileTrajectory.binarytrack; }
                    weapon_multishoot = Random.Range(1, 3) == 1 ? 1 : 2;
                    break;
                }
            default:
                {
                    variation = Random.Range(1, 9);
                    if (variation <= 5) { weapon_projectile_trajectory = ProjectileTrajectory.normal; }
                    else if (variation == 6) { weapon_projectile_trajectory = ProjectileTrajectory.helix; }
                    else if (variation == 7) { weapon_projectile_trajectory = ProjectileTrajectory.tracking; }
                    else { weapon_projectile_trajectory = ProjectileTrajectory.binarytrack; }
                    if (Random.Range(1, 6) < 3) { weapon_multishoot = 1; }
                    else { weapon_multishoot = Random.Range(3, 6); }
                    break;
                }
        }
        switch (weapon_projectile_trajectory)
        {
            case ProjectileTrajectory.binarytrack:
                {
                    weapon_multishootAperture = Random.Range(1f, 2.5f) * 10 * weapon_multishoot;
                    break;
                }
            case ProjectileTrajectory.helix:
                {
                    weapon_multishootAperture = Random.Range(1f, 2.5f) * 9f * weapon_multishoot;
                    break;
                }
            case ProjectileTrajectory.tracking:
                {
                    weapon_multishootAperture = Random.Range(1f, 2.5f) * 8f * weapon_multishoot;
                    break;
                }
            default:
                {
                    weapon_multishootAperture = Random.Range(1f, 2.5f) * 3.5f * weapon_multishoot;
                    break;
                }

        }
    }
    public void SetDpsStats() {
        if (weapon_multishoot == 1)
        {
            if (Random.Range(1, 10) <= 3)
            {
                weapon_randomSpread = 10;
                weapon_firerate = WEAPON_BASE_FIRERATE *1.75f;
            }
            else
            {
                weapon_firerate = WEAPON_BASE_FIRERATE;
                weapon_randomSpread = 0;
            }
        }
        else {
            weapon_randomSpread = 0;
            weapon_firerate = WEAPON_BASE_FIRERATE / weapon_multishoot;
        }

        switch (weapon_element) {
            case DamageElement.gamma:
                {
                    weapon_heat_per_projectile = 2.8f;
                    weapon_critChance = 0;
                    break;
                }
            case DamageElement.plasma:
                {
                    weapon_heat_per_projectile = 2.65f;
                    weapon_critChance = Random.Range(40, 61);
                    break;
                }
            case DamageElement.photon:
                {
                    weapon_heat_per_projectile = 2.65f;
                    weapon_critChance = Random.Range(40, 61);
                    break;
                }
            case DamageElement.nuclear:
                {
                    weapon_heat_per_projectile = 2.65f;
                    weapon_critChance = 0;
                    break;
                }
            case DamageElement.electric:
                {
                    weapon_heat_per_projectile = 2.5f;
                    weapon_critChance = Random.Range(5, 21);
                    break;
                }
            case DamageElement.cryo:
                {
                    weapon_heat_per_projectile = 2f;
                    weapon_critChance = Random.Range(20, 31);
                    break;
                }
            default:
                {
                    weapon_heat_per_projectile = 2.5f;
                    weapon_critChance = Random.Range(20, 31);
                    break;
                }
        }
        if (CanCrit())
            weapon_critMultiplier = 1 + (35 / weapon_critChance);
        else
            weapon_critMultiplier = 1;
    }
    public void SetDamage() {
        switch (weapon_element)
        {
            case DamageElement.gamma:
                {
                    weapon_damage = (int)((WEAPON_BASE_DAMAGE + WEAPON_BASE_SCALING_PER_LEVEL * weapon_upgLevel) * ELEM_GAMMA_DMGMULT);
                    break;
                }
            case DamageElement.plasma:
                {
                    weapon_damage = (int)((WEAPON_BASE_DAMAGE + WEAPON_BASE_SCALING_PER_LEVEL * weapon_upgLevel) * ELEM_PLASMA_DMGMULT);
                    break;
                }
            case DamageElement.photon:
                {
                    weapon_damage = (int)((WEAPON_BASE_DAMAGE + WEAPON_BASE_SCALING_PER_LEVEL * weapon_upgLevel) * ELEM_PHOTON_DMGMULT);
                    break;
                }
            case DamageElement.nuclear:
                {
                    weapon_damage = (int)((WEAPON_BASE_DAMAGE + WEAPON_BASE_SCALING_PER_LEVEL * weapon_upgLevel) * ELEM_NUCLEAR_DMGMULT);
                    break;
                }
            case DamageElement.electric:
                {
                    weapon_damage = (int)((WEAPON_BASE_DAMAGE + WEAPON_BASE_SCALING_PER_LEVEL * weapon_upgLevel) * ELEM_ELECTRIC_DMGMULT);
                    break;
                }
            case DamageElement.cryo:
                {
                    weapon_damage = (int)((WEAPON_BASE_DAMAGE + WEAPON_BASE_SCALING_PER_LEVEL * weapon_upgLevel) * ELEM_CRYO_DMGMULT);
                    break;
                }
            default:
                {
                    weapon_damage = (int)((WEAPON_BASE_DAMAGE + WEAPON_BASE_SCALING_PER_LEVEL * weapon_upgLevel) * ELEM_PULSE_DMGMULT);
                    break;
                }
        }
    }
    #endregion
    #region Atribute Getters
    public DamageElement GetWeaponElement() {
        return weapon_element;
    }
    public bool CanCrit() {
        return weapon_element != DamageElement.nuclear && weapon_element != DamageElement.gamma;
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
        if (weapon_element == DamageElement.pulse)
            return (WEAPON_BASE_DAMAGE + weapon_upgLevel * WEAPON_BASE_SCALING_PER_LEVEL) * ELEM_PULSE_DMGMULT;
        else
            return 0;
    }
    public float GetNuclearDamage() {
        if (weapon_element == DamageElement.nuclear)
            return (WEAPON_BASE_DAMAGE + weapon_upgLevel * WEAPON_BASE_SCALING_PER_LEVEL) * ELEM_NUCLEAR_DMGMULT;
        else
            return 0;
    }
    public float GetPhotonDamage() {
        if (weapon_element == DamageElement.photon)
            return (WEAPON_BASE_DAMAGE + weapon_upgLevel * WEAPON_BASE_SCALING_PER_LEVEL) * ELEM_PHOTON_DMGMULT;
        else
            return 0;
    }
    public float GetElectricDamage() {
        if (weapon_element == DamageElement.electric)
            return (WEAPON_BASE_DAMAGE + weapon_upgLevel * WEAPON_BASE_SCALING_PER_LEVEL) * ELEM_ELECTRIC_DMGMULT;
        else
            return 0;
    }
    public float GetCryoDamage() {
        if (weapon_element == DamageElement.cryo)
            return (WEAPON_BASE_DAMAGE + weapon_upgLevel * WEAPON_BASE_SCALING_PER_LEVEL) * ELEM_CRYO_DMGMULT;
        else
            return 0;
    }
    public float GetPlasmaDamage()
    {
        if (weapon_element == DamageElement.plasma)
            return (WEAPON_BASE_DAMAGE + weapon_upgLevel * WEAPON_BASE_SCALING_PER_LEVEL) * ELEM_PLASMA_DMGMULT;
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
    public Rarity GetWeaponRarity() {
        return weapon_rarity;
    }
    public int GetUpgradeLevel() {
        return weapon_upgLevel;
    }
    public int GetWeaponDamage() {
        return (int)weapon_damage;
    }
    public float GetDps() {
        return weapon_damage * weapon_multishoot * weapon_firerate;
    }
    #endregion
    #region Weapon Interaction And Modification
    public void LevelUp() {
        weapon_upgLevel++;
        if (weapon_upgLevel > 99)
            weapon_upgLevel = 99;
        SetDamage();
    }
    #endregion
    #region Comparators
    // Sort by Level -> Rarity -> Element
    public static int CompareByLevel(WeaponData w1, WeaponData w2) {
        if (w1.GetUpgradeLevel() == w2.GetUpgradeLevel()) {
            if (w1.GetWeaponRarity() == w2.GetWeaponRarity())
            {
                if (w1.GetWeaponElement() == w2.GetWeaponElement())
                    return 0;
                if (w1.GetWeaponElement() < w2.GetWeaponElement())
                    return -1;
                else
                    return 1;
            }
            if (w1.GetWeaponRarity() < w2.GetWeaponRarity())
                return -1;
            return 1;
        }
        if (w1.GetUpgradeLevel() < w2.GetUpgradeLevel())
            return -1;
        return 1;
    }
    // Sort by Element -> Rarity -> Level
    public static int CompareByElement(WeaponData w1, WeaponData w2)
    {
        if (w1.GetWeaponElement() == w2.GetWeaponElement()) {
            if (w1.GetWeaponRarity() == w2.GetWeaponRarity())
            {
                if (w1.GetUpgradeLevel() == w2.GetUpgradeLevel())
                    return 0;
                if (w1.GetUpgradeLevel() < w2.GetUpgradeLevel())
                    return -1;
                return 1;
            }
            if (w1.GetWeaponRarity() < w2.GetWeaponRarity())
                return -1;
            return 1;
        }
        if (w1.GetWeaponElement() < w2.GetWeaponElement())
            return -1;
        return 1;
    }
    // Sort by Rarity -> Level -> Element
    public static int CompareByRarity(WeaponData w1, WeaponData w2) {
        if (w1.GetWeaponRarity() == w2.GetWeaponRarity()) {
            if (w1.GetUpgradeLevel() == w2.GetUpgradeLevel()) {
                if (w1.GetWeaponElement() == w2.GetWeaponElement())
                    return 0;
                if (w1.GetWeaponElement() < w2.GetWeaponElement())
                    return -1;
                else
                    return 1;
            }    
            if (w1.GetUpgradeLevel() < w2.GetUpgradeLevel())
                return -1;
            return 1;
        }
        if (w1.GetWeaponRarity() < w2.GetWeaponRarity())
            return -1;
        return 1;
    }
    #endregion
}
