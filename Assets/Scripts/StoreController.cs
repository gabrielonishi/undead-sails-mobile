using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoreController : MonoBehaviour
{
    private int GAME_SCENE_BUILD_INDEX = 4;
    private int SKELETON_TUTORIAL_SCENE_BUILD_INDEX = 7;
    private int SWORD_TUTORIAL_SCENE_BUILD_INDEX = 8;
    private int KICK_TUTORIAL_SCENE_BUILD_INDEX = 9;
    private int GAME_OVER_SCENE_BUILD_INDEX = 10;

    [SerializeField]
    private AudioSource clickSound, buySound;

    PlayerInventory inventory;

    [SerializeField]
    int unlockPunchPrice, upgradePunchPrice, upgradePunchDamage;

    [SerializeField]
    int unlockKickPrice, kickBaseDamage, upgradeKickPrice, upgradeKickDamage;

    [SerializeField]
    int unlockSwordPrice, swordBaseDamage, upgradeSwordPrice, upgradeSwordDamage;

    [SerializeField]
    int upgradeHPPrice, upgradeHPAmount, ticketPrice;

    [SerializeField]
    TextMeshProUGUI coinBalanceText;

    [SerializeField]
    TextMeshProUGUI currentPunchDamageText, unlockPunchPriceText, upgradePunchPriceText, unlockPunchText;

    [SerializeField]
    TextMeshProUGUI currentKickDamageText, unlockKickPriceText, upgradeKickPriceText, unlockKickText;

    [SerializeField]
    TextMeshProUGUI currentSwordDamageText, unlockSwordPriceText, upgradeSwordPriceText, unlockSwordText;

    [SerializeField]
    TextMeshProUGUI totalHPText, upgradeLifePriceText, ticketPriceText;

    [SerializeField]
    Button unlockPunchButton, unlockKickButton, unlockSwordButton;

    private int coinBalance, punchDamage, kickDamage, swordDamage, currentWave;

    void Start()
    {
        inventory = PlayerInventory.Instance;
        fetchVariables();
        initializeText();
        updateUnlockTexts();
    }

    private void fetchVariables()
    {
        coinBalance = inventory.getCoinsTotal();
        punchDamage = inventory.getPunchDamage();
        kickDamage = inventory.getKickDamage();
        swordDamage = inventory.getSwordDamage();
    }

    private void initializeText()
    {
        coinBalanceText.text = coinBalance.ToString();
        currentPunchDamageText.text = "Dano Atual: " + inventory.getPunchDamage().ToString();
        unlockPunchPriceText.text = unlockPunchPrice.ToString();
        upgradePunchPriceText.text = upgradePunchPrice.ToString();
        currentKickDamageText.text = "Dano Atual: " + inventory.getKickDamage().ToString();
        unlockKickPriceText.text = unlockKickPrice.ToString();
        upgradeKickPriceText.text = upgradeKickPrice.ToString();
        currentSwordDamageText.text = "Dano Atual: " + inventory.getSwordDamage().ToString();
        unlockSwordPriceText.text = unlockSwordPrice.ToString();
        upgradeSwordPriceText.text = upgradeSwordPrice.ToString();
        totalHPText.text = "HP Atual: " + inventory.getHealthTotal().ToString();
        ticketPriceText.text = ticketPrice.ToString();
    }

    private void updateUnlockTexts()
    {
        if(punchDamage > 0)
        {
            unlockPunchButton.interactable = false;
            unlockPunchText.text = $"<s>DESBLOQUEAR</s>";
        }

        if(swordDamage > 0)
        {
            unlockSwordButton.interactable = false;
            unlockSwordText.text = $"<s>DESBLOQUEAR</s>";
        }

        if(kickDamage > 0)
        {
            unlockKickButton.interactable = false;
            unlockKickText.text = $"<s>DESBLOQUEAR</s>";
        }
    }

    public bool isPurchasable(int price)
    {
        if (coinBalance >= price) return true;
        return false;
    }

    public void buy(int price)
    {
        buySound.Play();
        coinBalance -= upgradePunchPrice;
        inventory.addCoinsTotal(-price);
        coinBalanceText.text = coinBalance.ToString();
    }

    public void upgradePunch()
    {
        if (isPurchasable(upgradePunchPrice))
        {
            buy(upgradePunchPrice);
            inventory.addPunchDamage(upgradePunchDamage);
            currentPunchDamageText.text = "Dano Atual: " + inventory.getPunchDamage().ToString();
        }
    }

    public void unlockKick()
    {
        if (isPurchasable(unlockKickPrice))
        {
            buy(unlockKickPrice);
            inventory.addKickDamage(kickBaseDamage);
            currentKickDamageText.text = "Dano Atual: " + inventory.getKickDamage().ToString();
            updateUnlockTexts();
            SceneManager.LoadScene(KICK_TUTORIAL_SCENE_BUILD_INDEX);
        }
    }

    public void upgradeKick()
    {
        if (isPurchasable(upgradeKickPrice) && kickDamage > 0)
        {
            buy(upgradeKickPrice);
            inventory.addKickDamage(upgradeKickDamage);
            currentKickDamageText.text = "Dano Atual: " + inventory.getKickDamage().ToString();
        }
    }

    public void unlockSword()
    {
        if (isPurchasable(unlockSwordPrice))
        {
            buy(unlockSwordPrice);
            inventory.addSwordDamage(swordBaseDamage);
            currentSwordDamageText.text = "Dano Atual: " + inventory.getSwordDamage().ToString();
            updateUnlockTexts();
            SceneManager.LoadScene(SWORD_TUTORIAL_SCENE_BUILD_INDEX);
        }
    }

    public void upgradeSword()
    {
        if(isPurchasable(upgradeSwordDamage) && swordDamage > 0)
        {
            buy(upgradeSwordDamage);
            inventory.addSwordDamage(upgradeSwordDamage);
            currentSwordDamageText.text = "Dano Atual: " + inventory.getSwordDamage().ToString();
        }
    }

    public void upgradeHP()
    {
        if (isPurchasable(upgradeHPPrice))
        {
            buy(upgradeHPPrice);
            inventory.addHealthTotal(upgradeHPAmount);
            totalHPText.text = "HP Atual: " + inventory.getHealthTotal().ToString();
        }
    }

    public void goToNextWave()
    {
        currentWave = inventory.getCurrentWave();
        if (currentWave == 2)
        {
            StartCoroutine(PlaySoundAndLoadSceneCoroutine(SKELETON_TUTORIAL_SCENE_BUILD_INDEX));
            //SceneManager.LoadScene(SKELETON_TUTORIAL_SCENE_BUILD_INDEX);
        }
        else
        {
            StartCoroutine(PlaySoundAndLoadSceneCoroutine(GAME_SCENE_BUILD_INDEX));
            //SceneManager.LoadScene(GAME_SCENE_BUILD_INDEX);
        }
    }

    public void buyTicket()
    {
        if (isPurchasable(ticketPrice))
        {
            buy(ticketPrice);
            Destroy(inventory);
            StartCoroutine(PlaySoundAndLoadSceneCoroutine(GAME_OVER_SCENE_BUILD_INDEX));
            // SceneManager.LoadScene(GAME_OVER_SCENE_BUILD_INDEX);
        }
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
