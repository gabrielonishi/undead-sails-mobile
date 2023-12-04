using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    // Singleton pattern
    public static PlayerInventory Instance;

    [SerializeField]
    public int punchDamage;

    private int coinsTotal, currentWave, coinsWon;
    private int kickDamage, swordDamage, healthTotal;
    private bool alreadyWatchedAd = false;
    private void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            kickDamage = 0; 
            swordDamage = 0;
            healthTotal = 100;
            currentWave = 1;
            coinsTotal = 0;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void addCoinsTotal(int coins)
    {
        coinsTotal += coins;
    }

    public int getCoinsTotal()
    {
        return coinsTotal;
    }

    public int getCurrentWave()
    {
        return currentWave;
    }

    public void incCurrentWave()
    {
        currentWave++;
    }

    public int getPunchDamage()
    {
        return punchDamage;
    }

    public void addPunchDamage(int damage)
    {
        punchDamage += damage;
    }

    public int getKickDamage()
    {
        return kickDamage;
    }

    public void addKickDamage(int damage)
    {
        kickDamage += damage;
    }

    public int getSwordDamage()
    {
        return swordDamage;
    }

    public void addSwordDamage(int damage)
    {
        swordDamage += damage;
    }

    public int getHealthTotal()
    {
        return healthTotal;
    }

    public void addHealthTotal(int healthPoints)
    {
        healthTotal += healthPoints;
    }

    public void setCoinsWon(int coins)
    {
        coinsWon = coins;
    }

    public int getCoinsWon()
    {
        return coinsWon;
    }

    public void watchedAd()
    {
        alreadyWatchedAd = true;
    }

    public bool getAlreadyWatchedAd()
    {
        return alreadyWatchedAd;
    }
}
