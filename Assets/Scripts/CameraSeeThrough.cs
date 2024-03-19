using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraSeeThrough : MonoBehaviour {

    [Header("Camera Configs")]
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask cameraMask;

    
    private List<Tree> invisibleTreesLastUpdate = new List<Tree>();


    private void Update() {
        
        Debug.DrawLine(transform.position, target.position, Color.yellow);

        foreach(Tree t in invisibleTreesLastUpdate) {
            t.TogglePlaceHolderVisibility();
        }
        invisibleTreesLastUpdate.Clear();

        Vector3 direction = (target.position - transform.position).normalized;
        float distance = (target.position - transform.position).magnitude;
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, Vector3.one, direction, Quaternion.identity, distance - 1.5f, cameraMask);

        if (hits.Length > 0) {
            for(int i = 0; i < hits.Length; i++) {
                //Get tree hit
                Transform hitTransform = hits[i].transform;
                Tree tree = hitTransform.GetComponentInParent<Tree>();
                invisibleTreesLastUpdate.Add(tree);
                tree.TogglePlaceHolderVisibility();
            }
        }

    }

}
