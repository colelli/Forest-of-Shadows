using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator {

    private const int DEFAULT_RANGE_BOUNDS = 100000;

    public static Texture2D GenerateNoiseMap(Vector2Int mapSize, int seed, float noiseScale) {
        Texture2D noiseMapTexture = new Texture2D(mapSize.x, mapSize.y);
        System.Random prng = new System.Random(seed);

        for(int x = 0; x < noiseMapTexture.width; x++) {
            for(int y = 0; y < noiseMapTexture.height; y++) {
                (float xOffset, float yOffset) = (prng.Next(-DEFAULT_RANGE_BOUNDS, DEFAULT_RANGE_BOUNDS), prng.Next(-DEFAULT_RANGE_BOUNDS, DEFAULT_RANGE_BOUNDS));
                float noiseValue = Mathf.PerlinNoise((float)x / mapSize.x * noiseScale, (float)y / mapSize.y * noiseScale);
                noiseMapTexture.SetPixel(x, y, new Color(0, noiseValue, 0));
            }
        }
        noiseMapTexture.Apply();
        return noiseMapTexture;
    }

}