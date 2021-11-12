using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionController : MonoBehaviour
{
    [SerializeField] private GameObject blindScreen;
    [SerializeField] private GameObject userInterface;
    [SerializeField] private GameManager gameManager;

    public void ChangeLayer(int increment)
    {
        gameManager.CurrentLayer += increment;
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
}
