using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveRagdollArmController : MonoBehaviour
{
    [SerializeField] private Vector3 aim;
    [SerializeField] private GameObject gun;
    public Vector2 Aim { set { aim = value; } }
    private enum Input
    {
        MOUSE,
        CONTROLLER
    }
    [SerializeField] private Input input;
    [SerializeField] private Transform armIndex;
    [SerializeField] private ConfigurableJoint joint;

    private void Update()
    {
        if(input == Input.MOUSE)
        {
            aim.z = 10f;
            aim = Camera.main.ScreenToWorldPoint(aim);
        }
        else
        {
            aim += armIndex.position;
        }

        armIndex.LookAt(aim);
        joint.targetRotation = armIndex.rotation.normalized;
    }

    public void PickUp()
    {
        gun.SetActive(true);
    }
}
