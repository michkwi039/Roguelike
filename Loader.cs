using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {


    public GameObject gameManager;
    public GameObject menu;
    void Start()//metoda aktywująca menu podczas uruchamiania gry
    {
        if (GameManager.instance == null)
        {
            menu.SetActive(true);
            Instantiate(gameManager);
        }
    }
    // Use this for initialization
    void Awake()
    {   //metoda aktywująca menu podczas uruchamiania gry
        if (GameManager.instance == null)
        {
            menu.SetActive(true);
            Instantiate(gameManager);
        }
	}
	
}
