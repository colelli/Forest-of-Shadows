using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

    // DOES NOTHING BUT USEFUL TO GET SPAWNPOINT REF AVOIDING SCANNING EVERY GO CHILDREN
    [SerializeField] private Transform spawnPoint;

    public Vector3 GetSpawnPointPosition() {
        return spawnPoint.position;
    }

    public Quaternion GetSpawnPointRotation() {
        return spawnPoint.rotation;
    }

}
