using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private int STORE_TUTORIAL_SCENE_BUILD_INDEX = 5;
    private int STORE_SCENE_BUILD_INDEX = 6;

    PlayerInventory inventory;

    private int currentWave, enemiesLeft, zombiesAmount, skeletonsAmount;

    [SerializeField]
    private AudioSource clickSound;
    [SerializeField]
    private int waitTime = 10;
    [SerializeField]
    Canvas gameOverCanvas, instructionsCanvas, waveOverCanvas;
    [SerializeField]
    TextMeshProUGUI coinsWonText;
    [SerializeField]
    GameObject zombie, skeleton;
    [SerializeField]
    Transform playerTransform;

    private int coinsWon;

    private int maxX = 15, maxY = -2, minX = 0, minY = -8;

    void Start()
    {
        inventory = PlayerInventory.Instance;
        gameOverCanvas.gameObject.SetActive(false);
        waveOverCanvas.gameObject.SetActive(false);
        currentWave = inventory.getCurrentWave();
        if (currentWave == 1) 
        {
            instructionsCanvas.gameObject.SetActive(true);
            Instantiate(zombie, new Vector3(10, -1, 0), Quaternion.identity);
        }
        else if (currentWave == 2)
        {
            Debug.Log("entra");
            instructionsCanvas.gameObject.SetActive(false);
            Instantiate(skeleton, new Vector3(10, -1, 0), Quaternion.identity);
        }
        else
        {
            instructionsCanvas.gameObject.SetActive(false);
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
        gameOverCanvas.gameObject.SetActive(true);
        instructionsCanvas.gameObject.SetActive(false);
        GameObject playerGameObject = GameObject.Find("Player");
        Destroy(playerGameObject);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    private void WaveOver()
    {
        inventory.incCurrentWave();
        StartCoroutine(WaveOverTimer());
    }

    public void GoToStore()
    {
        inventory.addCoinsTotal(coinsWon);
        GameObject playerGameObject = GameObject.Find("Player");

        Destroy(playerGameObject);

        if (currentWave == 1)
        {
            // SceneManager.LoadScene(STORE_TUTORIAL_SCENE_BUILD_INDEX);
            Debug.Log("Entra em GoToStore()");
            StartCoroutine(PlaySoundAndLoadSceneCoroutine(STORE_TUTORIAL_SCENE_BUILD_INDEX));
        }
        else
        {
            SceneManager.LoadScene(STORE_SCENE_BUILD_INDEX);
        }
    }

    private IEnumerator WaveOverTimer()
    {
        yield return new WaitForSeconds(waitTime);
        instructionsCanvas.gameObject.SetActive(false);
        waveOverCanvas.gameObject.SetActive(true);
        coinsWonText.text = "Moedas arrecadadas: " + coinsWon.ToString();
    }

    private IEnumerator PlaySoundAndLoadSceneCoroutine(int sceneIndex)
    {
        Debug.Log("Entra");
        clickSound.Play();

        // Wait for the audio clip to finish playing
        yield return new WaitForSeconds(clickSound.clip.length);

        // Now, load the specified scene
        SceneManager.LoadScene(sceneIndex);
    }
}
