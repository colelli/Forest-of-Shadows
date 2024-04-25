using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PropSpawnManager : MonoBehaviour {

    public static PropSpawnManager Instance { get; private set; }
    [SerializeField] private LayerMask mask;
    private Vector2 mapSize;
    private Vector3 mapOffset;

    private void Awake() {
        //We check if there is already a Singleton of PropSpawnManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        MeshRenderer mr = MapGenerator.Instance.GetTerrain().GetComponent<MeshRenderer>();
        mapSize = new Vector2(mr.bounds.size.x, mr.bounds.size.z);
        mapOffset = new Vector3(mapSize.x / 2f, 0f, mapSize.y / 2f);
        SpawnPropInRandomLocation();
    }

    private void SpawnPropInRandomLocation() {
        Transform parent = new GameObject("PropParent").transform;
        foreach(KeyValuePair<PropSO, int> entry in DeliveryManager.Instance.GetDeliverablesList()) {
            for(int i=0; i<entry.Value; i++) {
                bool propSpawned = false;
                do {
                    propSpawned = InstantiateProp(Utils.GetRandomLocation(mapSize, mapOffset), entry.Key.propPrefab, mask, parent);
                } while (!propSpawned);
            }
        }
    }

    private bool InstantiateProp(Vector3 spawnLocation, Transform prefab, LayerMask mask, Transform parent) {
        Quaternion prefabRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
        Vector3 prefabSize = Vector3.one;

        if(Utils.CheckSpawnAvailability(spawnLocation, prefabRotation, prefabSize, mask)) {
            Transform prefabTransform = Instantiate(prefab, spawnLocation, prefabRotation);
            prefabTransform.parent = parent;
            return true;
        } else {
            Debug.Log("Overlapping prefab not spawned!");
            return false;
        }

    }

}