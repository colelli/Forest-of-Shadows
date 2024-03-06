using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebug : MonoBehaviour {

    [SerializeField] private Material playerDebugMat;
    [SerializeField] private Material lightDebugMat;
    private GameObject parent;
    private GameObject lightHitbox;
    private GameObject playerHitbox;

    private CharacterController characterController;
    private SphereCollider lightSourceCollider;

    private void Awake() {
        parent = new GameObject("Debug");
        parent.transform.parent = transform;
        lightSourceCollider = GetComponent<SphereCollider>();
        characterController = GetComponent<CharacterController>();
        CreateSphereHitbox();
        CreatePlayerHitbox();
    }

    private void CreateSphereHitbox() {
        lightHitbox = new GameObject("SphereHitbox");
        lightHitbox.transform.parent = parent.transform;
        lightHitbox.AddComponent<MeshRenderer>().material = lightDebugMat;
        lightHitbox.AddComponent<MeshFilter>().mesh = RetreiveSphereMesh();
        lightHitbox.transform.localScale = Vector3.one * lightSourceCollider.radius * 2;
        lightHitbox.transform.position = lightSourceCollider.center;
    }

    private void CreatePlayerHitbox() {
        playerHitbox = new GameObject("PlayerHitbox");
        playerHitbox.transform.parent = parent.transform;
        playerHitbox.AddComponent<MeshRenderer>().material = playerDebugMat;
        playerHitbox.AddComponent<MeshFilter>().mesh = RetrieveCapsuleMesh();
        float xz = characterController.radius * 2;
        playerHitbox.transform.localScale = new Vector3(xz, characterController.height / 2f, xz);
        playerHitbox.transform.position = characterController.center;
    }

    private Mesh RetreiveSphereMesh() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        Destroy(obj);
        return mesh;
    }

    private Mesh RetrieveCapsuleMesh() {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        Mesh mesh = obj.GetComponent <MeshFilter>().mesh;
        Destroy(obj);
        return mesh;
    }

}
