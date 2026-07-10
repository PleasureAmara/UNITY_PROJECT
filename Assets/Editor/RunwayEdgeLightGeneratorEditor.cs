using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(RunwayEdgeGenerator))]
public class RunwayEdgeLightGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        RunwayEdgeGenerator generator = (RunwayEdgeGenerator)target;

        GUILayout.Space(10);

        if(GUILayout.Button("Generate Lights")) 
        {
            generator.Generate();
        }

        if (GUILayout.Button("Clear Lights"))
        {
            generator.Clear();
        }

        
    }
}
