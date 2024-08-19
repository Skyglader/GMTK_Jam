using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseButtons : MonoBehaviour
{
    public void Restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Game");
    }

    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Main Menu");
    }

    public void StartButton()
    {
        Animator fadeIn = GameObject.Find("Fade IN").GetComponentInChildren<Animator>();

        fadeIn.CrossFade("FadeIn", 0.1f);
    }

    public void HowToPlay()
    {

    }

    public void Settings()
    {

    }
}
