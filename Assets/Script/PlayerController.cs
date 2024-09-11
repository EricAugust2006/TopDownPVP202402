using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed;

    Animator animator;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
    }

    void Attack() 
    {
        if (Input.GetButtonDown("Fire1")) 
        {
            animator.SetTrigger("Atk");
        }
    }

    void Move() 
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        if(Mathf.Abs(hAxis) >= 0.01) {
            animator.SetFloat("X", hAxis);
            animator.SetFloat("Y", 0);
        }

        if (Mathf.Abs(vAxis) >= 0.01) {
            
            animator.SetFloat("Y", vAxis);
            animator.SetFloat("X", 0);
        }

        Debug.Log("X:" + animator.GetFloat("X"));
        Debug.Log("Y:" + animator.GetFloat("Y"));

        Vector2 newvelocity = new Vector2(hAxis, vAxis).normalized;

        rb.velocity = newvelocity * speed;
    }

}
