using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils {

    /// <summary>
    /// Check at the given position and rotation, if we can spawn a new entity with a footprint of entityBoxSize.<br/>
    /// The method checks for any collision with the defined layer mask.
    /// </summary>
    /// <param name="position">The position to check</param>
    /// <param name="rotation">The rotation to check</param>
    /// <param name="entitySize">The entity size</param>
    /// <param name="layerMask">The layermask to check (avoids colliding with the ground or other unwanted layers)</param>
    /// <returns></returns>
    public static bool CheckSpawnAvailability(Vector3 position, Quaternion rotation, Vector3 entitySize, LayerMask layerMask) {

        Collider[] overlappingColliders = new Collider[1];
        int numberOfOverlappingColliders = Physics.OverlapBoxNonAlloc(position, entitySize, overlappingColliders, rotation, layerMask);

        if(numberOfOverlappingColliders == 0) {
            return true;
        }

        return false;

    }

    public static bool CheckSpawnAvailability(Vector3 position, Quaternion rotation, Vector3 entitySize, LayerMask[] layerMasks) {

        for(int i = 0; i < layerMasks.Length; i++) {
            if (!CheckSpawnAvailability(position, rotation, entitySize, layerMasks[i])){
                //We found an unwanted overlap
                return false;
            }
        }

        return true;

    }

    public static Vector3 GetRandomLocation(Vector2 mapSize, Vector3 mapOffset) {
        Vector3 randomLocation = new Vector3(Random.Range(0f, mapSize.x), 0f, Random.Range(0f, mapSize.y)) - mapOffset;

        // Direction where to cast the ray
        Vector3 displacement = (randomLocation + Vector3.down) - randomLocation;

        if (Physics.Raycast(randomLocation, displacement.normalized, out RaycastHit hit)) {
            randomLocation.y = hit.point.y;
        }

        return randomLocation;
    }

}