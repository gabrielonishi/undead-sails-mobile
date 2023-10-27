using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType {
        Slashing,
        Kicking,
        Sliding,
        Health
    }

    public static int GetCost(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.Slashing: return 100;
            case ItemType.Kicking: return 150;
            case ItemType.Sliding: return 200;
            case ItemType.Health: return 50;
        }
    }

    public static Sprite GetSprite(ItemType itemType) {
        switch (itemType)
        {
            default:
            case ItemType.Slashing: return GameAssets.i.s_Slashing;
            case ItemType.Kicking: return GameAssets.i.s_Kicking;
            case ItemType.Sliding: return GameAssets.i.s_Sliding;
            case ItemType.Health: return GameAssets.i.s_Health;
        }
    }
}
