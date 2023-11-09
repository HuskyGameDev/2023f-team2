using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    private static List<HUDController> these = new List<HUDController>();
    
    [SerializeField] private bool isForPlayer1;
    
    // Start is called before the first frame update
    void Start()
    {
        these.Add(this);
    }

    public static void UpdateWins()
    {
        foreach (HUDController g in these)
            g.ChangeSize();
    }

    private void ChangeSize()
    {
        int width = isForPlayer1 ? GameController.Player1Wins : GameController.Player2Wins;
        transform.localScale = new Vector3(width, 1, 0);
        transform.position = new Vector3(width * .5f * (isForPlayer1 ? -1 : 1), transform.position.y, 0);
    }
}
