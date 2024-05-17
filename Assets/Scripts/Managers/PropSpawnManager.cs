using System;
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

    // Power-ups
    [SerializeField] private PropListSO powerUpList;
    private Dictionary<PropSO, int> powerUps = new Dictionary<PropSO, int>();
    private int powerUpCount;

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
        DayManager.OnNightStarted += DayManager_OnNightStarted;
        SetupPowerUps();

        MeshRenderer mr = MapGenerator.Instance.GetTerrain().GetComponent<MeshRenderer>();
        int offset = GameManager.Instance.GetCurrentDifficultyData().GetDifficultyLevel() == 3 ? 80 : 40;
        mapSize = new Vector2(mr.bounds.size.x - offset, mr.bounds.size.z - offset);
        mapOffset = new Vector3(mapSize.x / 2f, 0f, mapSize.y / 2f);
        // Spawn items to deliver
        SpawnPropInRandomLocation(DeliveryManager.Instance.GetDeliverablesList(), new GameObject("PropParent").transform);
    }

    private void DayManager_OnNightStarted(object sender, System.EventArgs e) {
        // Spawn power-ups
        SpawnPropInRandomLocation(powerUps, new GameObject("PowerUpsParent").transform);
    }

    private void SetupPowerUps() {
        CalculatePowerUpCount();
        PopulatePowerUpDictionary();
    }

    private void CalculatePowerUpCount() {
        int powerUpBaseCount = 6;
        GameDifficultyData difficulty = GameManager.Instance.GetCurrentDifficultyData();
        powerUpCount = (int)Mathf.Floor((powerUpBaseCount / (difficulty.GetDifficultyLevel() + 1)) - 1);
    }

    private void PopulatePowerUpDictionary() {
        foreach(PropSO powerUp in powerUpList.propSOList) {
            powerUps.Add(powerUp, powerUpCount);
        }
    }

    private void SpawnPropInRandomLocation(Dictionary<PropSO, int> entries, Transform parent) {
        foreach(KeyValuePair<PropSO, int> entry in entries) {
            for(int i=0; i<entry.Value; i++) {
                bool propSpawned = false;
                do {
                    propSpawned = InstantiateProp(Utils.GetRandomLocation(mapSize, mapOffset), entry.Key.propPrefab, mask, parent);
                } while (!propSpawned);
            }
        }
    }

    private bool InstantiateProp(Vector3 spawnLocation, Transform prefab, LayerMask mask, Transform parent) {
        Quaternion prefabRotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
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

    private void OnDestroy() {
        DayManager.OnNightStarted -= DayManager_OnNightStarted;
    }

}