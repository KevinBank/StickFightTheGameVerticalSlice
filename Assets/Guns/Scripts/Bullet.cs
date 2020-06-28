using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float velocity;
    [SerializeField] private LayerMask layer;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.right * velocity * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.transform.tag == "Player")
        {
            collision.gameObject.SendMessage("TakeDamage", 1);
            //Instantiate(particle);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
        
    }

    public void SetBulletValues(float damage, float velocity)
    {
        this.damage = damage;
        this.velocity = velocity;
    }
}
