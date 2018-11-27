using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoardManager : MonoBehaviour {
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count(int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    public int columns = 8;
    public int rows = 8;
    public Count wallCount = new Count(5, 9);//ustawianie zakresów ilości przedmiotów które pojawią sie na planszy
    public Count foodCount = new Count(1, 5);
    public Count coinCount = new Count(1, 2);
    public Count tentCount = new Count(2, 4);
    public GameObject exit;//pola przechowujące różne rodzaje klocków z których będzie składany świat
    public GameObject[] floorTiles;
    public GameObject[] wallTiles;
    public GameObject[] foodTiles;
    public GameObject[] enemyTiles;
    public GameObject[] outerWallTiles;
    public GameObject[] coinTiles;
    public GameObject[] tentTiles;
    public GameObject[] weaponTiles;
    public GameObject[] armorTiles;
    public GameObject shopTile;

    private Shop shopHolder;
    private Transform boardHolder;
    private List <Vector3> gridPositions = new List <Vector3> ();
    
    void InitialiseList()//tworzenie listy pozycji w których mogą pojawiać się przedmioty
    {
        gridPositions.Clear();

        for (int x=1; x<columns-1; x++)
        {
            for (int y=1; y<rows-1;y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    void BoardSetup()//wytwarzanie podłogi i ścian zewnętrznych 
    {
        boardHolder = new GameObject("Board").transform;

        for (int x=-1;x<columns+1;x++)
        {
            for (int y=-1;y<rows+1;y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                GameObject instance= Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;
                instance.transform.SetParent(boardHolder);
            }
        }
    }
    Vector3 RandomPosition()//metoda losująca pozycje w której będzie umieszczany przedmiot i usuwająca ją z listy pozycji aby wiecej niż jeden przedmiot nie pojawiał się w danej pozycji
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    void LayoutObjectAtRandom(GameObject[] tileArray,int minimum,int maximum)//metoda umieszczająca przedmiot w danej wylosowanej pozycji
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i=0; i<objectCount;i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }
    void LayoutCombinedObjectAtRandom(GameObject[] tileArray, int minimum, int maximum)//metoda umieszczająca rozbudowany przedmiot w danej wylosowanej pozycji
    {
        int objectCount = Random.Range(minimum, maximum + 1);

        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            int randomTile = Random.Range(0, tileArray.Length / 2);
            GameObject tileChoice = tileArray[randomTile];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
            Instantiate(tileArray[randomTile+2], new Vector3(randomPosition.x+1,randomPosition.y), Quaternion.identity);

        }
    }
    public void SetupScene(int level)//metoda rozmieszczająca odpowiednie ilości przedmiotów na planszy
    {
        BoardSetup();
        InitialiseList();
        if(level%2==1){//zwykły poziom
            GameManager.instance.shopArea = false;
            LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
            LayoutObjectAtRandom(foodTiles, foodCount.minimum, foodCount.maximum);
            LayoutObjectAtRandom(coinTiles, coinCount.minimum, coinCount.maximum);
            int enemyCount = (int)Mathf.Log(level, 2f);
            LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        }
        else//poziom sklepowy
        {

            GameManager.instance.shopArea = true;
            //LayoutCombinedObjectAtRandom(tentTiles, tentCount.minimum, tentCount.maximum);
            Instantiate(tentTiles[0], new Vector3(columns - 7, rows - 7, 0f), Quaternion.identity);
            Instantiate(tentTiles[2], new Vector3(columns - 6, rows - 7, 0f), Quaternion.identity);
            Instantiate(tentTiles[1], new Vector3(columns - 3, rows - 7, 0f), Quaternion.identity);
            Instantiate(tentTiles[3], new Vector3(columns - 2, rows - 7, 0f), Quaternion.identity);
            Instantiate(tentTiles[1], new Vector3(columns - 7, rows - 3, 0f), Quaternion.identity);
            Instantiate(tentTiles[3], new Vector3(columns - 6, rows - 3, 0f), Quaternion.identity);
            Instantiate(tentTiles[0], new Vector3(columns - 3, rows - 3, 0f), Quaternion.identity);
            Instantiate(tentTiles[2], new Vector3(columns - 2, rows - 3, 0f), Quaternion.identity);
            Instantiate(tentTiles[0], new Vector3(columns - 7, rows - 2, 0f), Quaternion.identity);
            Instantiate(tentTiles[2], new Vector3(columns - 6, rows - 2, 0f), Quaternion.identity);
            Instantiate(tentTiles[1], new Vector3(columns - 3, rows - 2, 0f), Quaternion.identity);
            Instantiate(tentTiles[3], new Vector3(columns - 2, rows - 2, 0f), Quaternion.identity);
            Instantiate(shopTile, new Vector3(columns - 4, rows - 4, 0f), Quaternion.identity);
            Instantiate(weaponTiles[GameManager.instance.weapon], new Vector3(columns - 6, rows - 5, 0f), Quaternion.identity);
            Instantiate(armorTiles[GameManager.instance.armor], new Vector3(columns - 3, rows - 5, 0f), Quaternion.identity);
        }
        Instantiate(exit, new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }

}
