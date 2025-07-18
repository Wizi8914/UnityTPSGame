using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Crosshair))]
public class CrosshairBumpEditor : Editor
{
    private float bumpAmount = 20f; // Default bump amount, can be adjusted in the Inspector

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Crosshair crosshair = (Crosshair)target;

        if (GUILayout.Button("Bump Crosshair"))
        {
            crosshair.BumpCrosshair(bumpAmount);
        }
    }
}