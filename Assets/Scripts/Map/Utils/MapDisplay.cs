using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {

    [SerializeField] private Renderer textureRenderer;
    private float scaleConstant = 10f;

    public void DrawTexture(Texture2D texture) {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width / scaleConstant, 1, texture.height / scaleConstant);
    }

}
