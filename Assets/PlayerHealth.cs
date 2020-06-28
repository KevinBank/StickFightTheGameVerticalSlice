using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int health;

    private void TakeDamage(int damage)
    {
        health -= damage;
        if(health >= 0)
        {
            this.GetComponent<ActiveRagdollLegContoller>().SetActive(false);
            this.GetComponent<ActiveRagdollLegContoller>().enabled = false;
        }
    }
}
