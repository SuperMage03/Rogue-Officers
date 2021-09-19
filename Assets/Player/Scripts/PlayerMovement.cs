using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public CharacterController cc;
    public Transform cam;
    public Animator anime;
    
    public float walkSpeed = 3f;
    public float runSpeed = 6f;
    public float terminalVel = -100f;
    public float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;
    public float hp = 300f;

    private float turnSmoothVelocity; 
    private float netGravityVel = 0f;
    
    private static readonly int AnimeForward = Animator.StringToHash("Forward");

    public override void OnStartClient() {
        anime = GetComponent<Animator>();
    }

    [Client]
    void Update()
    {
        
        /*
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if(Input.GetKey(KeyCode.Escape)) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.None;
        }*/


        if(!hasAuthority) return;

        // Movement
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && !anime.GetCurrentAnimatorStateInfo(0).IsName("m_pistol_shoot"))
        {
            float lookAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, lookAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, lookAngle, 0f) * Vector3.forward;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                cc.Move(moveDir * runSpeed * Time.deltaTime);
                anime.SetInteger(AnimeForward, 2);
            }

            else
            {
                cc.Move(moveDir * walkSpeed * Time.deltaTime);
                anime.SetInteger(AnimeForward, 1);
            }


            // Animation
            
        }

        else
        {
            // Animation
            anime.SetInteger(AnimeForward, 0);
        }
        

        // Gravity
        netGravityVel = !cc.isGrounded ? Mathf.Max(netGravityVel + gravity * Time.deltaTime, terminalVel) : 0f;
        cc.Move(Vector3.up * netGravityVel);
    }
}
