using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor {

    public override void OnInspectorGUI() {

        MapGenerator mapGen = (MapGenerator)target;

        if (DrawDefaultInspector()) {
            if (mapGen.CanAutoUpdate()) {
                mapGen.DrawTreeMap();
            }
        }

        if (GUILayout.Button("Generate Noise")) {
            mapGen.DrawTreeMap();
        }

    }

}
