using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScores : MonoBehaviour
{
    const int TOTAL_HIGH_SCORES = 5;
    const string NAME_KEY = "Player";
    const string SCORE_KEY = "Score";

    [SerializeField] string pName;
    [SerializeField] int pScore;
    [SerializeField] Text[] nameTexts;
    [SerializeField] Text[] scoreTexts;

    // Start is called before the first frame update
    void Start()
    {
        pName = PersistentData.Instance.GetName();
        pScore = PersistentData.Instance.GetScore();

        RecordPlayerScore();
        DisplayHighScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RecordPlayerScore()
    {
        for (int i = 1; i < TOTAL_HIGH_SCORES + 1; i++)
        {
            string curNameKey = NAME_KEY + i;
            string curScoreKey = SCORE_KEY + i;

            if (PlayerPrefs.HasKey(curScoreKey))
            {
                int curScore = PlayerPrefs.GetInt(curScoreKey);
                if(pScore > curScore)
                {
                    int tempScore = curScore;
                    string tempName = PlayerPrefs.GetString(curNameKey);

                    PlayerPrefs.SetString(curNameKey, pName);
                    PlayerPrefs.SetInt(curScoreKey, pScore);

                    pName = tempName;
                    pScore = tempScore;
                }
            }
            else
            {
                PlayerPrefs.SetString(curNameKey, pName);
                PlayerPrefs.SetInt(curScoreKey, pScore);
                return;
            }
        }
    }
    void DisplayHighScores()
    {
        for(int i = 0; i < TOTAL_HIGH_SCORES; i++)
        {
            nameTexts[i].text = PlayerPrefs.GetString(NAME_KEY + (i + 1));
            scoreTexts[i].text = PlayerPrefs.GetInt(SCORE_KEY + (i + 1)).ToString();
        }
    }

}
