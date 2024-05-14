using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingTest : MonoBehaviour {

    private void Update() {
        transform.position = GameManager.Instance.GetPlayer().transform.position;
    }

    private void OnTriggerEnter(Collider other) {
        Tree tree = other.GetComponentInParent<Tree>();
        if(tree != null) {
            tree.Show();
        }
    }

    private void OnTriggerExit(Collider other) {
        Tree tree = other.GetComponentInParent<Tree>();
        if (tree != null) {
            tree.Hide();
        }
    }

}
