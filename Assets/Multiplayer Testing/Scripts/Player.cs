using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private float speed = 2f;

    private CharacterController controller;

    public override void OnStartClient() {
        controller = GetComponent<CharacterController>();
    }

    [Client]
    void Update() {

        if(!hasAuthority) return;
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        controller.Move(movement * speed * Time.deltaTime);
        
    }

}
