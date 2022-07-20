using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InstantiateInventory : MonoBehaviour
{
    [SerializeField] private GameObject _inventory;

    public GameObject Instantiate()
    {
        GameObject inventory = Instantiate(_inventory);
        inventory.name = "Inventory";
        return inventory;
    }

    public static void GetInventory(out GameObject inv, InstantiateInventory instance, out Inventory inventory, out ItemSprites itemSprites)
    {
        inv = instance.Instantiate();
        itemSprites = inv.GetComponent<ItemSprites>();
        inventory = new Inventory(inv, itemSprites);
    }

}
