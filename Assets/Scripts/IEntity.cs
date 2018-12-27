using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity {
    void DealDamage(float amount, Enums.DamageType dmgType);
    void Shoot(float angle);
    void Kill();
    void ResetEntity();
    float GetPhysicalDamage();
    float GetPhotonDamage();
    float GetNuclearDamage();
    float GetElectricDamage();
    float GetCryoDamage();
    float GetBulletSpeedScale();
    int GetBulletBounces();
    float GetTrajectoryTracking();
    float GetTrajectoryHelix();
    float GetTrajectoryWave();
    float GetEntityTimescale();
    bool IsAlly();
    bool IsAlive();
    float GetHealthPercent();
    GameObject GetGameObject();



}
