using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private Transform mainObject;

    List<InteractebleObject> allObjects;
    
    public void CreateObjectHierarchy()
    {
        if (Application.isPlaying)
        {
            DestoroyObjectHierarchy();
            allObjects = new List<InteractebleObject>();
            SetObject(mainObject);
            SetObjects(mainObject);

        }        
    }

    public void DestoroyObjectHierarchy()
    {
        InteractebleObject[] allPreviousObjects = FindObjectsOfType<InteractebleObject>();
        foreach (InteractebleObject interactebleObject in allPreviousObjects)
        {
            if (Application.isPlaying)
            {
                Destroy(interactebleObject);
            }
            else
            {
                DestroyImmediate(interactebleObject);
            }            
        }
    }

    void SetObject(Transform targetTransform)
    {
        if (targetTransform.CompareTag("Audio Source"))
        {
            return;
        }
        InteractebleObject obj = targetTransform.gameObject.AddComponent<InteractebleObject>();
        obj.SetObject(targetTransform, allObjects.Count);        
        obj.AddChildren();        
        allObjects.Add(obj);        
    }

    void SetObjects(Transform transform)
    {
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
        {
            SetObject(transform.GetChild(i));

            int childChildCount = transform.GetChild(i).childCount;
            if (childChildCount != 0)
            {
                SetObjects(transform.GetChild(i));
            }
        }
    }

    public InteractebleObject FindObjectById(int id)
    {
        return allObjects[id];
    }

    public Transform FindTranformById(int id)
    {
        return allObjects[id].transform;
    }

    public int FindLayerById(int id)
    {
        return allObjects[id].GetLayer();
    } 
 

    public List<InteractebleObject> FindAllChildrenOfObject(int id, List<InteractebleObject> listOfChildrens)
    {


        int childCount = FindObjectById(id).GetAllChilds().Count;
        Transform currentTransform = FindTranformById(id);
        for (int i = 0; i < childCount; i++)
        {
            if (currentTransform.GetChild(i).CompareTag("Audio Source"))
            {
                continue;
            }

            listOfChildrens.Add(currentTransform.GetChild(i).GetComponent<InteractebleObject>());

            int childChildCount = currentTransform.GetChild(i).childCount;
            if (childChildCount != 0)
            {
                FindAllChildrenOfObject(currentTransform.GetChild(i).GetComponent<InteractebleObject>().GetId(), listOfChildrens);
            }
        }

        return listOfChildrens;
    }
}


public class InteractebleObject : MonoBehaviour
{
    //потом убрать SerializeField
    [SerializeField] private int id;
    [SerializeField] private int layer;
    [SerializeField] private Transform parent; 
    [SerializeField] private List<InteractebleObject> children;
    [SerializeField] private float volume;
    [SerializeField] private float soundDistance;
    [SerializeField] private float sizeModifier;

    public void SetObject(Transform currentObject, int currentId)
    {
        id = currentId;
        layer = GetParentLayer(currentObject) +1;
        parent = currentObject.parent;
        children = new List<InteractebleObject>();
        sizeModifier = GetSizeModifier(currentObject);
    } 

    float GetSizeModifier(Transform currentObject)
    {
        if (currentObject != null)
        {
            return (Mathf.Abs(currentObject.localScale.x)  + 
                    Mathf.Abs(currentObject.localScale.y)  +
                    Mathf.Abs(currentObject.localScale.z)) / 3;
        }
        return 0;
    }

    int GetParentLayer(Transform currentObject)
    {
        if (currentObject.parent == null)
        {
            return -1;
        }

        return currentObject.parent.GetComponent<InteractebleObject>().layer;
    }    

    public void AddChildren()
    {
        if (parent != null)
        {
            parent.GetComponent<InteractebleObject>().children.Add(this);
        }        
    }

    public void SetSoundDistance(float newSoundDistance)
    {
        soundDistance = newSoundDistance;
    }

    public float GetSoundDistance()
    {
        return soundDistance;
    }

    public void SetVolume(float newVolume)
    {
        volume = newVolume;
    }

    public float GetVolume()
    {
        return volume;
    }

    public float GetSizeModifier()
    {
        return sizeModifier;
    }

    public List<InteractebleObject> GetAllChilds()
    {
        return children;
    }

    public Transform GetParent()
    {
        return parent;
    }

    public int GetId()
    {
        return id;
    }

    public int GetLayer()
    {
        return layer;
    }
}