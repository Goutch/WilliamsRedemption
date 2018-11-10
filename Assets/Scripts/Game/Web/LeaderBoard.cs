using System;
using System.Collections;
using System.Net.Configuration;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerData:MonoBehaviour
{
    public string name;
    public int score;
    public float time;
}

public class LeaderBoard : MonoBehaviour
{        
    public void InsertTest()
    {
        InsertDataToDatabase();
    }
        
    public void InsertDataToDatabase()
    {
        PlayerData myPlayerData = new PlayerData();
        myPlayerData.name = GameObject.Find(Game.Values.GameObject.NameFieldText).GetComponent<Text>().text;
        Debug.Log(myPlayerData.name);
        myPlayerData.score = 234;
        myPlayerData.time = 255;
        if (myPlayerData.name != "")
        {
            string json = JsonUtility.ToJson(myPlayerData);
            
            WWW www;
            Hashtable postHeader = new Hashtable();
            postHeader.Add("Content-Type","application/json");

            var formData = System.Text.Encoding.UTF8.GetBytes(json);
            
            www = new WWW("http://35.188.160.44/api/insert", formData, postHeader);
        } 
    }
}