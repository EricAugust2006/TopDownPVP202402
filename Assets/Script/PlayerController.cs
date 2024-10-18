using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{

    public NetworkVariable<int> playerId;


    [SerializeField] GameObject playerCam;

    [SerializeField] GameObject projectilePrefab;
    
    [SerializeField] HealthController healthController;
    
    [SerializeField] float speed;

    [SerializeField] float projectileSpeed;

    Animator animator;

    Rigidbody2D rb;

    float vAxis, hAxis, atkDirH, atkDirV;

    Vector2 lastMove = Vector2.down;

    public override void OnNetworkSpawn()
    {
        // Somente o servidor deve definir o playerId
        if (IsServer)
        {
            SetPlayerId();
        }

        // Debug para verificar o ID
        Debug.Log("Player ID após o spawn: " + playerId.Value);
    }

    // Atribui o playerId no servidor
    private void SetPlayerId()
    {
        playerId.Value = Random.Range(1, int.MaxValue);  // O servidor sorteia o valor
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        Debug.LogWarning("Meu playerId é :"+playerId.Value);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCam.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner && !healthController.isMorto.Value) return;
        
        Move();
        Attack();
    }

    void Attack() 
    {
        atkDirH = Input.GetAxisRaw("Horizontal");
        atkDirV = Input.GetAxisRaw("Vertical");

        Vector2 atkDir = new Vector2(atkDirH, atkDirV).normalized;

        if (atkDir != Vector2.zero)
        {
            lastMove = atkDir;
        }

        if (Input.GetButtonDown("Fire1")) 
        {

            // Define a rotação do projetil baseado na direção do último movimento
            float angle = Mathf.Atan2(lastMove.y, lastMove.x) * Mathf.Rad2Deg;

            animator.SetTrigger("Atk");

            AttackServerRpc(lastMove, angle, playerId.Value);
        }
    }

    [ServerRpc]
    void AttackServerRpc(Vector2 projDirection, float angle, int id) 
    {

        Quaternion rotation = Quaternion.Euler(0, 0, angle);

        GameObject projectile = Instantiate(projectilePrefab,
                                    transform.position,
                                    rotation);

        ProjectileController projectileController =
            projectile.GetComponent<ProjectileController>();

        projectileController.SetProjectile(projDirection, projectileSpeed, id);

        NetworkObject projectileNetworkObject = projectile.GetComponent<NetworkObject>();

        projectileNetworkObject.Spawn();

    }

    void Move() 
    {
        hAxis = Input.GetAxis("Horizontal");
        vAxis = Input.GetAxis("Vertical");

        if(Mathf.Abs(hAxis) >= 0.01) {
            animator.SetFloat("X", hAxis);
            animator.SetFloat("Y", 0);
        }

        if (Mathf.Abs(vAxis) >= 0.01) {
            
            animator.SetFloat("Y", vAxis);
            animator.SetFloat("X", 0);
        }

        Vector2 newvelocity = new Vector2(hAxis, vAxis).normalized;

        rb.velocity = newvelocity * speed;
    }

}
