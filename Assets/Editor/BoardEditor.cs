using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Board))]
public class BoardEditor : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Board myScript = (Board)target;

        if (GUILayout.Button("Create Rows"))
        {
            myScript.CreateRows();
        }
        if (GUILayout.Button("Find Rows"))
        {
            myScript.FindRows();
        }
        if (GUILayout.Button("Delete Rows"))
        {
            myScript.DeleteRows();
        }
    }


}
