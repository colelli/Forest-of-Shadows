using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpawnManager : MonoBehaviour {

    [SerializeField] private Transform structurePrefab;
    private Vector2 mapSize;
    private Vector3 mapOffset;
    private static Player inGamePlayer;
    private static Transform inGameStructure;

    private void Start() {
        // Get references
        MeshRenderer mr = MapGenerator.Instance.GetTerrain().GetComponent<MeshRenderer>();
        mapSize = new Vector2(mr.bounds.size.x, mr.bounds.size.z);
        mapOffset = new Vector3(mapSize.x / 2f, 0f, mapSize.y / 2f);

        // Place the spawn structure randomly
        PlaceSpawnStructure();

        // Call the MapGen to generate the world
        MapGenerator.Instance.DrawTreeMap();

        // Spawn the player
        SpawnPlayer();
    }

    public static Player GetInGamePlayer() {
        return inGamePlayer;
    }

    private void SpawnPlayer() {
        SpawnPoint spawnPoint = inGameStructure.GetComponent<SpawnPoint>();
        Vector3 spawnPosition = spawnPoint.GetSpawnPointPosition();
        Quaternion spawnRotation = spawnPoint.GetSpawnPointRotation();
        inGamePlayer = Instantiate(GameManager.Instance.GetPlayer(), spawnPosition, spawnRotation);
    }

    private void PlaceSpawnStructure() {
        InstatiateStructure(Utils.GetRandomLocation(mapSize, mapOffset), structurePrefab);
    }

    private void InstatiateStructure(Vector3 spawnLocation, Transform prefab) {
        Quaternion prefabRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        inGameStructure = Instantiate(prefab, spawnLocation, prefabRotation);
    }
    
}
