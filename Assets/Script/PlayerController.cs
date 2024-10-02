using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{

    [SerializeField] GameObject projectilePrefab;
    
    [SerializeField] float speed;

    [SerializeField] float projectileSpeed;

    Animator animator;
    Rigidbody2D rb;

    float vAxis, hAxis, atkDirH, atkDirV;

    Vector2 lastMove = Vector2.down;

    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

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
            
            Quaternion rotation = Quaternion.Euler(0, 0, angle);

            animator.SetTrigger("Atk");

            GameObject projectile = Instantiate(projectilePrefab, 
                                        transform.position,
                                        rotation);

            ProjectileController projectileController =
                projectile.GetComponent<ProjectileController>();

            Collider2D collider = GetComponent<Collider2D>();

            projectileController.SetProjectile(lastMove, projectileSpeed, collider.GetInstanceID());

        }
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
