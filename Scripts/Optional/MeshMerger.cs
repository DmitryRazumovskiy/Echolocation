using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshMerger : MonoBehaviour
{
    [SerializeField] private Transform mainParent;

    [Space]

    [Header("Выходные параметры")]
    public string outputName;
    public Transform parent;
    public Material material;

    [Header("Входные параметры")]
    public List<GameObject> objectsToMerge = new List<GameObject>();

    [Space]
    [HideInInspector] public bool hideMergedObjects;
    [HideInInspector] public bool deleteMergedObjects;

    public void MergeMeshes()
    {
        GameObject mergedObject = CombineMeshes();

        SetParent(mergedObject);

        if (deleteMergedObjects)
        {
            DeleteMergedObjects();
        }

        if (hideMergedObjects)
        {
            HideMergedObjects();
        }        

    }  
    
    private void SetParent(GameObject mergedObject)
    {        
        if (parent == null)
        {
            mergedObject.transform.SetParent(mainParent);
            return;
        }
        mergedObject.transform.SetParent(parent);

    }

    private void HideMergedObjects()
    {
        foreach (GameObject go in objectsToMerge)
        {
            go.SetActive(false);
        }
    }

    private void DeleteMergedObjects()
    {
        foreach (GameObject go in objectsToMerge)
        {
            DestroyImmediate(go);
        }
        objectsToMerge = new List<GameObject>();
    }

    private GameObject CombineMeshes()
    {
        MeshFilter[] filters = new MeshFilter[objectsToMerge.Count];

        for (int i = 0; i < objectsToMerge.Count; i++)
        {
            filters[i] = objectsToMerge[i].GetComponent<MeshFilter>();
        }

        GameObject mergedObject = new GameObject(outputName);
        mergedObject.AddComponent<MeshFilter>();
        mergedObject.AddComponent<MeshRenderer>();
        mergedObject.AddComponent<MeshCollider>();

        Mesh mergedObjectMesh = new Mesh();

        CombineInstance[] combines = new CombineInstance[objectsToMerge.Count];

        for (int i = 0; i < objectsToMerge.Count; i++)
        {
            combines[i].subMeshIndex = 0;
            combines[i].mesh = filters[i].sharedMesh;
            combines[i].transform = filters[i].transform.localToWorldMatrix;
        }

        mergedObjectMesh.CombineMeshes(combines);
        mergedObjectMesh.RecalculateBounds();
        mergedObjectMesh.RecalculateNormals();
        mergedObjectMesh.Optimize();

        mergedObject.GetComponent<MeshFilter>().sharedMesh = mergedObjectMesh;
        mergedObject.GetComponent<MeshCollider>().sharedMesh = mergedObjectMesh;
        if (material != null)
        {
            mergedObject.GetComponent<MeshRenderer>().sharedMaterial = material;
        }            

        return mergedObject;
    }

    public void FindObjectsByName(string name,bool useSearchName)
    {
        Transform[]  objects = FindObjectsOfType<Transform>();
        List<GameObject> foundObjects = new List<GameObject>();

        foreach (Transform objectTransform in objects)
        {
            if (objectTransform.name == name)
            {
                foundObjects.Add(objectTransform.gameObject);
            } 
        }
        if (useSearchName)
        {
            outputName = name;
        }        
        objectsToMerge = foundObjects;
    }
}
