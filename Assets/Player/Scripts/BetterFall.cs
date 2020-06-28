using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this class makes the player fall faster so the falling feels a but more real
/// </summary>
[RequireComponent(typeof(ActiveRagdollLegContoller))]
public class BetterFall : MonoBehaviour
{
    private ActiveRagdollLegContoller legController;
    [Tooltip("The effected rigedbody")]
    [SerializeField] private Rigidbody rigidbody;
    [Range(0,2)]
    [SerializeField] private float fallingVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        legController = this.GetComponent<ActiveRagdollLegContoller>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (legController.InAir && rigidbody.velocity.y < 0)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y - fallingVelocity, rigidbody.velocity.z);
        }
    }
}
