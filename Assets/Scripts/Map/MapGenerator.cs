using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour {

    public static MapGenerator Instance { get; private set; }

    private enum GenerationMode {
        TextureGeneration,
        ObjectsGeneration
    }

    [Header("Render Configs")]
    [SerializeField] private GenerationMode generationMode;

    [Header("Map Configs")]
    [SerializeField] private Transform map;
    [SerializeField] private Transform mapParent;
    [SerializeField] private Transform[] mapPrefabs;
    [SerializeField] private LayerMask terrainMask;
    private Vector2Int mapSize;
    private Vector3 mapOffset;
    private int seed;
    [SerializeField] private bool autoUpdate;

    [Header("Tree Configs")]
    [SerializeField] private Texture2D treeNoiseTexture;
    [SerializeField] private Transform[] treePrefabs;
    [SerializeField][Range(0f, 1f)] private float treeDensity;
    [SerializeField] private float treeNoiseScale;
    [SerializeField] private LayerMask treeMask;

    [Header("Decoration Configs")]
    [SerializeField] private Texture2D decorNoiseTexture;
    [SerializeField] private Transform[] decorationPrefabs;
    [SerializeField][Range(0f, 1f)] private float decorationDensity;
    [SerializeField] private float decorNoiseScale;
    [SerializeField] private LayerMask decorationMask;

    private void Awake() {
        //We check if there is already a Singleton of MapGenerator
        if (Instance != null && Instance != this) {
            Destroy(this);
            throw new System.Exception($"[{this.name}] >>> An Instance of this Singleton already exists!");
        } else {
            //There are not instances
            Instance = this;
        }
    }

    public void DrawTreeMap() {
        MeshRenderer mr = map.GetComponent<MeshRenderer>();
        int offset = 20 * (GameManager.Instance.GetCurrentDifficultyData().GetDifficultyLevel() + 1);
        mapSize = new Vector2Int((int)mr.bounds.size.x - offset, (int)mr.bounds.size.z - offset);
        Debug.Log(mapSize);
        mapOffset = new Vector3(mapSize.x / 2f, 0f, mapSize.y / 2f);
        switch (generationMode) {
            case GenerationMode.TextureGeneration:
                GenerateTextures();
                break;
            case GenerationMode.ObjectsGeneration:
                GenerateObjects();
                break;
            default:
                break;
        }
    }

    private void GenerateTextures() {
        seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        //Generate Tree Noise Texture
        treeNoiseTexture = NoiseGenerator.GenerateNoiseMap(mapSize, seed, treeNoiseScale);

        //Generate Decor Noise Texture
        decorNoiseTexture = NoiseGenerator.GenerateNoiseMap(mapSize, seed, decorNoiseScale);

        //MapDisplay display = GetComponent<MapDisplay>();
        //display.DrawTexture(treeNoiseTexture);
    }

    private void GenerateObjects() {
        GenerateTextures();
        GenerateDecorations();
        GeneateTrees();
    }

    private void GeneateTrees() {
        Transform parent = new GameObject("Trees").transform;
        for (int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {

                float noiseValue = treeNoiseTexture.GetPixel(x, y).g;

                if(noiseValue > 1 - treeDensity) {
                    //We can instantiate the prefab
                    InstantiatePrefab(new Vector2(x, y), treePrefabs, treeMask, parent);
                }

            }
        }
    }

    private void GenerateDecorations() {
        Transform parent = new GameObject("Decorations").transform;
        for(int x = 0; x < mapSize.x; x++) {
            for(int y = 0; y < mapSize.y; y++) {

                float noiseValue = decorNoiseTexture.GetPixel(x, y).g;

                if(noiseValue > 1 - decorationDensity) {
                    //We can instantite the prefab
                    InstantiatePrefab(new Vector2(x, y), decorationPrefabs, decorationMask, parent);
                }

            }
        }
        foreach (Transform t in parent.GetComponentInChildren<Transform>()) {
            BoxCollider bc = t.GetComponent<BoxCollider>();
            DestroyImmediate(bc);
        }
    }

    private void InstantiatePrefab(Vector2 coords, Transform[] prefabs, LayerMask mask, Transform parent) {
        Vector3 spawnLocation = new Vector3(coords.x, 2f, coords.y) - mapOffset;

        Vector3 displacement = (spawnLocation + Vector3.down) - spawnLocation;
        if (Physics.Raycast(spawnLocation, displacement.normalized, out RaycastHit hit)) {
            spawnLocation.y = hit.point.y;
        } else {
            //Skip current position
            return;
        }

        Transform prefab = prefabs[UnityEngine.Random.Range(0, prefabs.Length)];
        Quaternion prefabRotation = Quaternion.Euler(Vector3.up * UnityEngine.Random.Range(0f, 360f));
        Vector3 prefabSize = prefab.GetComponent<NavMeshModifierVolume>().size;

        if (Utils.CheckSpawnAvailability(spawnLocation, prefabRotation, prefabSize, mask)) {
            Transform prefabTransform = Instantiate(prefab, spawnLocation, prefabRotation);
            prefabTransform.parent = parent;
        } else {
            Debug.Log("Overlapping prefab not spawned!");
        }
    }

    public bool CanAutoUpdate() {
        return autoUpdate;
    }

    public Transform GetTerrain() {
        if(map == null)
            map = Instantiate(mapPrefabs[GameManager.Instance.GetCurrentDifficultyData().GetDifficultyLevel()], Vector3.zero, Quaternion.identity, mapParent);
        return map;
    }

}
