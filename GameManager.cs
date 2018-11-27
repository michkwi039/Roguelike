using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float levelStartDelay = 2f;
    public float turnDelay = .1f;
    public static GameManager instance = null;              
    private BoardManager boardScript;
    public int weapon = 1;
    public int armor = 1;
    public int playerFoodPoints = 100;
    public int playerMoney=0;
    [HideInInspector] public bool playersTurn = true;
    public bool shopArea=false;
    

    private Text levelText;
    private Text EqWeapon;
    private Text EqArmor;
    private GameObject levelImage;
    private GameObject weaponText;
    private GameObject armorText;
    private GameObject scoreManager;
    public int level = 0; 
    private List<Enemy> enemies;
    private bool enemiesMoving;
    private bool doingSetup;
    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)//może być tylko jeden GameManager więc robimy go singletonem

            instance = this;

        else if (instance != this)

            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        enemies = new List<Enemy>();

        boardScript = GetComponent<BoardManager>();

    }
    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        level++;
        InitGame();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void InitGame()//wczytywanie komponentów podczas ładowania gry
    {
        doingSetup = true;
        levelImage = GameObject.Find("LevelImage");
        levelText = GameObject.Find("LevelText").GetComponent<Text>();
        levelText.text = "Day " + level;
        levelImage.SetActive(true);
        Invoke("HideLevelImage", levelStartDelay);
        weaponText = GameObject.Find("WeaponText");
        armorText = GameObject.Find("ArmorText");
        EqWeapon = GameObject.Find("EqWeapon").GetComponent<Text>();
        EqWeapon.text = "Weapon: " + weapon;
        EqArmor = GameObject.Find("EqArmor").GetComponent<Text>();
        EqArmor.text = "Armor: " + armor;
        scoreManager = GameObject.Find("PlayerName");
        scoreManager.SetActive(false);
        enemies.Clear();
        boardScript.SetupScene(level);
        ShopActivator();

    }
    private void ShopActivator()//uruchamia UI sklepu
    {
        if (!shopArea)
        {
            weaponText.SetActive(false);
            armorText.SetActive(false);
        }
        else
        {
            weaponText.SetActive(true);
            armorText.SetActive(true);
        }
    }
    public void WeaponActivator()//po zakupieniu broni aktualizuje ekwipunek i wyłącza część UI sklepu za nią odpowiedzialną
    {
        EqWeapon.text = "Weapon: " + weapon;
        weaponText.SetActive(false);
    }
    public void ArmorActivator()//po zakupieniu pancerza aktualizuje ekwipunek i wyłącza część UI sklepu za nią odpowiedzialną
    {
       
        EqArmor.text = "Armor: " + armor;
        armorText.SetActive(false);
    }
    private void HideLevelImage()//wyłącza ekran ładowania
    {
        levelImage.SetActive(false);
        doingSetup = false;
    }
    public void GameOver()//metoda wyświetlająca ekran końcowy po śmierci bohatera
    {
        levelText.text = "After " + level + "days, you starved.";
        levelImage.SetActive(true);
        new WaitForSeconds(2);

        scoreManager.SetActive(true);
        levelImage.SetActive(false);
    }


    //Update is called every frame.
    void Update()
    {

        if (playersTurn || enemiesMoving||doingSetup)
            return;

        StartCoroutine(MoveEnemies());//jeśli nie ma tury gracza rozpocznij ruch wrogów 

    }

    public void AddEnemyToList(Enemy script)//dodanie wrogów do listy aby przy ładowaniu poziomu można było łatwo usunąć wrogów z poprzedniego poziomu
    {
        enemies.Add(script);
    }

    IEnumerator MoveEnemies()//przemieszczanie każdego wroga po kolei
    {
        enemiesMoving = true;
        yield return new WaitForSeconds(turnDelay);
        if (enemies.Count == 0)
        {
            yield return new WaitForSeconds(turnDelay);
        }
        for (int i=0; i< enemies.Count; i++)
        {
            enemies[i].MoveEnemy();
            yield return new WaitForSeconds(turnDelay);
        }
        playersTurn = true;
        enemiesMoving = false;
    }
}
