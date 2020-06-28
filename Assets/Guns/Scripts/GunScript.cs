using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    #region Variabels
    #region Input
    private bool trigger;
    //trigger is onButton
    public bool Trigger
    {
        set { trigger = value; }
    }
    #endregion
    #region Objects
    [Header("Objects")]
    [Tooltip("A Bullet prefab that wil be shot.")]
    [SerializeField] private GameObject bullet;
    [Tooltip("A Shell prefab that wil be Emitted.")]
    [SerializeField] private GameObject shell;
    [Tooltip("The position that the Bullet will be spawned.")]
    [SerializeField] private Transform bulletSpawner;
    [Tooltip("The position that the Shell will be spawned.")]
    [SerializeField] private Transform shellEmitter;
    [Tooltip("The rigidbody of the gun for recoil")]
    [SerializeField] private Rigidbody rb;
    #endregion
    #region Gun Stats
    [Header("Gun Stats")]
    [SerializeField] private FireMode fireMode;
    [Tooltip("The total amount of Ammo the Gun has.")]
    [SerializeField] private int ammo;
    [Tooltip("The amount of force the gun applies on itself upon firing")]
    [SerializeField] private float recoil;
    [Tooltip("The damage that every Bullet will deal.")]
    [SerializeField] private float damage;
    [Tooltip("The speed of the fired bullet")]
    [SerializeField] private float velocity;
    [Tooltip("The amount of time between shots in second.")]
    [Range(0,2)]
    [SerializeField] private float fireRate;
    [Tooltip("The amount of rotation applied to the Bullet Spawner.")]
    [SerializeField] private float spread;
    [Tooltip("The speed the shell will begin at on the X, Y and Z axis")]
    [SerializeField] private Vector3 shellVel;
    [Tooltip("Adds randomness to the emitted shell on the X, Y and Z axis")]
    [SerializeField] private Vector3 shellVelRRange;
    private bool canShoot;
    #endregion
    #region Spacials
    [Header("Spacials")]
    [Tooltip("The amount of Bullets that are shot when the Fire Mode is set to BURST.")]
    [SerializeField] private int burstAmount;
    private int currentBurstAmount;
    [Tooltip("The amount of time between the Bullets spawned in a single burst.")]
    [SerializeField] private float burstRate;
    [Tooltip("The Amount of Bullets spawned per shot.")]
    [SerializeField] private int bulletAmount = 1;
    #endregion
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        currentBurstAmount = burstAmount;
        canShoot = true;
        trigger = false;
    }

    private void Update()
    {
        //these two if statements are for debuging perposes
        if (Input.GetKeyDown(KeyCode.Mouse0))
            trigger = true;
        if (Input.GetKeyUp(KeyCode.Mouse0))
            trigger = false;

        if (trigger && canShoot)
            Shoot();
    }

    private void Shoot()
    {
        for (int i = 0; i < bulletAmount; i++)
        {
            bulletSpawner.localRotation = Quaternion.AngleAxis(180 + Random.Range(-spread, spread), bulletSpawner.forward);
            GameObject bulletIns = Instantiate(bullet, bulletSpawner.position, bulletSpawner.rotation);
            bulletSpawner.localRotation = Quaternion.AngleAxis(180, bulletSpawner.forward);
            bulletIns.GetComponent<Bullet>().SetBulletValues(damage, velocity);

            rb.AddForceAtPosition(transform.right * recoil, bulletSpawner.position);
            Debug.Log(transform.rotation.z);

            EmitShell();
        }
        if (fireMode == FireMode.SEMI_AUTOMATIC)
            trigger = false;
        StartCoroutine(CoolDown());
    }

    private void EmitShell()
    {
        GameObject shellIns = Instantiate(shell, shellEmitter.position, shellEmitter.rotation);
        shellIns.GetComponent<Shell>().SetShellValues(shellVel, shellVelRRange);
    }
    private IEnumerator CoolDown()
    {
        canShoot = false;
        if (fireMode == FireMode.BURST && currentBurstAmount - 1 > 0)
        {
            yield return new WaitForSeconds(burstRate);
            currentBurstAmount--;
            Shoot();
            StopCoroutine(CoolDown());
        }
        else
        {
            yield return new WaitForSeconds(fireRate);
            canShoot = true;
            currentBurstAmount = burstAmount;
            StopCoroutine(CoolDown());
        }
    }
}