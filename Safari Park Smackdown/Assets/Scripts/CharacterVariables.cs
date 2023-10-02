using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterVariables : ScriptableObject
{
    [Header("Character Constants")]
    public static string NAME;
    public static string STORY;
    public static int MAX_HEALTH;
    public static string WEIGHT; // "light" "medium" "heavy" 
    public static int SPEED;
    public static int JUMP_HEIGHT;

    [Header("Character Variables")]
    public int health;
    



    
}
