using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController gc;
    public static int RoundLimit = 5;
    public static int Player1Wins = 0;
    public static int Player2Wins = 0;

    public static bool Player1HasChosenUpgrade = false;
    public static bool Player2HasChosenUpgrade = false;

    // Start is called before the first frame update
    void Start()
    {
        gc = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player1HasChosenUpgrade && Player2HasChosenUpgrade)
            StartRound();
    }

    public static void StartRound()
    {
        Unpause();
        PlayerController.Player1.ResetPlayer();
        PlayerController.Player2.ResetPlayer();

        Player1HasChosenUpgrade = false;
        Player2HasChosenUpgrade = false;

        Debug.Log("Player 1 wins: " + Player1Wins + ", Player 2 wins: " + Player2Wins);
    }

    public static void EndRound()
    {
        Pause();

        if (PlayerController.Player1.health <= 0)
            Player2Wins++;
        else
            Player1Wins++;
        HUDController.UpdateWins();

        if (Player1Wins >= RoundLimit || Player2Wins >= RoundLimit)
            EndGame();
        else
        {
            PlayerController.CreateUpgradeCards();
        }
    }

    public static void Pause() 
    {
        Time.timeScale = 0;
        gc.transform.GetChild(0).gameObject.SetActive(true);
    }

    public static void Unpause()
    {
        Time.timeScale = 1;
        gc.transform.GetChild(0).gameObject.SetActive(false);
    }

    public static void EndGame()
    {
        gc.StartCoroutine(gc.GoToMain());
    }

    IEnumerator GoToMain()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
