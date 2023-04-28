using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    [SerializeField] int playerScore;
    [SerializeField] string playerName;

    const int startingScore = 0;

    public static PersistentData Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerScore = startingScore;
        playerName = "";
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName(string s)
    {
        playerName = s;
    }
    public void SetScore(int score)
    {
        playerScore = score;
    }
    public string GetName()
    {
        return playerName;
    }
    public int GetScore()
    {
        return playerScore;
    }
}
