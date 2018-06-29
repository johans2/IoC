using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(MonoBehaviour), true)]
public class DependencyViewerInspector : Editor {

    
    private Type targetType;

    void OnEnable() {
        targetType = target.GetType();    
    }


    public override void OnInspectorGUI() {
        
        if(GUILayout.Button("View dependency graph")) {
            OpenWindow();
        }

        DrawDefaultInspector();
    }

    private void OpenWindow() {
        NodeBasedEditor.SetType(targetType);
        NodeBasedEditor window = EditorWindow.GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Dependency graph");
    }

}