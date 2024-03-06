using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MagicalCane))]
public class MagicalCaneEditor : Editor {

    public override void OnInspectorGUI() {
        
        MagicalCane magicalCane = (MagicalCane)target;

        if(DrawDefaultInspector()) {
            if (magicalCane.CanAutoUpdate()) {
                magicalCane.SetupLightSourceCollider();
            }
        }

        if(GUILayout.Button("Update LightSource Collider")) {
            magicalCane.SetupLightSourceCollider();
        }

    }

}
