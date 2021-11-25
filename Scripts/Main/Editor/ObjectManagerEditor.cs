using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ObjectManager))]
public class ObjectManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        ObjectManager objectManager = (ObjectManager)target;

        if (DrawDefaultInspector())
        {

        }

        if (GUILayout.Button("Очистить иерархию объектов"))
        {
            objectManager.DestoroyObjectHierarchy();
        }
    }
}
