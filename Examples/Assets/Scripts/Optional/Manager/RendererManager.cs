using UnityEngine;
using UnityEngine.Rendering;


public class RendererManager : MonoBehaviour
{
    public bool applyRendererFeature;

    private void Start()
    {
        ApplyRendererFeature();
    }


    public void ApplyRendererFeature()
    {
        MeshRenderer[] gameObjects = FindObjectsOfType<MeshRenderer>();
        foreach (MeshRenderer gO in gameObjects)    
        {
            gO.shadowCastingMode = applyRendererFeature ? ShadowCastingMode.Off : ShadowCastingMode.On;
        }
    }
}
