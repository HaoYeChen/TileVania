using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3;
    [SerializeField] int score = 0;

    [SerializeField] Text livesText;
    [SerializeField] Text scoreText;

    //test
    public int deaths = 0;
    public Text deathText;

    [SerializeField] Image[] hearts;

    [SerializeField] float restartDelay = 4f; 
    private void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        livesText.text = playerLives.ToString();
        scoreText.text = score.ToString();
    }

    void Update()
    {
        DisplayHud();
    }

    private void DisplayHud()
    {
        for (int i = 0; i < playerLives; i++)
        {
            hearts[i].enabled = true;
            if (playerLives < hearts.Length)
                hearts[playerLives].enabled = false;
            
        }
        scoreText.text = score.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        //scoreText.text = score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            //TakeLife();
            Invoke("TakeLife", restartDelay);
        }
        else
        {
            //ResetGameSession();

            ReloadCurrentScene();
        }
    }
    public void IncreaseDeaths()
    {
        deaths += 1;
        deathText.text = deaths.ToString();
    }


    private void TakeLife()
    {
        playerLives--;
        var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        livesText.text = playerLives.ToString();
    }

    //private void ResetGameSession()
    //{
    //    SceneManager.LoadScene(0);
    //    Destroy(gameObject);
    //}

    private void ReloadCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

}
