using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tree : MonoBehaviour {

    [SerializeField] private MeshRenderer treeRenderer;
    [SerializeField] private MeshRenderer placeholderRenderer;

    private void Awake() {
        treeRenderer.enabled = true;
        placeholderRenderer.enabled = false;
    }

    private void Start() {
        treeRenderer.enabled = false;
    }

    public void TogglePlaceHolderVisibility() {
        treeRenderer.enabled = !treeRenderer.enabled;
        placeholderRenderer.enabled = !placeholderRenderer.enabled;
        if (placeholderRenderer.enabled)
            StartCoroutine(Fade());
    }

    private IEnumerator Fade() {
        Material m = placeholderRenderer.material;
        for (float alpha = 1f; alpha >= 0.25; alpha -= 0.05f) {
            m.SetFloat("_Opacity", alpha);
            yield return null;
        }
        StopCoroutine(Fade());
    }

    public void Show() {
        treeRenderer.enabled = true;
    }

    public void Hide() {
        treeRenderer.enabled = false;
    }

}