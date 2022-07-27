using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInputs : MonoBehaviour
{
    [SerializeField] public Transform _slider;

    public ChangeSlot changeSlot;

    private KeyCode[] _validSlotInputs;
    private const int MOVING_SPACE_X = 65;
    private const int START_OF_INVENTORY_X = 61;

    private PlayerController _playerController;
    private void Start()
    {
        changeSlot = new ChangeSlot();

        GameObject player = GameObject.Find("CurrentScientist");
        _playerController = player.GetComponent<PlayerController>();

        _validSlotInputs = new[] 
        { 
            KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, 
            KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6
        };
    }

    private void Update()
    {
        KeyCode sliderPosition = GetSliderPosition();
        if (sliderPosition != KeyCode.None)
        {   
            MoveSliderToPosition(sliderPosition);
        }
            
        changeSlot.GetInput(_playerController, GetSliderIndex());

        if (Input.GetKeyDown(KeyCode.K))
        {
            foreach (Item item in _playerController.inventory.inventoryItems)
            {
                print(item.itemSlotIndx);
            }
        }
   
    }

    private KeyCode GetSliderPosition()
    {
        for (int i = 0; i < _validSlotInputs.Length; i++)   
        {
            if (Input.GetKeyDown(_validSlotInputs[i]))
            {
                return _validSlotInputs[i];
            }
        }
        return KeyCode.None;
    }

    private void MoveSliderToPosition(KeyCode sliderPos)
    {
        string sldStr = sliderPos.ToString();
        int indx = (sldStr[5] - '0') - 1;
        float xPosition = START_OF_INVENTORY_X + MOVING_SPACE_X * indx;
        _slider.transform.position = new Vector2(xPosition, _slider.transform.position.y);
    }

    public int GetSliderIndex()
    {
        float xPos = _slider.transform.position.x;
        return Convert.ToInt32((xPos / MOVING_SPACE_X) - START_OF_INVENTORY_X + 60);
    }
}
