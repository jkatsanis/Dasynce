using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType itemType { get; }
    public Sprite itemSprite { get; }
    public int itemSlotIndx { get; set; }    
    
    public Item(ItemType itemType, Sprite itemSprite)
    {
        this.itemType = itemType;
        this.itemSprite = itemSprite;
    }

    public enum ItemType
    {
        ManaPotion
    };
    

}
