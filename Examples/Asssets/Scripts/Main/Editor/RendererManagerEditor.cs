using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RendererManager))]
public class RendererManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        RendererManager rendererManager = (RendererManager)target;

        if (DrawDefaultInspector())
        {
            rendererManager.ApplyRendererFeature();
        }       

    }
}
