using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerDriveCar playerDriveCar;
    private bool isNearCar = false;
    private Collider otherCar;

    // Start is called before the first frame update
    void Start()
    {
        playerDriveCar = gameObject.GetComponent<PlayerDriveCar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("PRESSED E");

            if (playerDriveCar.inCar)
            {
                Debug.Log("Exit car!");
                playerDriveCar.EnableUserInCar();
                isNearCar = false;
                otherCar = null;
            }
            else if (isNearCar)
            {
                Debug.Log("In car!");
                playerDriveCar.DisableUserInCar(otherCar.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Drivable Car"))
        {
            isNearCar = true;
            otherCar = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Drivable Car"))
        {
            isNearCar = false;
            otherCar = null;
        }
    }
}