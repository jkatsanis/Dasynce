using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotData
{
    public GameObject[] slots { get; private set; }
    public GameObject[] stackSlots { get; private set; }
    public int[] stackSlotCount { get; private set; }

    public SlotData(GameObject[] stackSlots)
    {
        this.stackSlots = stackSlots;
        slots = GetSlots();
        stackSlotCount = new int[stackSlots.Length];
    }


    public bool SlotHasEmptySprite(GameObject slot)
    {
        Image image = slot.GetComponent<Image>();

        if (image.sprite.name == Inventory.INVENTORY_EMPTY_SLOT_NAME)
        {
            return true;
        }
        return false;
    }

    public bool IsASlotFree()
    {
        for (int i = 0; i <= Inventory.INVENTORY_SLOTS; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name == Inventory.INVENTORY_EMPTY_SLOT_NAME)
            {
                return true;
            }
        }
        return false;
    }

    public void ReplaceSlot(Item item, ItemSprites itemSprites)
    {
        Image image = slots[item.itemSlotIndx].GetComponent<Image>();
        image.sprite = itemSprites.defaultSlot;
    }
    public void ReplaceSlot(int indx, ItemSprites itemSprites)
    {
        Image image = slots[indx].GetComponent<Image>();
        image.sprite = itemSprites.defaultSlot;
    }

    public void AddItemToSlot(Item item)
    {
        if (item.isStackable)
        {
            GameObject slot = GetSlotToModify(item, out bool counterDoesExist, out int index);

            TMP_Text stackSlotText = stackSlots[index].GetComponent<TMP_Text>();
            stackSlotCount[index]++;
            stackSlotText.text = stackSlotCount[index].ToString();
            item.itemSlotIndx = index;

            if (!counterDoesExist)
            {
                Image image = slot.GetComponent<Image>();
                image.sprite = item.itemSprite;
            }
            return;
        }
        for (int i = 0; i <= Inventory.INVENTORY_SLOTS; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name == Inventory.INVENTORY_EMPTY_SLOT_NAME)
            {
                item.itemSlotIndx = i;
                image.sprite = item.itemSprite;
                return;
            }
        }
    }

    public GameObject GetSlotToModify(Item item, out bool counterDoesExist, out int indx)
    {
        for (int i = 0; i <= Inventory.INVENTORY_SLOTS; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name.ToLower() == item.itemType.ToString().ToLower())
            {
                indx = i;
                counterDoesExist = true;
                return slots[i];
            }
        }
        for (int i = 0; i <= Inventory.INVENTORY_SLOTS; i++)
        {
            Image image = slots[i].GetComponent<Image>();
            if (image.sprite.name == Inventory.INVENTORY_EMPTY_SLOT_NAME)
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
        GameObject[] invSlots = new GameObject[Inventory.INVENTORY_SLOTS + 1];
        for (int i = 0; i <= Inventory.INVENTORY_SLOTS; i++)
        {
            invSlots[i] = GameObject.Find(i.ToString());
        }
        return invSlots;
    }

    public static int GetSlotIndex(GameObject slot)
    {
        return Convert.ToInt32(slot.name);
    }

}
