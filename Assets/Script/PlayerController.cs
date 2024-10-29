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
    SpriteRenderer spriteRenderer;

    float vAxis, hAxis;
    Vector2 lastMove = Vector2.down;

    private Color defaultColor = Color.white;

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            SetPlayerId();
        }

        SetPlayerColor();
        Debug.Log("Player ID após o spawn: " + playerId.Value);
    }

    private void SetPlayerId()
    {
        playerId.Value = Random.Range(1, int.MaxValue); 
    }

    private void SetPlayerColor()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (IsOwner && IsHost)
        {
            spriteRenderer.color = defaultColor;
        }
        else
        {
            Color randomColor = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );

            spriteRenderer.color = randomColor;
        }
    }

    void Start()
    {
        if (!IsOwner) return;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerCam.SetActive(true);
    }

    void Update()
    {
        if (!IsOwner || healthController.isMorto.Value) return;

        Move();
        Attack();
    }

    void Attack() 
    {
        float atkDirH = Input.GetAxisRaw("Horizontal");
        float atkDirV = Input.GetAxisRaw("Vertical");

        Vector2 atkDir = new Vector2(atkDirH, atkDirV).normalized;

        if (atkDir != Vector2.zero)
        {
            lastMove = atkDir;
        }

        if (Input.GetButtonDown("Fire1")) 
        {
            float angle = Mathf.Atan2(lastMove.y, lastMove.x) * Mathf.Rad2Deg;
            animator.SetTrigger("Atk");
            AttackServerRpc(lastMove, angle, playerId.Value);
        }
    }

    [ServerRpc]
    void AttackServerRpc(Vector2 projDirection, float angle, int id) 
    {
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject projectile = Instantiate(projectilePrefab, transform.position, rotation);

        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
        projectileController.SetProjectile(projDirection, projectileSpeed, id);

        NetworkObject projectileNetworkObject = projectile.GetComponent<NetworkObject>();
        projectileNetworkObject.Spawn();
    }

    void Move() 
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(hAxis) > 0.01f || Mathf.Abs(vAxis) > 0.01f)
        {
            animator.SetFloat("X", hAxis);
            animator.SetFloat("Y", vAxis);
            lastMove = new Vector2(hAxis, vAxis).normalized;
        }

        Vector2 newVelocity = new Vector2(hAxis, vAxis).normalized * speed;
        rb.velocity = newVelocity;
    }
}
