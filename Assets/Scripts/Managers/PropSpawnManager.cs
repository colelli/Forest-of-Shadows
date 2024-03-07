using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Instantiate(propSO.propPrefab, GetRandomLocation(), Quaternion.Euler(Vector3.zero));
        }
    }

    private Vector3 GetRandomLocation() {
        //TO-DO create random location logic
        return new Vector3(Random.Range(0f, 10f), 0, Random.Range(0f, 10f));
    }

}