using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameData : MonoBehaviour {

    public ShipData playerSelectedShip;
    public WeaponData playerSelectedWeapon;

    public static GlobalGameData currentInstance;

    void Awake()
    {
        currentInstance = this;
    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
