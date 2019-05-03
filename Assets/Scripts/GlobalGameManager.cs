using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour {

    private GameData GD;

    public static GlobalGameManager currentInstance;
    public Color qualityColorCommon;
    public Color qualityColorRare;
    public Color qualityColorExotic;
    public Color qualityColorHiTech;
    public Color qualityColorPrototype;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        currentInstance = this;
        GD = new GameData();
    }
	// Update is called once per frame
	void Update () {
		
	}
    public ShipData GetPlayerSelectedShip()
    {
        return GD.GetPlayerSelectedShip();
    }
    public WeaponData GetPlayerSelectedWeapon()
    {
        return GD.GetPlayerSelectedWeapon();
    }
    public GameData GetGameData() {
        return GD;
    }
    public Color GetQualityColorCommon() {
        return qualityColorCommon;
    }
    public Color GetQualityColorRare()
    {
        return qualityColorRare;
    }
    public Color GetQualityColorExotic()
    {
        return qualityColorExotic;
    }
    public Color GetQualityColorHiTech()
    {
        return qualityColorHiTech;
    }
    public Color GetQualityColorPrototype()
    {
        return qualityColorPrototype;
    }
}
