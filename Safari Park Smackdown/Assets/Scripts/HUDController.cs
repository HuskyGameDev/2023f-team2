using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    [SerializeField] private GameObject player1WinCounter;
    [SerializeField] private GameObject player2WinCounter;

    [SerializeField] private GameObject player1HealthBar;
    [SerializeField] private GameObject player2HealthBar;

    private static HUDController hud;
    
    // Start is called before the first frame update
    void Start()
    {
        hud = this;
    }

    public static void UpdateWins()
    {
        hud.player1WinCounter.transform.localScale = new Vector3(GameController.Player1Wins, 1, 0);
        hud.player1WinCounter.transform.position = new Vector3(GameController.Player1Wins * .5f * -1, hud.player1WinCounter.transform.position.y, 0);

        hud.player2WinCounter.transform.localScale = new Vector3(GameController.Player2Wins, 1, 0);
        hud.player2WinCounter.transform.position = new Vector3(GameController.Player2Wins * .5f, hud.player2WinCounter.transform.position.y, 0);
    }

    public void Update()
    {
        float hpRatio = Mathf.Max(0, PlayerController.Player1.health / PlayerController.Player1.maxHealth);
        player1HealthBar.transform.localScale = new Vector3(hpRatio * 7, .75f, 0);
        player1HealthBar.transform.position = new Vector3(-8 + hpRatio * 3.5f, -4.25f, 0);

        Vector3 rgb = new Vector3(240, hpRatio * 80, hpRatio * 120) / 255f;
        player1HealthBar.GetComponent<SpriteRenderer>().color = new Color(rgb.x, rgb.y, rgb.z);
        rgb *= .8f;
        player1HealthBar.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(rgb.x, rgb.y, rgb.z);

        hpRatio = Mathf.Max(0, PlayerController.Player2.health / PlayerController.Player2.maxHealth);
        player2HealthBar.transform.localScale = new Vector3(hpRatio * 7, .75f, 0);
        player2HealthBar.transform.position = new Vector3(8 - hpRatio * 3.5f, -4.25f, 0);

        rgb = new Vector3(240, hpRatio * 80, hpRatio * 120) / 255f;
        player2HealthBar.GetComponent<SpriteRenderer>().color = new Color(rgb.x, rgb.y, rgb.z);
        rgb *= .8f;
        player2HealthBar.GetComponentsInChildren<SpriteRenderer>()[1].color = new Color(rgb.x, rgb.y, rgb.z);
        
    }
}
