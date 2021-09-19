using System;
using UnityEngine;

namespace Mirror
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(NetworkManager))]
    public class MainMenuScript : MonoBehaviour
    {
        NetworkManager manager;
        public GameObject cam;
        public GameObject menuCanvas;

        void Awake()
        {
            manager = GetComponent<NetworkManager>();
            Debug.Log(manager);
        }
    
        void StartButtons()
        {
            if (!NetworkClient.active)
            {
                // Server + Client
                if (Application.platform != RuntimePlatform.WebGLPlayer)
                {
                    if (GUILayout.Button("Host (Server + Client)"))
                    {
                        manager.StartHost();
                    }
                }
    
                // Client + IP
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Client"))
                {
                    manager.StartClient();
                }
    
                manager.networkAddress = GUILayout.TextField(manager.networkAddress);
                GUILayout.EndHorizontal();
    
                // Server Only
                if (Application.platform == RuntimePlatform.WebGLPlayer)
                {
                    // cant be a server in webgl build
                    GUILayout.Box("(  WebGL cannot be server  )");
                }
                else
                {
                    if (GUILayout.Button("Server Only")) manager.StartServer();
                }
            }
            else
            {
                // Connecting
                GUILayout.Label("Connecting to " + manager.networkAddress + "..");
                if (GUILayout.Button("Cancel Connection Attempt"))
                {
                    manager.StopClient();
                }
            }
        }
    
        public void SinglePlayer()
        {
            if (!NetworkClient.active && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                // Server + Client
                cam.SetActive(false);
                menuCanvas.SetActive(false);
                manager.StartHost();
            }
        }

        public void Multiplayer()
        {
            cam.SetActive(false);
            menuCanvas.SetActive(false);
            manager.StartClient();
            manager.networkAddress = "34.130.236.58";
        }
    
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
