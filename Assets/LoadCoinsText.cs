using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadCoinsText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] 
    private TMP_Text coinsText ;
    private PlayerInventory inventory;

    void Start()
    {
        inventory = PlayerInventory.Instance;
        coinsText.text = "Coins won: " + inventory.getCoinsWon();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
