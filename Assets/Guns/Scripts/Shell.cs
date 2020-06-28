using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private Vector3 shellVel;
    [SerializeField] private Vector3 shellVelRRange;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
        rb.velocity = new Vector3(
            shellVel.x + Random.Range(-shellVelRRange.x, shellVelRRange.x),
            shellVel.y + Random.Range(-shellVelRRange.y, shellVelRRange.y),
            shellVel.z + Random.Range(-shellVelRRange.z, shellVelRRange.z));
    }

    public void SetShellValues(Vector3 shellVel, Vector3 shellVelRRange)
    {
        this.shellVel = shellVel;
        this.shellVelRRange = shellVelRRange;
    }
}
