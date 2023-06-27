using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class LoadRank : MonoBehaviour
{
    public Text BestPlayerText;

    static int bestScore;
    static string bestPlayer;

    private void Awake()
    {
        LoadPlayerRank();
    }

    void SetBestRank()
    {
        if (BestPlayerText == null && bestScore == 0)
            BestPlayerText.text = "";
        else
            BestPlayerText.text = $"Best Score - {bestPlayer}: {bestScore}";
    }

    void LoadPlayerRank()
    {
        string path = Application.persistentDataPath + "/saverank.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.BestPlayerName;
            bestScore = data.BestScore;
            SetBestRank();
        }
    }

    class SaveData
    {
        public string BestPlayerName;
        public int BestScore;
    }
}
