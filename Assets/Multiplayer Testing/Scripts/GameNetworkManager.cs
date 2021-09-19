using UnityEngine;
using Mirror;

public class GameNetworkManager : NetworkManager
{

    [SerializeField] Vector3 spawnPoint = new Vector3(0f, 0f, 0f);
    private int clientNumber = 0;
    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // add player at correct spawn position
        GameObject player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);

        clientNumber++;

        NetworkServer.AddPlayerForConnection(conn, player); //player owned by connection
        player.GetComponent<PlayerCameraController>().AssignID(clientNumber);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {

        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);

        clientNumber--;
        Debug.Log(clientNumber);
    }
}

