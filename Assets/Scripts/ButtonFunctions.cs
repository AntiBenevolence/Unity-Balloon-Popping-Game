using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonFunctions : MonoBehaviour
{
    [SerializeField] string playerName;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject resumeButton;
    [SerializeField] InputField playerNameInput;
    [SerializeField] int level;

    void Awake()
    {
        level = SceneManager.GetActiveScene().buildIndex;
       
        pauseButton = GameObject.FindGameObjectWithTag("PauseButton");
    
        resumeButton = GameObject.FindGameObjectWithTag("ResumeButton");
        
        if (level == 1 || level == 2 || level == 3)
        {
            pauseButton.SetActive(true);
            resumeButton.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (level == 1 || level == 2 || level == 3)
            resumeButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        playerName = playerNameInput.text;
        PersistentData.Instance.SetName(playerName);
        SceneManager.LoadScene("Level 1");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
    public void HighScores()
    {
        SceneManager.LoadScene("HighScores");
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        pauseButton.SetActive(false);
        resumeButton.SetActive(true);

    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
    }
}
