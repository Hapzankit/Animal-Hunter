using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerHealth = 100;

    public void TakeDamage(int damageAmount)
    {
        if (playerHealth > 0)
        {
            playerHealth -= damageAmount;
        }
    }
}
