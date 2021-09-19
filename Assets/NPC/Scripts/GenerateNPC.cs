using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GenerateNPC : NetworkBehaviour
{
    public GameObject npcPrefab;
    public int xPos, zPos;
    public int npcCount = 0;
    public float groundBuffer = 0.05f;

    private GameObject NPC;
    
    // Start is called before the first frame update
    public override void OnStartServer()
    {
        StartCoroutine(NPCDrop());
    }

    [ServerCallback]
    IEnumerator NPCDrop()
    {
        while (npcCount <= 20)
        {
            xPos = Random.Range(-100, 100);
            zPos = Random.Range(-100, 100);

            if (Physics.Raycast(new Vector3(xPos, 300f, zPos), Vector3.down, out var groundHit))
            {
                GameObject hitObject = groundHit.transform.gameObject;
                Transform hitTransform = groundHit.transform;
                if (hitObject.name.Substring(0, 4) == "Road")
                {
                    
                    NPC = Instantiate(npcPrefab, new Vector3(xPos, hitTransform.position.y + groundBuffer, zPos), Quaternion.identity);
                    NetworkServer.Spawn(NPC);
                    
                    yield return new WaitForSeconds(0.2f);
                    npcCount++;
                }
            }
        }
    }

    
}
