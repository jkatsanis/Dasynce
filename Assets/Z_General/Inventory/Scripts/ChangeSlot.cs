using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeSlot
{
    public Sprite changeSlotSprite { get; set; }
    public bool changeSlot { get; set; } = false;
    public GameObject changeSlotGameObject { get; set; }
    public int changeSlotIndx { get; private set; } 

    public void GetInput(PlayerController controller, int sliderIndx)
    {
        if (Input.GetMouseButtonDown(1) && !changeSlot)
        {
            GameObject slot = controller.inventory.slotData.slots[sliderIndx];
            if (controller.inventory.slotData.SlotHasEmptySprite(slot))
            {
                return;
            }
            changeSlotGameObject = slot;
            changeSlotSprite = slot.GetComponent<Image>().sprite;
            changeSlot = true;
            changeSlotIndx = sliderIndx;
        }
        else if (Input.GetMouseButtonDown(1) && changeSlot)
        {
            GameObject slot = controller.inventory.slotData.slots[sliderIndx];

            Image slotIndx = slot.GetComponent<Image>();
            slotIndx.sprite = changeSlotSprite;

            changeSlot = false;

            Replace(changeSlotGameObject, controller.inventory.itemSprites, controller, slot);
        }
    }

    private void Replace(GameObject changeSlot, ItemSprites itemsprites, PlayerController controller, GameObject slot)
    {
        Image img = changeSlot.GetComponent<Image>();
        img.sprite = itemsprites.defaultSlot;
        if (controller.inventory.slotData.stackSlotCount[changeSlotIndx] > 0)
        {
            TMP_Text changeSlotItems = changeSlot.GetComponentInChildren<TMP_Text>();
            TMP_Text slotItems = slot.GetComponentInChildren<TMP_Text>();

            slotItems.text = changeSlotItems.text;      //Changing stackslotcount text
            changeSlotItems.text = "";

            int items = controller.inventory.slotData.stackSlotCount[changeSlotIndx];

            controller.inventory.slotData.stackSlotCount[changeSlotIndx] = 0;  //Changing stackslotcount data
            controller.inventory.slotData.stackSlotCount[SlotData.GetSlotIndex(slot)] = items;
        }

        foreach (Item item in controller.inventory.inventoryItems)
        {
            if (item.itemSlotIndx == changeSlotIndx) 
            {
                item.itemSlotIndx = Convert.ToInt32(slot.gameObject.name);      //Changing the itemSlot index
            }
        }
    }
}
