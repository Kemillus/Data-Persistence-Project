using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text namePlayer;
    public Text ScoreText;
    public Text bestScoreAndPlayer;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    static int bestScore;
    static string bestPlayer;

    private void Awake()
    {
        LoadPlayerRank();
    }

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        namePlayer.text = PlayerDate.Instance.playerName;
        SetBestPlayer();
    }

    private void SetBestPlayer()
    {
        if (bestPlayer == null && bestScore == 0)
            bestScoreAndPlayer.text = "";

        else
            bestScoreAndPlayer.text = $"Best Score - {bestPlayer}: {bestScore}";
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = UnityEngine.Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        PlayerDate.Instance.score = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        CheckBestPlayer();
        GameOverText.SetActive(true);
    }

    private void CheckBestPlayer()
    {
        int score = PlayerDate.Instance.score;

        if (score > bestScore)
        {
            bestPlayer = PlayerDate.Instance.playerName;
            bestScore = score;
            bestScoreAndPlayer.text = $"Best Score - {bestPlayer}: {bestScore}";
            SaveGameRank(bestPlayer, bestScore);
        }
    }

    public void SaveGameRank(string bestPlayer, int bestScore)
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestPlayer = bestPlayer;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/saverank.json", json);
    }

    private void LoadPlayerRank()
    {
        string path = Application.persistentDataPath + "/saverank.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            bestPlayer = data.bestPlayer;
            bestScore = data.bestScore;
        }
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    [Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestPlayer;
    }
}
