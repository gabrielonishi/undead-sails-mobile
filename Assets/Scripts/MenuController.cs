using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    private int SKELETON_TUTORIAL_SCENE_BUILD_INDEX = 4;
    private int STORE_SCENE_BUILD_INDEX = 6;

    [SerializeField]
    private AudioSource audioSource;

    public void NextScene()
    {
        PlaySoundAndLoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToGame()
    {
        PlaySoundAndLoadScene(SKELETON_TUTORIAL_SCENE_BUILD_INDEX);
    }

    public void GoToStore()
    {
        PlaySoundAndLoadScene(STORE_SCENE_BUILD_INDEX);
    }

    public void GoToMenu()
    {
        PlaySoundAndLoadScene(0);
    }

    private void PlaySoundAndLoadScene(int sceneIndex)
    {
        StartCoroutine(PlaySoundAndLoadSceneCoroutine(sceneIndex));
    }

    private IEnumerator PlaySoundAndLoadSceneCoroutine(int sceneIndex)
    {
        audioSource.Play();

        // Wait for the audio clip to finish playing
        yield return new WaitForSeconds(audioSource.clip.length);

        // Now, load the specified scene
        SceneManager.LoadScene(sceneIndex);
    }
}
