using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NetworkGUI : MonoBehaviour
{

    GameManager gameManager;

    private void OnGUI()
    {
        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            StartButtons();
        }
        else 
        { 
            StatusLabel();
            if (GUILayout.Button("Disconnect")) 
            { 
                NetworkManager.Singleton.Shutdown();
            }
            if (NetworkManager.Singleton.IsServer && GUILayout.Button("Re-Launch")) 
            {
                GameManager GM = FindObjectOfType<GameManager>();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void StartButtons() 
    {
        GUILayout.BeginArea(new Rect(10, 10, 150, 150));
        if (GUILayout.Button("Host")) 
        {
            NetworkManager.Singleton.StartHost();
        }
        if (GUILayout.Button("Client")) 
        {
            NetworkManager.Singleton.StartClient();
        }
        if (GUILayout.Button("Server")) 
        { 
            NetworkManager.Singleton.StartServer();
        }
        GUILayout.EndArea();
    }

    private void StatusLabel() 
    {
        GUILayout.BeginArea(new Rect(10,10,150,300));

        //if ternário
        string mode = NetworkManager.Singleton.IsHost ? "Host" :
                        NetworkManager.Singleton.IsServer ? "Server" : "Client";

        GUILayout.Label("Transport: "
            +NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
      
        /*if (NetworkManager.Singleton.IsHost)
        {
            mode = "Host";
        }
        else 
        {
            if (NetworkManager.Singleton.IsServer) 
            {
                mode = "Server";
            }
            else
            {
                mode = "Client";
            }
        }*/

        GUILayout.EndArea();


    }

}
