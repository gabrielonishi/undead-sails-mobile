using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    private int GAME_SCENE_BUILD_INDEX = 6;
    private int STORE_SCENE_BUILD_INDEX = 8;
    private int WATCH_AD_SCENE_BUILD_INDEX = 15;
    
    [SerializeField]
    private AudioSource audioSource;

    public void GoToStoryScene()
    {
        Debug.Log("Entra em GoToStoryScene");
        PlaySoundAndLoadScene(1);
    }

    public void NextScene()
    {
        PlaySoundAndLoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void GoToGame()
    {
        PlaySoundAndLoadScene(GAME_SCENE_BUILD_INDEX);
    }

    public void GoToStore()
    {
        PlaySoundAndLoadScene(STORE_SCENE_BUILD_INDEX);
    }

    public void GoToMenu()
    {
        PlaySoundAndLoadScene(0);
    }

    public void GoToWatchAd()
    {
        PlaySoundAndLoadScene(WATCH_AD_SCENE_BUILD_INDEX);
    }

    private void PlaySoundAndLoadScene(int sceneIndex)
    {
        StartCoroutine(PlaySoundAndLoadSceneCoroutine(sceneIndex));
    }

    private IEnumerator PlaySoundAndLoadSceneCoroutine(int sceneIndex)
    {
        Debug.Log("Entra");
        audioSource.Play();

        while (audioSource.isPlaying)
        {
            yield return null;
        }

        Debug.Log("Passa de WaitForSeconds");
        // Now, load the specified scene
        SceneManager.LoadScene(sceneIndex);
    }
}
