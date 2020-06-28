using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    [SerializeField] private GameObject player1Particle, player2Particle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            Instantiate(player1Particle, other.transform.position, this.transform.rotation);
            other.GetComponentInParent<PlayerHealth>().SendMessage("TakeDamage", 100f);
            Destroy(other.gameObject);
        }

        else if (other.gameObject.layer == 10)
        {
            Instantiate(player2Particle, other.transform.position, this.transform.rotation);
            other.GetComponentInParent<PlayerHealth>().SendMessage("TakeDamage", 100f);
            Destroy(other.gameObject);
        }
    }
}