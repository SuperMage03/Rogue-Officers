using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerCameraController : NetworkBehaviour
{

    private Camera mainCamera;
    private GameObject playerCamera;

    private bool setup = false;
    private int clientID;

    public override void OnStartClient() {
        mainCamera = transform.Find("Main Camera").GetComponent<Camera>();
        playerCamera = transform.Find("Player Camera").gameObject;

        AssignID(GetComponent<PlayerInformation>().clientID);
    }

    [Client]
    public void AssignID(int id) {
        clientID = id;
    }

    [Client]
    void Update() {

        if(!hasAuthority) return;

        mainCamera.gameObject.SetActive(true);
        playerCamera.SetActive(true);

        if(setup || mainCamera == null || playerCamera == null) return;

        playerCamera.layer = clientID+10;
        for(int i = 1; i <= 5; i++) {
            if(clientID != i) mainCamera.cullingMask = mainCamera.cullingMask ^ (1 << (i+10));
        }

        setup = true;
    }



    
    
}
