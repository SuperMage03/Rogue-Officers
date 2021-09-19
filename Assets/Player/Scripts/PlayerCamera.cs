using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private CinemachineFreeLook cmfl;

    public PlayerDriveCar playerDriveCar;
    // Start is called before the first frame update
    void Start()
    {
        cmfl = GetComponent<CinemachineFreeLook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDriveCar.inCar)
        {
            cmfl.m_Orbits[1].m_Height = 0.30f;
            cmfl.m_Orbits[1].m_Radius = 5f;
        }

        else
        {
            cmfl.m_Orbits[1].m_Height = 0.55f;
            cmfl.m_Orbits[1].m_Radius = 3.52f;
        }
    }
}
