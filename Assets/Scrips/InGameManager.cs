using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InGameManager : MonoBehaviour
{
    [Header("Pannels")]
    public GameObject PausePannel;
    public GameObject WinPannel;
    public GameObject LosePannel;

    public AudioSource gameMusic;
    public Text ScoreTxt;
    int playerScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        playerScore = 0;
        ScoreTxt.text = "Score: "+playerScore;

        PausePannel.SetActive(false);
        WinPannel.SetActive(false);
        LosePannel.SetActive(false);
    }

    public void PausePressed()
    {
        FindObjectOfType<AudioManager>().Play("select");

        if (Time.timeScale != 0)
        {
            //Pause
            gameMusic.Pause();
            Time.timeScale = 0;
            PausePannel.SetActive(true);
        }
        else
        {
            //Resume
            gameMusic.Play();
            Time.timeScale = 1;
            PausePannel.SetActive(false);
        }
    }

    public void RestartPressed()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void WinGame()
    {
        FindObjectOfType<AudioManager>().Play("win");

        // Win
        gameMusic.Pause();
        Time.timeScale = 0;
        WinPannel.SetActive(true);
    }

    public void GameOver()
    {
        FindObjectOfType<AudioManager>().Play("lose");

        // Lose
        gameMusic.Pause();
        Time.timeScale = 0;
        LosePannel.SetActive(true);
    }

    public void IncrementScore(int addBy)
    {
        playerScore += addBy;
        ScoreTxt.text = "Score: " + playerScore;
    }
}
