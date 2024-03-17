using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour {

    private enum GenerationMode {
        TextureGeneration,
        ObjectsGeneration
    }

    [Header("Render Configs")]
    [SerializeField] private GenerationMode generationMode;

    [Header("Map Configs")]
    [SerializeField] private Transform map;
    [SerializeField] private LayerMask terrainMask;
    private Vector2Int mapSize;
    private Vector3 mapOffset;
    [SerializeField] private int seed;
    [SerializeField] private float noiseScale;
    [SerializeField] [Range(0f, 1f)] private float treeDensity;
    [SerializeField] private Texture2D noiseTexture;
    [SerializeField] private bool autoUpdate;

    [Header("Tree Configs")]
    [SerializeField] private Transform[] treePrefabs;
    [SerializeField] private LayerMask treeMask;

    public void DrawTreeMap() {
        MeshRenderer mr = map.GetComponent<MeshRenderer>();
        mapSize = new Vector2Int((int)mr.bounds.size.x, (int)mr.bounds.size.z);
        mapOffset = new Vector3(mapSize.x / 2f, 0f, mapSize.y / 2f);
        switch (generationMode) {
            case GenerationMode.TextureGeneration:
                GenerateTexture();
                break;
            case GenerationMode.ObjectsGeneration:
                GenerateObjects();
                break;
            default:
                break;
        }
    }

    private void GenerateTexture() {
        noiseTexture = NoiseGenerator.GenerateNoiseMap(mapSize, seed, noiseScale);
        MapDisplay display = GetComponent<MapDisplay>();
        display.DrawTexture(noiseTexture);
    }

    private void GenerateObjects() {
        GenerateTexture();
        GeneateTrees();
    }

    private void GeneateTrees() {
        Transform parent = new GameObject("Trees").transform;
        for (int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {

                float noiseValue = noiseTexture.GetPixel(x, y).g;

                if(noiseValue > 1 - treeDensity) {

                    //We can instantiate the prefab
                    Vector3 spawnLocation = new Vector3(x, 2f, y) - mapOffset;

                    Vector3 displacement = (spawnLocation + Vector3.down) - spawnLocation;
                    if (Physics.Raycast(spawnLocation, displacement.normalized, out RaycastHit hit)) {
                        spawnLocation.y = hit.point.y;
                    } else {
                        //Skip current position
                        continue;
                    }

                    Transform treePrefab = treePrefabs[Random.Range(0, treePrefabs.Length)];
                    Quaternion treeRotation = Quaternion.Euler(Vector3.up * Random.Range(0f, 360f));
                    Vector3 treeSize = treePrefab.GetComponent<NavMeshModifierVolume>().size;

                    if(Utils.CheckSpawnAvailability(spawnLocation, treeRotation, treeSize, treeMask)) {
                        Transform treeTransform = Instantiate(treePrefab, spawnLocation, treeRotation);
                        treeTransform.parent = parent;
                    } else {
                        Debug.Log("Overlapping tree not spawned!");
                    }

                }

            }
        }
    }

    public bool CanAutoUpdate() {
        return autoUpdate;
    }

}
