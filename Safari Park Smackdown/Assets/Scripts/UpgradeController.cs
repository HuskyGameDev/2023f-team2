using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeController : MonoBehaviour
{
    [System.NonSerialized] public GameObject self;
    [System.NonSerialized] public bool isForPlayer1;
    public bool isRepeatable;
    public string upgradeType;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        foreach(SpriteRenderer sprite in gameObject.GetComponentsInChildren<SpriteRenderer>())
            sprite.color = Color.green;
    }

    private void OnMouseUp()
    {
        PlayerController player = isForPlayer1 ? PlayerController.Player1 : PlayerController.Player2;
        player.currentUpgradeList.Add(self);
        if(!isRepeatable)
            player.possibleUpgradeList.Remove(self);

        switch(upgradeType)
        {
            case "JabDmgInc":
                player.transform.Find("JabCollider").GetComponent<Attack>().damage += 5;
                break;
            case "Jump":
                player.hasJump = true;
                break;
            default: 
                Debug.LogError("Upgrade \"" + upgradeType + "\" not found.");
                break;
        }

        foreach(GameObject card in GameObject.FindGameObjectsWithTag("Upgrade"))
            if(card.GetComponent<UpgradeController>().isForPlayer1 == isForPlayer1 && card != this)
            {
                Destroy(card);
            }

        if(isForPlayer1)
            GameController.Player1HasChosenUpgrade = true;
        else
            GameController.Player2HasChosenUpgrade = true;

        Destroy(this);
    }
}
