using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource audioSource;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Check if instance already exists
        if (Instance == null)
            Instance = this;
        // If instance already exists and it's not this, destroy it
        else if (Instance != this)
            Destroy(gameObject);    

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}