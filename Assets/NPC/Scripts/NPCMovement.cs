using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Mirror;

public class NPCMovement : NetworkBehaviour
{
    public float groundBuffer = 0.05f;
    public bool gotHit = false;
    public GameObject gotHitBy = null;
    public float hp;
    public float maxHp = 100f;
    public float punchDamage = 10f;

    public float runSpeed = 5f, walkSpeed = 2f;
    private Animator anime;
    
    private float punchCheckTime = Mathf.Infinity;
    private float punchCheckBufferTime = 0.85f;

    private NavMeshAgent navMeshAgent;
    private static readonly int Forward = Animator.StringToHash("Forward");
    private static readonly int Punch = Animator.StringToHash("Punch");
    private static readonly int Death = Animator.StringToHash("Death");

    [SerializeField] private GameObject healthBarUI;
    public Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        anime = GetComponent<Animator>();
        StartCoroutine(NPCRandMovement());

        hp = maxHp;
        slider.value = CalculateHealth();
        healthBarUI.SetActive(false);

    }

    [ServerCallback]
    private void Update()
    {
        slider.value = CalculateHealth();

        if(hp < maxHp) healthBarUI.SetActive(true);
        if(hp > maxHp) hp = maxHp;

        if (hp <= 0f)
        {
            navMeshAgent.SetDestination(navMeshAgent.transform.position);
            anime.SetBool(Death, true);
            Destroy(gameObject, 4f);
            return;
        }

        if (punchCheckTime <= Time.time)
        {
            if (Mathf.Abs(Vector3.Distance(navMeshAgent.transform.position, gotHitBy.transform.position)) <= 1.1f)
            { 
                PlayerMovement playerMovement = gotHitBy.gameObject.GetComponent<PlayerMovement>();
                playerMovement.hp -= punchDamage;
                if (playerMovement.hp <= 0f)
                {
                    gotHit = false;
                    gotHitBy = null;
                    StartCoroutine(NPCRandMovement());
                }
            }
            punchCheckTime = Mathf.Infinity;
        }

        if (gotHit)
        {

            healthBarUI.transform.rotation = Quaternion.LookRotation(transform.position - gotHitBy.transform.position);

            if (Mathf.Abs(Vector3.Distance(navMeshAgent.transform.position, gotHitBy.transform.position)) > 1f)
            {
                navMeshAgent.SetDestination(gotHitBy.transform.position);
            }

            else
            {
                anime.SetTrigger(Punch);
                punchCheckTime = float.IsPositiveInfinity(punchCheckTime) ? Time.time + punchCheckBufferTime : punchCheckTime;
                navMeshAgent.SetDestination(navMeshAgent.transform.position);
            }
        }

        if (navMeshAgent.velocity.magnitude > 0f)
        {
            anime.SetInteger(Forward, gotHit ? 2 : 1);
            navMeshAgent.speed = gotHit ? runSpeed : walkSpeed;
        }

        else
        {
            anime.SetInteger(Forward, 0);
        }
    }

    IEnumerator NPCRandMovement()
    {
        while (true)
        {
            if (gotHit) yield break;
            navMeshAgent.SetDestination(getRandPos());
            yield return new WaitForSeconds(15f);
        }
    }

    Vector3 getRandPos()
    {
        while (true)
        {
            int xPos = Random.Range(-100, 100);
            int zPos = Random.Range(-100, 100);
            
            if (Physics.Raycast(new Vector3(xPos, 300f, zPos), Vector3.down, out var groundHit))
            {
                GameObject hitObject = groundHit.transform.gameObject;
                Transform hitTransform = groundHit.transform;
                if (hitObject.name.Substring(0, 4) == "Road")
                {
                    return new Vector3(xPos, hitTransform.position.y + groundBuffer, zPos);
                }
            }
        }


    }


    float CalculateHealth() {
        return hp / maxHp;
    }

}
