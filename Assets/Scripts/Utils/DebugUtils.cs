using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtils : MonoBehaviour {


    private void OnDrawGizmos() {
        Vector3 displacement = (transform.position + Vector3.down) - transform.position;
        Debug.DrawRay(transform.position, displacement.normalized * 10f, Color.red);
    }

}
