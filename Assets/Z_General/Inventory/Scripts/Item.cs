using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemType itemType { get; }
    public Sprite itemSprite { get; }
    public bool isStackable { get; }
    public int itemSlotIndx { get; set; }    
    
    public Item(ItemType itemType, Sprite itemSprite, bool isStackable)
    {
        this.itemType = itemType;
        this.itemSprite = itemSprite;
        this.isStackable = isStackable;
    }

    public static bool isManaPotionStackAble = true;
    public enum ItemType
    {
        ManaPotion
    };
    

}
