using System;
using System.Collections;
using System.Net.Configuration;
using UnityEngine;
using UnityEngine.UI;
using Game.Controller;
using Game.UI;
using UnityEngine.SocialPlatforms.Impl;

[Serializable]
public class PlayerData:MonoBehaviour
{
    public string name;
    public int score;
    public float time;
}

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    public void InsertDataToDatabase(int score, float time)
    {
        PlayerData myPlayerData = new PlayerData();
        
        myPlayerData.name = nameField.text;
        myPlayerData.score = score;
        myPlayerData.time = time;
        
        if (myPlayerData.name != "")
        {
            SendDataToServer(myPlayerData);
        } 
    }

    private void SendDataToServer(PlayerData myPlayerData)
    {
        string json = JsonUtility.ToJson(myPlayerData);
            
        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type","application/json");

        var formData = System.Text.Encoding.UTF8.GetBytes(json);
            
        www = new WWW("http://35.188.160.44/api/insert", formData, postHeader);
    }
}