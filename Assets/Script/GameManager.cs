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
    public GameObject playerPrefab;
    public GameObject playerPrefab2;  // Prefab para o segundo personagem

    private void Start()
    {
        // Se for o Host, instanciar ambos os jogadores
        if (NetworkManager.Singleton.IsHost)
        {
            SpawnPlayers();
        }
    }

    private void SpawnPlayers()
    {
        // Instancia o primeiro jogador
        GameObject player1 = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        player1.GetComponent<NetworkObject>().Spawn();

        // Instancia o segundo jogador em outra posição
        GameObject player2 = Instantiate(playerPrefab2, new Vector3(2, 0, 0), Quaternion.identity);
        player2.GetComponent<NetworkObject>().Spawn();
    }
}
