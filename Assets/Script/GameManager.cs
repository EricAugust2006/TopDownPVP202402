using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : MonoBehaviour
{
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
