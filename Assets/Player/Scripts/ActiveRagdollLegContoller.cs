using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The class is responseble for all the movement and physics of the legs this includes the ground check jumping and walking
/// </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(ActiveRagdollFeetController))]
public class ActiveRagdollLegContoller : MonoBehaviour
{
    #region Variables
    #region Input
    private float inputMove;
    public float InputMove
    {
        set { inputMove = Mathf.Clamp(value, -1, 1); }
    }
    #endregion
    #region Balance
    [Header("Balance")]
    [SerializeField] private Rigidbody pelvis;
    [SerializeField] private Rigidbody[] legs;
    [SerializeField] private ActiveRagdollState mode = ActiveRagdollState.ACTIVE;
    private ActiveRagdollState currentMode = ActiveRagdollState.ACTIVE;
    #endregion
    #region Movement
    [Header("Movement")]
    [SerializeField] private bool canJump;
    [SerializeField] private float moveSpeedGround, moveSpeedAir, jumpForce, jumpDelay;
    private Animator anim;
    #endregion
    #region Ground Check
    [Header("Ground Check")]
    [SerializeField] private bool inAir;
    public bool InAir
    {
        get { return inAir; }
        set { this.inAir = value; }
    }
    [SerializeField] private LayerMask layer;
    [SerializeField] private float range;
    [SerializeField] private Vector3 size;
    [SerializeField] private Vector3 offset;
    private bool boxHit, rayHitDown, rayHitLeft, rayHitRight = false;
    private ActiveRagdollFeetController feetController;
    #endregion
    #endregion

    private void Awake()
    {
        anim = this.GetComponent<Animator>();
        feetController = this.GetComponent<ActiveRagdollFeetController>();
    }

    private void FixedUpdate()
    {
        if (!inAir)
        {
            Balance();
            SetActive(true);
        }
        else
        {
            SetActive(false);
            canJump = false;
        }
        GroundCheck();
        Move();
    }
    private void Update()
    {
        //this if statment checks if the ActiveRagdollState has been changed in the inspector if so it will call the SetActive function
        if (currentMode != mode)
        {
            if (mode == ActiveRagdollState.ACTIVE)
                SetActive(true);
            else
                SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        //this draws the gizmos for the raycast that is part of the 3 groundchecks
        DrawRay(rayHitDown, pelvis.transform.position, -Vector3.up * range);
        DrawRay(rayHitRight, pelvis.transform.position, Vector3.right * (range / 2.5f));
        DrawRay(rayHitLeft, pelvis.transform.position, -Vector3.right * (range / 2.5f));

        //this draws the gizmos for the OverlapBox that is part of the 3 groundchecks
        if (boxHit)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawWireCube(pelvis.transform.position + offset, size);
    }
    private void DrawRay(bool value, Vector3 from, Vector3 direction)
    {
        if (value)
            Gizmos.color = Color.green;
        else
            Gizmos.color = Color.red;
        Gizmos.DrawRay(from, direction);
    }

    /// <summary>
    /// All this function does is set the pelvis velocity to 0 on the x and z axis there for it is importend that this is the first
    /// function to be called in the FixedUpdate so that other funtcions can override it
    /// </summary>
    private void Balance()
    {
        pelvis.velocity = new Vector3(0, pelvis.velocity.y, 0);
    }
    /// <summary>
    /// This checks if the player is on the ground it does this in 3 different ways the first is a raycast straight down.
    /// the second is a overlap sphere on all of the feet and the final methode is a overlap box.
    /// all of these combined give a very accurate prediction if the player is on or close to the ground.
    /// 
    /// if any of the groundchecks detect something it will increase the numberOfHits by 1
    /// then when numberOfHits is not 0 it will set InAir to false
    /// </summary>
    private void GroundCheck()
    {
        rayHitRight = false;
        rayHitLeft = false;
        rayHitDown = false;
        boxHit = false;

        RaycastHit hit;

        //if (Physics.BoxCast(pelvis.transform.position))

        if (Physics.Raycast(pelvis.transform.position, Vector3.right, out hit, range / 2.5f, layer))
        {
            rayHitRight = true;
            inAir = false;
            StartCoroutine(SetCanJump(true, jumpDelay));
            return;
        }
        else rayHitRight = false;

        if (Physics.Raycast(pelvis.transform.position, -Vector3.right, out hit, range / 2.5f, layer))
        {
            rayHitLeft = true;
            inAir = false;
            StartCoroutine(SetCanJump(true, jumpDelay));
            return;
        }
        else rayHitLeft = false;

        //raycast
        if (Physics.Raycast(pelvis.transform.position, -Vector3.up, out hit, range, layer))
        {
            rayHitDown = true;
            inAir = false;
            StartCoroutine(SetCanJump(true, jumpDelay));
            return;
        }
        else rayHitDown = false;

        //overlap sphere
        foreach (GameObject foot in feetController.Feet)
        {
            if (foot.GetComponent<Foot>().Hit)
            {
                inAir = false;
                return;
            } 
        }

        //overlap box
        Collider[] colliders = Physics.OverlapBox(pelvis.transform.position + offset, size / 2, pelvis.transform.rotation, layer);
        if (colliders.Length > 0)
        {
            boxHit = true;
            inAir = false;
            return;
        }
        else boxHit = false;

        inAir = true;
    }
    /// <summary>
    /// This move the player using the input variable.
    /// when the player is on the ground it will use moveSpeedGround and play the animation
    /// but when the player is in the air it will use moveSpeedAir and disable the animator
    /// </summary>
    private void Move()
    {
        if (!inAir)//Movement for when the player is on the ground
        {
            anim.enabled = true;
            if (inputMove != 0)
            {
                pelvis.velocity += new Vector3(inputMove, 0.0f, 0f) * moveSpeedGround;
                anim.SetFloat("Move", inputMove);
                anim.SetBool("Idle", false);
            }
            else
                anim.SetBool("Idle", true);
        }
        else //Movement for when the player is in the air
        {
            if (inputMove != 0)
            {
                pelvis.velocity += new Vector3(inputMove, 0f, 0f) * moveSpeedAir;
                anim.enabled = false;
            }
        }
    }
    public void Jump()
    {
        if (canJump)
        {
            if(rayHitLeft)
            {
                Debug.Log("1");
                pelvis.velocity += new Vector3(jumpForce, jumpForce, 0f);
            }
            else if(rayHitRight)
            {
                Debug.Log("2");
                pelvis.velocity += new Vector3(-jumpForce, jumpForce, 0f);
            }
            else if(rayHitDown)
            {
                Debug.Log("0");
                pelvis.velocity += new Vector3(0f, jumpForce, 0f);
            }
            canJump = false;
        }
    }
    /// <summary>
    /// this sets all the ActiveRagdollState to either active or inactive depending on the param.
    /// it will also call the SetJointValues function
    /// </summary>
    /// <param name="value"></param>
    public void SetActive(bool value)
    {
        if (value)
        {
            mode = ActiveRagdollState.ACTIVE;
            currentMode = ActiveRagdollState.ACTIVE;

            SetJointValues(1000f, 200f, 0f);
        }
        else
        {
            mode = ActiveRagdollState.INACTIVE;
            currentMode = ActiveRagdollState.INACTIVE;

            SetJointValues(0f, 0f, 1f);
        }
    }
    /// <summary>
    /// This function gets called when the legs are being set active or inactive and it will set all the joints releted to the legs and balancing
    /// Effected Joints: Pelvis, R_Upper_Leg, R_Lower_Leg, L_Upper_Leg, L_Lower_leg.
    /// </summary>
    /// <param name="pelvisSpring"></param>
    /// <param name="legSpring"></param>
    /// <param name="legDampen"></param>
    private void SetJointValues(float pelvisSpring, float legSpring, float legDampen)
    {
        JointDrive jointDrive = new JointDrive();
        jointDrive.positionSpring = pelvisSpring;
        jointDrive.positionDamper = 0f;
        jointDrive.maximumForce = 3.402823e+38f;

        pelvis.GetComponent<ConfigurableJoint>().angularXDrive = jointDrive;
        pelvis.GetComponent<ConfigurableJoint>().angularYZDrive = jointDrive;

        jointDrive.positionSpring = legSpring;
        jointDrive.positionDamper = legDampen;
        jointDrive.maximumForce = 3.402823e+38f;

        foreach (Rigidbody leg in legs)
        {
            leg.GetComponent<ConfigurableJoint>().angularXDrive = jointDrive;
            leg.GetComponent<ConfigurableJoint>().angularYZDrive = jointDrive;
        }
    }

    private IEnumerator SetCanJump(bool value, float delay)
    {
        yield return new WaitForSeconds(delay);
        canJump = value;
        StopCoroutine(SetCanJump(true, 0f));
    }
}