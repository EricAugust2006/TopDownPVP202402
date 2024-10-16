using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProjectileController : NetworkBehaviour
{
    public NetworkVariable<int> shooterId = new NetworkVariable<int>(0);

    public NetworkVariable<Vector2> direction;

    public NetworkVariable<float> speed;

    Rigidbody2D rb;

    public override void OnNetworkSpawn()
    {
        if (IsServer) 
        { 
            rb = GetComponent<Rigidbody2D>();
            rb.velocity = direction.Value * speed.Value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int id = collision.GetComponent<PlayerController>().playerId.Value;
        if (collision.tag == "Player" && shooterId.Value != id) 
        { 
            HealthController healthController = collision.GetComponent<HealthController>();
            healthController.TakeDamage(1);
        }
    }

    public void SetProjectile(Vector2 dir, float spd, int originId) 
    {
        speed.Value = spd;
        direction.Value = dir;
        shooterId.Value = originId;
    }

}
