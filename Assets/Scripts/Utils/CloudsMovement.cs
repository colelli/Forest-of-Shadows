using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CloudsMovement : MonoBehaviour {

    [SerializeField] private float rotationSpeed;

    private void Update() {
        transform.Rotate(new Vector3(0f, 1f * rotationSpeed * Time.deltaTime, 0f));
    }

}