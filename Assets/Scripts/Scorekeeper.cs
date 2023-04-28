using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scorekeeper : MonoBehaviour
{
    [SerializeField] int score = 0;
    const int DEFAULT_POINTS = 1000;
    [SerializeField] Text scoreText;
    [SerializeField] Text levelText;
    [SerializeField] Text nameText;
    [SerializeField] Text directionsText;
    [SerializeField] int levelNum;
    [SerializeField] string directions;


    // Start is called before the first frame update
    void Start()
    {
        levelNum = SceneManager.GetActiveScene().buildIndex;
        score = PersistentData.Instance.GetScore();
        DisplayName();
        DisplayLevel();
        DisplayScore();
        DisplayDirections();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DisplayDirections()
    {
        if(levelNum == 1)
        {
            directions = "Slow Balloon. Shoot with Fireball";
        }
        else if (levelNum == 2)
        {
            directions = "Faster Balloon with vertical movement. Shoot with Fireball";
        }
        else if (levelNum == 3)
        {
            directions = "Balloon will flee from you. Shoot with Fireball";
        }
        directionsText.text = "Directions: " + directions;
    }
    public void DisplayName()
    {
        nameText.text = "Go get 'em " + PersistentData.Instance.GetName();
    }
    public void DisplayScore()
    {
        scoreText.text = "Score: " + PersistentData.Instance.GetScore();
    }

    public void DisplayLevel()
    {
        levelText.text = "Level: " + (levelNum);
    }
    public void AdvanceLevel()
    {
        levelNum += levelNum + 1;
    }
    public void UpdateScore(int balloonSize)
    {

        score += DEFAULT_POINTS - (balloonSize);
        PersistentData.Instance.SetScore(score);
        DisplayScore();
        
    }

    public void UpdateScore()
    {
        UpdateScore(DEFAULT_POINTS);
    }

    public void ZeroScore()
    {
        score = 0;
    }

}

