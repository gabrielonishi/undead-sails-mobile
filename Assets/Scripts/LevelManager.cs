using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int WAVE_OVER_SCENE_BUILD_INDEX = 13;
    private int YOU_LOSE_SCENE_BUILD_INDEX = 14;
    private int STORE_TUTORIAL_SCENE_BUILD_INDEX = 7;
    private int STORE_SCENE_BUILD_INDEX = 8;

    PlayerInventory inventory;

    private int currentWave, enemiesLeft, zombiesAmount, skeletonsAmount;

    [SerializeField]
    private AudioSource clickSound;
    [SerializeField]
    private int waitTime = 10;
    [SerializeField]
    GameObject zombie, skeleton;
    [SerializeField]
    Transform playerTransform;
    [SerializeField]
    private GameObject kickButton, slashButton; 

    private int coinsWon;

    private int maxX = 15, maxY = -2, minX = 0, minY = -8;

    void Start()
    {
        inventory = PlayerInventory.Instance;
        currentWave = inventory.getCurrentWave();
        if (currentWave == 1) 
        {
            Instantiate(zombie, new Vector3(10, -1, 0), Quaternion.identity);
        }
        else if (currentWave == 2)
        {
            Instantiate(skeleton, new Vector3(10, -1, 0), Quaternion.identity);
        }
        else
        {
            enemiesLeft = currentWave;
            zombiesAmount = Random.Range(0, enemiesLeft);
            skeletonsAmount = enemiesLeft - zombiesAmount;

            for(int i = 0;  i < skeletonsAmount; i++)
            { 
                Instantiate(skeleton, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), Quaternion.identity);
            }

            for (int i = 0; i < zombiesAmount; i++)
            {
                Instantiate(zombie, new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0), Quaternion.identity);
            }
        }

        if (inventory.getKickDamage() > 0)
        {
            kickButton.SetActive(true);
        }
        else 
        {
            kickButton.SetActive(false);
        }
        if (inventory.getSwordDamage() > 0)
        {
            slashButton.SetActive(true);
        }
        else
        {
            slashButton.SetActive(false);
        }
    }

    public void enemyDied(int coinsDropped)
    {
        enemiesLeft -= 1;
        coinsWon += coinsDropped;

        if (enemiesLeft <= 0)
        {
            WaveOver();
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameOverTimer());
    }

    private IEnumerator GameOverTimer()
    { 
        yield return new WaitForSeconds(waitTime);
        GameObject playerGameObject = GameObject.Find("Player");
        Destroy(playerGameObject);
        SceneManager.LoadScene(YOU_LOSE_SCENE_BUILD_INDEX);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void WaveOver()
    {
        inventory.incCurrentWave();
        inventory.addCoinsTotal(coinsWon);
        inventory.setCoinsWon(coinsWon);
        StartCoroutine(WaveOverTimer());
    }

    private IEnumerator WaveOverTimer()
    {
        yield return new WaitForSeconds(waitTime);
        GameObject playerGameObject = GameObject.Find("Player");
        Destroy(playerGameObject);
        SceneManager.LoadScene(WAVE_OVER_SCENE_BUILD_INDEX);
    }

    public void GoToStore()
    {
        GameObject playerGameObject = GameObject.Find("Player");
        Destroy(playerGameObject);

        if (currentWave == 1)
        {
            // SceneManager.LoadScene(STORE_TUTORIAL_SCENE_BUILD_INDEX);
            StartCoroutine(PlaySoundAndLoadSceneCoroutine(STORE_TUTORIAL_SCENE_BUILD_INDEX));
        }
        else
        {
            SceneManager.LoadScene(STORE_SCENE_BUILD_INDEX);
        }
    }

    private IEnumerator PlaySoundAndLoadSceneCoroutine(int sceneIndex)
    {
        clickSound.Play();

        yield return new WaitForSeconds(clickSound.clip.length);

        SceneManager.LoadScene(sceneIndex);
    }
}
