using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory 
{
    public const string INVENTORY_EMPTY_SLOT_NAME = "slot_empty";
    public const int INVENTORY_SLOTS = 5;       //starting on zero!
    public GameObject inventoryGameobject { get; private set; }   
    public List<Item> inventoryItems { get; private set; }
    public ItemSprites itemSprites { get; }
    public InventoryInputs inputs { get; private set; }
    public InventoryData inventoryData { get; }
    public SlotData slotData { get; private set; }
    public Inventory(GameObject inventoryGameobject, ItemSprites itemSprites)
    {
        this.itemSprites = itemSprites;
        inventoryItems = new List<Item>();

        this.inventoryGameobject = inventoryGameobject;
        inventoryData = inventoryGameobject.GetComponent<InventoryData>();
        inputs = inventoryGameobject.GetComponent<InventoryInputs>();
        slotData = new SlotData(inventoryData.stackSlots);
    }

    public void AddItem(Item item)
    {
        if (slotData.IsASlotFree())
        {
            inventoryItems.Add(item);
            slotData.AddItemToSlot(item);
        }
    } 

    public void RemoveItem(Item item)
    {
        if (RemovePossibleStackedItem(item))
        {
            return;
        }

        inventoryItems.Remove(item);    
        slotData.ReplaceSlot(item, itemSprites);
    }
    public void RemoveItem(Item item, bool removeAt) 
    {
        if (RemovePossibleStackedItem(item))
        {
            return;
        }

        inventoryItems.RemoveAt(GetRemoveIndex(inputs.GetSliderIndex()));
        slotData.ReplaceSlot(inputs.GetSliderIndex(), itemSprites);
    }
    public bool ExistItem(Item.ItemType itemType)
    {
        foreach (Item item in inventoryItems)
        {
            if (item.itemType == itemType)
            {
                return true;
            }
        }
        return false;
    }

    public bool ExistItem(Item.ItemType itemType, out Item original)
    {
        foreach (Item item in inventoryItems)
        {
            if (item.itemType == itemType)
            {
                original = item;
                return true;
            }
        }
        original = null;
        return false;   
    }

    private bool RemovePossibleStackedItem(Item item)
    {
        if (item.isStackable)
        {
            TMP_Text text = slotData.stackSlots[item.itemSlotIndx].GetComponent<TMP_Text>();

            if (IsLastitemInStackList(item))
            {
                text.text = string.Empty;
                slotData.stackSlotCount[item.itemSlotIndx]--;
                return false;   
            }
            else
            {
                inventoryItems.Remove(item);
                slotData.stackSlotCount[item.itemSlotIndx]--;
                text.text = slotData.stackSlotCount[item.itemSlotIndx].ToString();
                return true;
            }
        }
        return false;
    }
    private bool IsLastitemInStackList(Item item)
    {
        int stackSlotCnt = slotData.stackSlotCount[item.itemSlotIndx];
        return stackSlotCnt == 1;
    }
  
    public bool IsSliderOnItem(Item.ItemType itemType)
    {
        Image image = slotData.slots[inputs.GetSliderIndex()].GetComponent<Image>();
        return image.sprite.name.ToLower() == itemType.ToString().ToLower();   
    }

    public int GetRemoveIndex(int ind)
    {
        int indx = 0;
        for (int i = 0; i < ind; i++)
        {
            Image image = slotData.slots[i].GetComponent<Image>();
            if (image.sprite.name != INVENTORY_EMPTY_SLOT_NAME)
            {
                indx++;
            }
        }
        return indx;
       
    }
}
