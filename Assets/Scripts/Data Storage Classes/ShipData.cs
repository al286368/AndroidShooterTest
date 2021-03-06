﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipData {

    public enum Manufacturer { generic }
    public enum ShipClass { generic }
    public enum Upgrade { none }
    public enum Trait { none, precise, charged }

    private int baseEnergy;
    private int baseShield;
    private float shieldRecoveryRate;
    private float shieldRecoveryDelay;
    private float baseDefense;
    private float specialRechargeRate;
    private float heatRecovery;
    private int upgradeSlots;

    private Enums.PlayerSpecials special;

    private List<Upgrade> upgradesInstalled;
    private List<Trait> baseTraits;

    public ShipData() {
        baseEnergy = 100;
        baseShield = 300;
        baseDefense = 0;
        specialRechargeRate = 50;
        shieldRecoveryRate = 20;
        shieldRecoveryDelay = 2;
        heatRecovery = 10;
        upgradeSlots = 5;
        special = Enums.PlayerSpecials.deflectPulse;
    }

    public int GetShipEnergy()
    {
        return baseEnergy;
    }
    public int GetShipShield()
    {
        return baseShield;
    }
    public float GetSpecialRechargeRate()
    {
        return specialRechargeRate;
    }
    public int GetUpgradeSlots()
    {
        return upgradeSlots;
    }
    public float GetShipDefense() {
        return baseDefense;
    }
    public float GetShieldRecoveryRate() {
        return shieldRecoveryRate;
    }
    public float GetShieldRecoveryDelay() {
        return shieldRecoveryDelay;
    }
    public float GetHeatRecovery() {
        return heatRecovery;
    }
    public Enums.PlayerSpecials GetSpecial()
    {
        return special;
    }
}
