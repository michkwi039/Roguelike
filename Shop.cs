using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Shop : MonoBehaviour {

    // Use this for initialization
    public GameObject[] weaponTiles;
    public GameObject[] armorTiles;
    void awake()
    {

    }
    public int GetWeapon(){
        return GameManager.instance.weapon;
    }
    public int GetArmor()
    {
        return GameManager.instance.armor;
    }

}
