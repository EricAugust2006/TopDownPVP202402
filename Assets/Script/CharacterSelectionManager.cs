using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

using UnityEngine;
using Unity.Netcode;

public class CharacterSelectionManager : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    private int selectedCharacterIndex = 0;


    private void Start()
    {
    }

    public void SelectCharacter(int characterIndex)
    {
        selectedCharacterIndex = characterIndex;
    }

    public void StartGame()
    {

        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        GameObject characterPrefab = characterPrefabs[selectedCharacterIndex];

        Vector3 spawnPosition = GetSpawnLocation();  
        GameObject characterInstance = Instantiate(characterPrefab, spawnPosition, Quaternion.identity);

        if (NetworkManager.Singleton.IsServer || NetworkManager.Singleton.IsHost)
        {
            characterInstance.GetComponent<NetworkObject>().Spawn(); 
        }
    }

    private Vector3 GetSpawnLocation()
    {
        return new Vector3(0, 0, 0);
    }
}
