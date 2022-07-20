using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class pos : MonoBehaviour
{
    public TMP_Text text;
    GameObject player;
    PlayerController playerController;
    InventoryInputs inputs;
    GameObject inventory;
    bool b = false; 

    void Start()
    {
        StartCoroutine(Wiat());
    }

    void Update()
    {
        if (b)
        {
            int x = inputs.GetSliderIndex();
            text.text = x.ToString();          
        }       
    }

    private IEnumerator Wiat()
    {
        yield return new WaitForSeconds(1);
        inventory = GameObject.FindWithTag("Inventory");
        inputs = inventory.GetComponent<InventoryInputs>();

        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<PlayerController>();

        b = true;

    }
}
