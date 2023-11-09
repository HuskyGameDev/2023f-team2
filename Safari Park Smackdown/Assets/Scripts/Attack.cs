using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Generic Attack Script
 * Put on attack collider objects to allow them to have individual damage and hit stun values
 * that can be read by the receiving collision
 */
public class Attack : MonoBehaviour
{
    public float damage;
    [SerializeField] private float hitStun;

    public float GetDamage()
    {
        return damage;
    }

    public float GetHitStun()
    {
        return hitStun;
    }
}
