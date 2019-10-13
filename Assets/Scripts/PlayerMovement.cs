using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    public float movementSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    #endregion

    Animator anim;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            anim.SetInteger("anima", 1);
        }
        else
        {
            Flip();
            anim.SetInteger("anima", 2);
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            anim.SetInteger("anima", 3);
            movement.x = 0;
            movement.y = 0;
        }
    }

    void Flip()
    {
        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    private void FixedUpdate() => rb.MovePosition(rb.position + Time.fixedDeltaTime * movementSpeed * movement);
}
