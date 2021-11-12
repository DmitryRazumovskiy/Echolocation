using UnityEngine;

public class PlayerFootstepsManager : MonoBehaviour
{
    public AudioClip[] stepSounds;

    AudioSource audioSource;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Step()
    {
        audioSource.pitch = Random.Range(0.9f, 1f);
        audioSource.PlayOneShot(stepSounds[Random.Range(0, stepSounds.Length)]);        
    }
}
