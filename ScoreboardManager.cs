using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using System;
public class ScoreboardManager : MonoBehaviour {
    [Serializable]
    public class records
    {
        public string name="--------";
        public string days="--";
        public records(string days_, string name_)
        {
            days = days_;
            name = name_;
        }
        
    }

    
    public List<records> table;//pole w której przechowywana jest tablica wyników tablica wyników
    public void WriteString()//metoda dopisująca wynik do listy wyników
    {
        string path = "Assets/Scripts/Scoreboard.txt";
        ReadfromFile(path);

        InputField nazwa = GameObject.Find("InputField").GetComponent<InputField>();
        records yourRecord = new records(GameManager.instance.level.ToString(), nazwa.text);
        table.Add(yourRecord);
        table.Sort((records x, records y) =>
            Int32.Parse(x.days).CompareTo(Int32.Parse(y.days))//przeciążenie sortowania aby sortowało na podstawie ilości przetrwanych dni
        );
        StreamWriter writer = new StreamWriter(path, false);
        foreach (records rekord in table)
        {
            writer.WriteLine(rekord.days);
            writer.WriteLine(rekord.name);

        }
        writer.Close();
    }

   
    public void ReadString()//metoda wyswietlająca tablice wyników
    {
        
        string path = "Assets/Scripts/Scoreboard.txt";
        ReadfromFile(path);
        int a = 1;
        int cos = table.Count;
        int scoreSize = 5;
        for (int i=0;i<scoreSize&&i<cos;i++)
        {
            string boxname = "Days" + (i + 1);
            Text daysbox = GameObject.Find(boxname).GetComponent<Text>();
            daysbox.text = table[cos-a].days;
            boxname = "Name" + (i + 1);
            Text namebox = GameObject.Find(boxname).GetComponent<Text>();
            namebox.text = table[cos-a].name;
            a++;
        }
        
    }
    public void ReadfromFile(string path)//metoda odczytująca tablicę wyników z pliku
    {
        table.Clear();
        StreamReader reader = new StreamReader(path);
        while (reader.Peek() >= 0)
        {
            string days;
            string name;
            days = reader.ReadLine();
            name = reader.ReadLine();
            table.Add(new records(days, name));

        }
        reader.Close();

    }

}
