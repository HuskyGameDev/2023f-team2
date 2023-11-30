using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pauseButton;
    public void PlayMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void PauseButton()
    {
       Time.timeScale = 0f;
    }
    public void ResumeButton()
    {
        Time.timeScale = 1.0f;
    }
}
