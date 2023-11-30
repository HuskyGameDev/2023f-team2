using System.Collections;
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

    public static bool isPaused = false;
    public static bool isBetweenRounds = false;

    public GameObject pauseMenu;

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
        Pause();    //unpause
        isBetweenRounds = false;
        PlayerController.Player1.ResetPlayer();
        PlayerController.Player2.ResetPlayer();

        Player1HasChosenUpgrade = false;
        Player2HasChosenUpgrade = false;

        Debug.Log("Player 1 wins: " + Player1Wins + ", Player 2 wins: " + Player2Wins);
    }

    public static void EndRound()
    {
        if (!isPaused && !isBetweenRounds)
        {
            isBetweenRounds = true;
            Pause();

            if (PlayerController.Player1.health <= 0)
                Player2Wins++;
            else
                Player1Wins++;
            HUDController.UpdateWins();

            if (Player1Wins >= RoundLimit || Player2Wins >= RoundLimit)
                WinGame();
            else
            {
                PlayerController.CreateUpgradeCards();
            }
        }
    }

    public static void Pause() 
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        gc.transform.GetChild(0).gameObject.SetActive(isPaused);
        if(!isPaused )
        {
            gc.pauseMenu.SetActive(false);
        }
        Debug.Log("Game is now " + (isPaused ? "Paused" : "Unpaused"));
    }

    public static void WinGame()
    {
        gc.StartCoroutine(gc.WaitForWin());
    }

    public static void EndGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    IEnumerator WaitForWin()
    {
        
        yield return new WaitForSecondsRealtime(2);
        EndGame();
    }
}
