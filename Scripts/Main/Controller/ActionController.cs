using UnityEngine.UI;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] private GameObject blindScreen;
    [SerializeField] private GameObject userInterface;
    [SerializeField] private GameObject minimap;
    [SerializeField] private GameManager gameManager;

    private AudioData audioData;

    private void Awake()
    {
        audioData = gameManager.AudioData_;
    }

    public void ChangeObjectDistance(int increment)
    {
        audioData.currentObjectDistance += increment * audioData.objectDistanceStep;
        audioData.currentObjectDistance = Mathf.Clamp(audioData.currentObjectDistance,audioData.minObjectDistance,audioData.maxObjectDistance);
    }

    public void BlindScreenToggle()
    {
        if (blindScreen.activeInHierarchy)
        {
            userInterface.SetActive(true);
            blindScreen.SetActive(false);
        }
        else
        {
            userInterface.SetActive(false);
            blindScreen.SetActive(true);
        }
    }

    public void MinimapToggle()
    {
        if (minimap.activeInHierarchy)
        {
            minimap.SetActive(false);
        }
        else
        {
            minimap.SetActive(true);
        }
    }
}
