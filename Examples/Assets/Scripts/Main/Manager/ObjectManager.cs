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
        if (!targetTransform.gameObject.activeInHierarchy)
        {
            return;
        }
        if (targetTransform.GetComponent<InteractebleObject>() == null)
        {
            if (targetTransform.GetComponent<AudioSource>() == null)
            {
                targetTransform.gameObject.AddComponent<InteractebleObject>();
            }            
        }       
    }

    public List<InteractebleObject> GetListOfAllObjects()
    {
        return allObjects;
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
 
    public void AddToListOfObjects(InteractebleObject newInteractebleObject)
    {
        allObjects.Add(newInteractebleObject);
    }

    public List<InteractebleObject> FindAllChildrenOfObject(int id, List<InteractebleObject> listOfChildrens)
    {

        List<InteractebleObject> childs = FindObjectById(id).GetAllChilds();

        foreach (InteractebleObject child in childs)
        {
            listOfChildrens.Add(child.GetComponent<InteractebleObject>());
            int childChildCount = child.transform.childCount;
            if (childChildCount != 0)
            {
                FindAllChildrenOfObject(child.GetComponent<InteractebleObject>().GetId(), listOfChildrens);
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
    [SerializeField] private float sizeModifier;

    [SerializeField] private float volume; //пока никак не юзается
    [SerializeField] private float soundDistance;
    [SerializeField] private float pitch;


    [SerializeField] private bool hasAudioSource;
    [SerializeField] public bool HasAudioSource
    {
        get
        {
            return hasAudioSource;
        }
        set
        {
            if (value && !hasAudioSource)
            {
                GetComponent<ObjectAudioExperemental>().Initialization();
                hasAudioSource = value;
            }
        }
    }
    [SerializeField] private AudioSource audioSource;
    

    private void OnEnable()
    {
        ObjectManager objectManager = FindObjectOfType<GameManager>().ObjectManager_;
        SetObject(objectManager.GetListOfAllObjects().Count);
        objectManager.AddToListOfObjects(GetComponent<InteractebleObject>());
    }


    public void SetObject(int currentId)
    {       
        id = currentId;
        layer = GetParentLayer(transform) +1;
        parent = transform.parent;
        children = new List<InteractebleObject>();
        sizeModifier = GetSizeModifier(transform);

        if (parent != null)
        {
            parent.GetComponent<InteractebleObject>().children.Add(this);
        }

        //isTargeted = rayCastController.GetLastSelectedTargetId() == transform.GetComponent<InteractebleObject>().GetId() ? true : false;
        HasAudioSource = GetComponent<ObjectAudioExperemental>() != null ? true : false;
    }

    float GetSizeModifier(Transform currentObject)
    {
        if (currentObject != null)
        {            
            if (currentObject.GetComponent<Collider>() != null)
            {
                Bounds bounds = currentObject.GetComponent<Collider>().bounds;
                return (Mathf.Abs(bounds.size.x) +
                        Mathf.Abs(bounds.size.y) +
                        Mathf.Abs(bounds.size.z)) / 3;
            }
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

    public void SetAudioSource(AudioSource newAudioSource)
    {
        audioSource = newAudioSource;
    }

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void SetPitch(float value)
    {
        pitch = value;
    }

    public float GetPitch()
    {
        return pitch;
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

    public static int SortBySize(InteractebleObject firstInteracteble, InteractebleObject secondInteracteble)
    {
        return firstInteracteble.sizeModifier.CompareTo(secondInteracteble.sizeModifier);
    }
}