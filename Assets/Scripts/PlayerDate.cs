using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDate : MonoBehaviour
{
    public static PlayerDate Instance;

    public string playerName;
    public int score;

    private void Awake()
    {
        if(Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
