using UnityEngine;
using Mirror;

public class GameNetworkManager : NetworkManager
{

    [SerializeField] Vector3 spawnPoint = new Vector3(0f, 0f, 0f);
    private static int clientNumber = 0;
    
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // add player at correct spawn position
        GameObject player = Instantiate(playerPrefab, spawnPoint, Quaternion.identity);

        clientNumber++;

        NetworkServer.AddPlayerForConnection(conn, player); //player owned by connection
        //player.GetComponent<PlayerCameraController>().AssignID(clientNumber);

        //Debug.Log(player.GetComponent<PlayerMovement>());
        //player.GetComponent<PlayerMovement>().SetUsername(clientNumber-1);

        //Debug.Log("CLIENT NUMBER "+ clientNumber);
        player.GetComponent<PlayerInformation>().clientID = clientNumber;
        //Debug.Log(player.GetComponent<PlayerInformation>().clientID + "     " + clientNumber);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {

        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);

        //clientNumber--;
        Debug.Log(clientNumber);
    }
}

