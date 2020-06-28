using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Foot : MonoBehaviour
{
    [SerializeField] private ActiveRagdollFeetController controller;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask layer;
    private GameObject particle;
    private bool ready;
    private bool hit = false;
    public bool Hit { get { return this.hit; } }

    private void Start()
    {
        particle = controller.Particle;
        ready = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(particle != null && ready)
        {
            Instantiate(particle, this.transform.position, this.transform.rotation);
            ready = false;
            StartCoroutine(Delay());
        }
    }
    private void FixedUpdate()
    {
        GroundCheck();
    }
    private void OnDrawGizmos()
    {
        if (hit)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, radius);
    }

    private void GroundCheck()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius, layer);
        hit = colliders.Length > 0 ? true : false;

        /*if (colliders.Length > 0)
            hit = true;
        else
            hit = false;
            */
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.2f);
        ready = true;
        StopCoroutine(Delay());
    }
}