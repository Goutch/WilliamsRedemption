using System;
using System.Collections;
using System.Net.Configuration;
using UnityEngine;
using UnityEngine.UI;
using Game.Controller;
using Game.UI;
using Game.Values.AnimationParameters;
using Harmony;
using JetBrains.Annotations;
using UnityEngine.SocialPlatforms.Impl;

[Serializable]
public struct PlayerData
{
    public string name;
    public int score;
    public float time;

    public PlayerData(string name, int score, float time)
    {
        this.name = name;
        this.score = score;
        this.time = time;
    }
}

public class LeaderBoard : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    private GameController gameController;

    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag(Game.Values.Tags.GameController)
            .GetComponent<GameController>();
    }

    [UsedImplicitly]
    public void SendScoreToWebApi()
    {
        InsertDataToDatabase(nameField.text, gameController.TotalScore, gameController.TotalTime);
    }

    private void InsertDataToDatabase(string name, int score, float time)
    {
        PlayerData myPlayerData = new PlayerData(name, score, time);

        if (myPlayerData.name != "")
        {
            SendDataToServer(myPlayerData);
        }
    }

    private void SendDataToServer(PlayerData myPlayerData)
    {
        string json = JsonUtility.ToJson(myPlayerData);


        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");

        var formData = System.Text.Encoding.UTF8.GetBytes(json);

        WWW www = new WWW("http://35.188.160.44/api/insert", formData, postHeader);
    }

    public void GoToLeaderboardOnWeb()
    {
        Application.OpenURL("http://35.188.160.44/leaderboardPage");
    }
}