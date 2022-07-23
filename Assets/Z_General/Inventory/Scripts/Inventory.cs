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
    public GameObject[] slots { get; }
    public InventoryInputs inputs { get; private set; }
    public InventoryData inventoryData { get; }
    public GameObject[] stackSlots { get; private set; }
    public int[] stackSlotCount { get; private set; }

    public Inventory(GameObject inventoryGameobject, ItemSprites itemSprites)
    {
        this.itemSprites = itemSprites;
        inventoryItems = new List<Item>();

        this.inventoryGameobject = inventoryGameobject;
        inventoryData = inventoryGameobject.GetComponent<InventoryData>();
        stackSlots = inventoryData.stackSlots;
        stackSlotCount = new int[stackSlots.Length];
        inputs = inventoryGameobject.GetComponent<InventoryInputs>();
        slots = GetSlots();
    }

    public void AddItem(Item item)
    {
        if (IsASlotFree())
        {
            inventoryItems.Add(item);
            AddItemToSlot(item);
        }
    } 

    public void RemoveItem(Item item)
    {
        if (RemovePossibleStackedItem(item))
        {
            return;
        }

        inventoryItems.Remove(item);    
        ReplaceSlot(item);
    }
    public void RemoveItem(Item item, bool removeAt) 
    {
        if (RemovePossibleStackedItem(item))
        {
            return;
        }

        inventoryItems.RemoveAt(GetRemoveIndex(inputs.GetSliderIndex()));
        ReplaceSlot(inputs.GetSliderIndex());
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
            TMP_Text text = stackSlots[item.itemSlotIndx].GetComponent<TMP_Text>();


            if (IsLastitemInStackList(item))
            {
                text.text = string.Empty;
                stackSlotCount[item.itemSlotIndx]--;
                return false;   
            }
            else
            {
                inventoryItems.Remove(item);
                stackSlotCount[item.itemSlotIndx]--;
                text.text = stackSlotCount[item.itemSlotIndx].ToString();
                return true;
            }
        }
        return false;
    }
    private bool IsLastitemInStackList(Item item)
    {
        int stackSlotCnt = stackSlotCount[item.itemSlotIndx];
        return stackSlotCnt == 1;
    }
    private void ReplaceSlot(Item item)
    {
        Image image = slots[item.itemSlotIndx].GetComponent<Image>();
        image.sprite = itemSprites.defaultSlot;
    }
    private void ReplaceSlot(int indx)
    {
        Image image = slots[indx].GetComponent<Image>();
        image.sprite = itemSprites.defaultSlot;
    }

    private void AddItemToSlot(Item item)
    {
        if (item.isStackable)
        {
            GameObject slot = GetSlotToModify(item, out bool counterDoesExist, out int index);

            TMP_Text stackSlotText = stackSlots[index].GetComponent<TMP_Text>();
            stackSlotCount[index]++;
            stackSlotText.text = stackSlotCount[index].ToString();

            if (!counterDoesExist)
            {
                Image image = slot.GetComponent<Image>();
                image.sprite = item.itemSprite;
                item.itemSlotIndx = index;
            }
            return;
        }
        for (int i = 0; i <= INVENTORY_SLOTS; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name == INVENTORY_EMPTY_SLOT_NAME)
            {
                item.itemSlotIndx = i;
                image.sprite = item.itemSprite;
                return; 
            }
        }
    }
    
    private GameObject GetSlotToModify(Item item, out bool counterDoesExist, out int indx)
    {
        for (int i = 0; i <= INVENTORY_SLOTS; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name.ToLower() == item.itemType.ToString().ToLower())
            {
                indx = i;
                counterDoesExist = true;
                return slots[i];
            }
        }
        for (int i = 0; i <= INVENTORY_SLOTS; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name == INVENTORY_EMPTY_SLOT_NAME)
            {
                indx = i;
                counterDoesExist = false;
                return slots[i];
            }
        }
        indx = -1;
        counterDoesExist = false;
        return null;
    }

    private GameObject[] GetSlots()
    {
        GameObject[] invSlots = new GameObject[INVENTORY_SLOTS + 1];
        for (int i = 0; i <= INVENTORY_SLOTS; i++)
        {
            invSlots[i] = GameObject.Find(i.ToString());
        }
        return invSlots;
    }

    public bool IsSliderOnItem(Item.ItemType itemType)
    {
        Image image = slots[inputs.GetSliderIndex()].GetComponent<Image>();
        return image.sprite.name.ToLower() == itemType.ToString().ToLower();   
    }

    private bool IsASlotFree()
    {
        for (int i = 0; i <= INVENTORY_SLOTS; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name == INVENTORY_EMPTY_SLOT_NAME)
            {
                return true;
            }
        }
        return false;
    }

    public int GetRemoveIndex(int ind)
    {
        int indx = 0;
        for (int i = 0; i < ind; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name != INVENTORY_EMPTY_SLOT_NAME)
            {
                indx++;
            }
        }
        return indx;
       
    }
}
