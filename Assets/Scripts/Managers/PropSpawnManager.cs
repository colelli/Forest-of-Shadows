using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class PropSpawnManager : MonoBehaviour {

    public static PropSpawnManager Instance { get; private set; }

    private void Awake() {
        //We check if there is already a Singleton of PropSpawnManager
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start() {
        SpawnPropInRandomLocation();
    }

    private void SpawnPropInRandomLocation() {
        foreach(PropSO propSO in DeliveryManager.Instance.GetDeliverablesList()) {
            Instantiate(propSO.propPrefab, GetRandomLocation(), Quaternion.Euler(Vector3.up * Random.Range(0f, 360f)));
        }
    }

    private Vector3 GetRandomLocation() {
        Vector3 randomLocation = new Vector3(Random.Range(0f, 10f), 0, Random.Range(0f, 10f));
        if (NavMesh.SamplePosition(randomLocation, out NavMeshHit hit, 2f, NavMesh.AllAreas)){
            randomLocation.y = hit.position.y;
        } else {
            randomLocation = Vector3.zero;
        }

        return randomLocation;
    }

}