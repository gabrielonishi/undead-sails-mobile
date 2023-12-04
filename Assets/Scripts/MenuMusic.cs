using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMusic : MonoBehaviour
{
    [SerializeField]
    private AudioSource menuSong, gameSong, storeSong;
    private static MenuMusic instance;
    private int GAME_SCENE_BUILD_INDEX = 6;
    private int STORE_SCENE_BUILD_INDEX = 8;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            menuSong.Play();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // This method will be called every time a new scene is loaded.
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == GAME_SCENE_BUILD_INDEX)
        {
            storeSong.Stop();
            menuSong.Stop();
            gameSong.Play();
        } else if (currentSceneIndex == STORE_SCENE_BUILD_INDEX) {
            menuSong.Stop();
            gameSong.Stop();
            storeSong.Play();
        } else if (currentSceneIndex == 0) {
            storeSong.Stop();
            menuSong.Play();
            gameSong.Stop();
        }
    }
}
