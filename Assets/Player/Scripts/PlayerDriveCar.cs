using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class PlayerDriveCar : MonoBehaviour
{
    public bool inCar = false;
    public GameObject curCar = null;
    public Transform curCarTransform = null;

    private SkinnedMeshRenderer[] smr;
    private MeshRenderer[] mr;
    private CharacterController cc;
    private Transform playerTransform;
    private PlayerMovement playerMovement;

    private float horizontalIpt, verticalIpt;
    private float steeringAngle;
    private WheelCollider flwc, frwc, rlwc, rrwc;

    public float maxSteerAngle = 30f;
    private float motorTorque = 500f;
    private float brakeTorque = 1500f;

    // Start is called before the first frame update
    void Start()
    {
        cc = gameObject.GetComponent<CharacterController>();
        playerTransform = gameObject.GetComponent<Transform>();
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        smr = gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        mr = gameObject.GetComponentsInChildren<MeshRenderer>();
    }

    public void DisableUserInCar(GameObject car)
    {
        inCar = true;
        curCar = car;
        curCarTransform = car.GetComponent<Transform>();
        playerMovement.enabled = false;
        cc.enabled = false;
        foreach (SkinnedMeshRenderer curSmr in smr) curSmr.enabled = false;
        foreach (MeshRenderer curMr in mr) curMr.enabled = false;

        Transform wheelColliders = car.transform.Find("Wheel Colliders").transform;
        flwc = wheelColliders.Find("FLWC").gameObject.GetComponent<WheelCollider>();
        frwc = wheelColliders.Find("FRWC").gameObject.GetComponent<WheelCollider>();
        rlwc = wheelColliders.Find("RLWC").gameObject.GetComponent<WheelCollider>();
        rrwc = wheelColliders.Find("RRWC").gameObject.GetComponent<WheelCollider>();
    }


    public void EnableUserInCar()
    {
        Vector3 YRotToVectorAngle = new Vector3(Mathf.Cos((curCarTransform.eulerAngles.y + 90f) * Mathf.Deg2Rad), 0f,
            Mathf.Sin((curCarTransform.eulerAngles.y + 90f) * Mathf.Deg2Rad));
        inCar = false;
        playerTransform.position += YRotToVectorAngle.normalized * 2f + Vector3.up * 2f;
        playerTransform.rotation = Quaternion.Euler(playerTransform.eulerAngles.x, playerTransform.eulerAngles.y, 0f);
        playerMovement.enabled = true;
        cc.enabled = true;
        foreach (SkinnedMeshRenderer curSmr in smr) curSmr.enabled = true;
        foreach (MeshRenderer curMr in mr) curMr.enabled = true;

        curCar = null;
        curCarTransform = null;
    }


    // Update is called once per frame
    void Update()
    {
        if (!inCar) return;
        playerTransform.position = curCarTransform.TransformPoint(new Vector3(0f, 2f, -3f));
        playerTransform.rotation = curCarTransform.rotation;
    }

    void FixedUpdate()
    {
        if (!inCar) return;
        GetInput();
        Steer();
        Accelerate();
    }

    void GetInput()
    {
        horizontalIpt = Input.GetAxis("Horizontal");
        verticalIpt = Input.GetAxis("Vertical");
    }

    void Steer()
    {
        steeringAngle = maxSteerAngle * horizontalIpt;
        flwc.steerAngle = steeringAngle;
        frwc.steerAngle = steeringAngle;
    }

    void Accelerate()
    {
        flwc.motorTorque = verticalIpt * motorTorque;
        frwc.motorTorque = verticalIpt * motorTorque;

        if (Input.GetKey(KeyCode.Space))
        {
            rlwc.brakeTorque = brakeTorque;
            rrwc.brakeTorque = brakeTorque;
            flwc.brakeTorque = brakeTorque;
            frwc.brakeTorque = brakeTorque;
        }

        else
        {
            rlwc.brakeTorque = 0f;
            rrwc.brakeTorque = 0f;
            flwc.brakeTorque = 0f;
            frwc.brakeTorque = 0f;
        }
        
        Debug.Log(rlwc.brakeTorque + " " + brakeTorque);
    }
}