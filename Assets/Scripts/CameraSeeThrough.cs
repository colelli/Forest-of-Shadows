using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraSeeThrough : MonoBehaviour {

    [Header("Camera Configs")]
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask cameraMask;
    [SerializeField] [Range(0f, 1f)] private float seeThroughOpacity;

    private List<Transform> transformHiddenLastUpdate = new List<Transform>();


    private void Update() {
        
        foreach(Transform t in transformHiddenLastUpdate) {
            t.gameObject.SetActive(true);
        }
        transformHiddenLastUpdate.Clear();

        Vector3 direction = (target.position - transform.position).normalized;
        float distance = (target.position - transform.position).magnitude;
        if (Physics.BoxCast(transform.position, Vector3.one, direction, out RaycastHit hit, Quaternion.identity, distance, cameraMask)) {
            Transform hitTransform = hit.transform;
            transformHiddenLastUpdate.Add(hitTransform);
            hitTransform.gameObject.SetActive(false);
        }

    }

    private bool GetCurrentLODRenderer(LODGroup lodGroup, out MeshRenderer renderer) {
        renderer = null;
        Transform lodTransform = lodGroup.transform;
        foreach (Transform child in lodTransform) {
            renderer = child.GetComponent<MeshRenderer>();
            if (renderer != null && renderer.isVisible) {
                return true;
            }
        }
        return false;
    }

}

public struct HiddenTransformInfo {
    public readonly Transform transform;
    public readonly MeshRenderer mr;

    public HiddenTransformInfo(Transform transform, MeshRenderer mr) {
        this.transform = transform;
        this.mr = mr;
    }
}
