using UnityEngine;
using UnityEditor;
/*
 * Edit by Bob Jeltes https://github.com/emsylf on November 23, 2020: 
 * TorusCreator.cs is the class that allows the user to create a torus shape like any other basic Unity3D shape, through the GameObject/3D Object menu at the top of the window
 * Torus.cs is now its own class governing everything that has to do with the instance of a torus. 
 * TorusEditor.cs creates a custom inspector that allows you to see the effect of the changing variables in real-time.
 * This script was converted to an editor-script by Bob Jeltes https://github.com/emsylf
 */
[CustomEditor(typeof(Torus))]
public class TorusEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Torus targetScript = (Torus)target;

        EditorGUI.BeginChangeCheck();
        targetScript.radius = Mathf.Max(.001f, EditorGUILayout.FloatField("Radius", targetScript.radius));
        targetScript.thickness = Mathf.Max(.001f, EditorGUILayout.FloatField("Thickness", targetScript.thickness));
        targetScript.segments = Mathf.Max(3, EditorGUILayout.IntField("Segments", targetScript.segments));
        targetScript.segmentDetail = Mathf.Max(3, EditorGUILayout.IntField("Segment Detail", targetScript.segmentDetail));
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(target);
            targetScript.UpdateTorus();
        }

        if (GUILayout.Button("New mesh"))
        {
            targetScript.NewMesh();
        }
    }
}
