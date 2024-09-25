using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    int shooterId;

    Vector2 direction;

    float speed;

    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        Destroy(gameObject, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int id = collision.GetInstanceID();
        if (collision.tag == "Player" && shooterId != id) 
        { 
            HealthController healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(1);
        }
    }

    public void SetProjectile(Vector2 dir, float spd, int originId) 
    {
        speed = spd;
        direction = dir;
        shooterId = originId;
    }

}
