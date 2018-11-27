using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MovingObject {

    public int wallDamage = 1;
    public int pointsPerFood = 10;
    public int pointsPerSoda = 20;
    public int moneyPerCoin = 4;
    public int moneyPerCoinBag = 10;
    public float restartLevelDelay = 1f;
    public int armorCost;
    public int weaponCost;
    public Text foodText;

    private Animator animator;
    private int food;
    private int money;
    private int armor;
    private int weapon;
	// Use this for initialization
	protected override void Start () {//pobieranie aktualnego ekwipunku z GameManegera
        animator = GetComponent<Animator>();

        food = GameManager.instance.playerFoodPoints;
        money = GameManager.instance.playerMoney;
        armor = GameManager.instance.armor;
        weapon=GameManager.instance.weapon;

        foodText.text = "Food " + food;

        base.Start();
	}
    private void OnDisable()//przepisywanie wartości jedzenia i pieniedzy przy wyłączaniu
    {
        GameManager.instance.playerFoodPoints = food;
        GameManager.instance.playerMoney = money;
    }
	// Update is called once per frame
	void Update () {//sprawdzanie czy gracz chce wykonać ruch
        if (!GameManager.instance.playersTurn) return;

        int horizontal=0;
        int vertical = 0;

        horizontal = (int)Input.GetAxisRaw("Horizontal");
        vertical = (int)Input.GetAxisRaw("Vertical");

        if (horizontal != 0)
            vertical = 0;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Wall>(horizontal, vertical);
	}

    protected override void AttemptMove <T> (int xDir,int yDir)//poruszanie się i w zależności od strefy w której znajduje się gracz wyświetlanie odpowiedniego komunikatu
    {
        if (GameManager.instance.shopArea == true)
        {
            foodText.text = "Money: " + money;
        }
        else
        {
            food--;
            foodText.text = "Food: " + food;
        }
        base.AttemptMove<T>(xDir, yDir);

        RaycastHit2D hit;

        CheckIfGameOver();

        GameManager.instance.playersTurn = false;

    }
    private void OnTriggerEnter2D (Collider2D other)//różne typy zachowań gracza w zależności od tego na co wejdzie
    {
        if (other.tag == "Exit")
        {
            Invoke("Restart", restartLevelDelay);
            enabled = false;
        }
        else if (other.tag=="Food")
        {
            food += pointsPerFood;
            foodText.text = "+" + pointsPerFood + "  Food: " + food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Soda")
        {
            food += pointsPerSoda;
            foodText.text = "+" + pointsPerSoda + "  Food: "+food;
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Coin")
        {
            money += moneyPerCoin;
            foodText.text = "+" + moneyPerCoin + "  Coins";
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "CoinBag")
        {
            money += moneyPerCoinBag;
            foodText.text = "+" + moneyPerCoinBag + "  Coins";
            other.gameObject.SetActive(false);
        }
        else if (other.tag == "Weapon")
        {
            if (money < 20)
            {
                foodText.text = "You don't have enough money";
            }
            else
            {
                money -= 20;
                foodText.text = "You bought an item";
                weapon += 1;
                other.gameObject.SetActive(false);
                GameManager.instance.weapon = weapon;
                GameManager.instance.WeaponActivator();
            }
        }
        else if (other.tag == "Armor")
        {
            if (money < 20)
            {
                foodText.text = "You don't have enough money";
            }
            else
            {
                money -= 20;
                foodText.text = "You bought an item";
                armor += 1;
                other.gameObject.SetActive(false);
                GameManager.instance.armor =armor;
                GameManager.instance.ArmorActivator();
            }
        }
    }

    protected override void OnCantMove<T>(T component)//jeśli nie można się przemieścić w dane miejscie zakładamy że postać uderzyła w mur i zadajemy mu odpowiednią ilość obrażeń
    {
        Wall hitWall = component as Wall;
        hitWall.DamageWall(wallDamage*weapon);
        animator.SetTrigger("playerChop");
    }

    private void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void LoseFood(int loss)//utrata życia w przypadku trafienia przez zombie
    {
        animator.SetTrigger("playerHit");
        food -= (loss/armor);
        foodText.text = "-" + loss / armor + "  Food: " + food;
        CheckIfGameOver();
    }
    private void CheckIfGameOver()//sprawdzanie czy nie przegraliśmy
    {
        if(food <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
}
