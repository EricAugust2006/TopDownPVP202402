using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    private void Start() {
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnClientConnected(ulong clientId) {
        if (NetworkManager.Singleton.IsServer) {
            SpawnPlayer(clientId);
        }
    }

    private void SpawnPlayer(ulong clientId) {

        // vai criar um novo objeto de jogador
        // e atribuir a ele um NetworkObject
        GameObject playerInstance = Instantiate(playerPrefab);
        NetworkObject networkObject = playerInstance.GetComponent<NetworkObject>();
        
        // vai atribuir o NetworkObject ao jogador
        // e atribuir o jogador ao cliente
        // que acabou de conectar
        networkObject.SpawnAsPlayerObject(clientId);
    }
}
