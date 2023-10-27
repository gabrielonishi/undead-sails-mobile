using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ui_shop : MonoBehaviour
{
    public Transform container;
    public Transform shopItemTemplate;

    private void Awake()
    {
        // container = transform.Find("container");
        // print(container);
        // shopItemTemplate = container.Find("NewAbilityTemplate");
        // print(shopItemTemplate);
        // shopItemTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        CreateItemButton(Item.GetSprite(Item.ItemType.Slashing), "Slashing", Item.GetCost(Item.ItemType.Slashing), 0);
        CreateItemButton(Item.GetSprite(Item.ItemType.Kicking), "Kicking", Item.GetCost(Item.ItemType.Kicking), 1);
        CreateItemButton(Item.GetSprite(Item.ItemType.Sliding), "Sliding", Item.GetCost(Item.ItemType.Sliding), 2);
        CreateItemButton(Item.GetSprite(Item.ItemType.Health), "Health", Item.GetCost(Item.ItemType.Health), 3);
    }

    private void CreateItemButton(Sprite itemSprite, string itemName, int itemCost, int positionIndex){
        Transform shopItemTransform = Instantiate(shopItemTemplate, container);
        RectTransform shopItemRectTransform = shopItemTransform.GetComponent<RectTransform>();

        float shopItemHeight = 90f;
        shopItemRectTransform.anchoredPosition = new Vector2(0, -shopItemHeight * positionIndex);

        shopItemTransform.Find("itemImage").GetComponent<Image>().sprite = itemSprite;
        shopItemTransform.Find("nameText").GetComponent<TextMeshProUGUI>().SetText(itemName);
        shopItemTransform.Find("priceText").GetComponent<TextMeshProUGUI>().SetText(itemCost.ToString());
    }
}
