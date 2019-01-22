using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameManager : MonoBehaviour {

    private GameData GD;

    public static GlobalGameManager currentInstance;

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
}
