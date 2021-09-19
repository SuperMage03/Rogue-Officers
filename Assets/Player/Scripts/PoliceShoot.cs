using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PoliceShoot : NetworkBehaviour
{
    private PlayerDriveCar playerDriveCar;
    public GameObject aimCam;
    public GameObject crosshair;
    public float bulletDamage = 10f;
    public Camera mainCam;
    private Animator anime;
    private static readonly int Aiming = Animator.StringToHash("Aiming");
    private static readonly int Shoot = Animator.StringToHash("Shoot");

    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        playerDriveCar = GetComponent<PlayerDriveCar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority || playerDriveCar.inCar) return;
        if (Input.GetMouseButton(1))
        {
            aimCam.SetActive(true);
            crosshair.SetActive(true);
            Ray screenCenterRay = mainCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, mainCam.nearClipPlane));
            transform.LookAt(screenCenterRay.direction * 1000f);
            transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

            if (Input.GetMouseButtonDown(0))
            {
                anime.SetTrigger(Shoot);
                if (Physics.Raycast(mainCam.transform.position, screenCenterRay.direction, out var hit, Mathf.Infinity))
                {
                    if (hit.transform.gameObject.CompareTag("NPC"))
                    {
                        NPCMovement npcMovement = hit.transform.gameObject.GetComponent<NPCMovement>();
                        npcMovement.gotHit = true;
                        npcMovement.gotHitBy = transform.gameObject;
                        npcMovement.hp -= bulletDamage;
                    }
                }
            }
        }

        else
        {
            aimCam.SetActive(false);
            crosshair.SetActive(false);
        }

        anime.SetBool(Aiming, Input.GetMouseButton(1));
    }
}
