using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity {

    void DealDamage(float amount, Enums.DamageType dmgType, IEntity damageDealer);
    void Shoot(float angle);
    void Kill();
    void ResetEntity();

    float GetPhysicalDamage();
    float GetPhotonDamage();
    float GetNuclearDamage();
    float GetElectricDamage();
    float GetCryoDamage();
    float GetPlasmaDamage();
    float GetGammaDamage();
    float GetBulletSpeedScale();
    float GetHealthPercent();
    float GetEntityTimescale();
    float GetCritChance();
    float GetCritMultiplier();

    int GetBulletBounces();

    bool IsAlly();
    bool IsAlive();

    WeaponData.ProjectileTrajectory GetWeaponProjectileTrajectory();
    WeaponData.DamageElement GetDamageElement();

    GameObject GetGameObject();



}
