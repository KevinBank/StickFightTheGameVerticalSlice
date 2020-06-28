using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUp : MonoBehaviour
{
    [SerializeField] private float RotationSpeed;

    private void Update()
    {
        transform.Rotate(Vector3.up * (RotationSpeed * Time.deltaTime));
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer != 11)
        {
            other.GetComponentInParent<ActiveRagdollArmController>().PickUp();
            Destroy(this.gameObject);
        }
    }
}
