using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    // An array of all the items the player can hold
    public GameObject[] items;
    
    // The transform where the item will spawn in
    public Transform itemSpawnPoint;
    
    // The index of the currently selected item
    public int currentItemIndex = 0;
    
    // The game object of the current item the player is holding
    private GameObject currentItem;

    private void Start()
    {
        // When the scene starts, select the item with the currentItemIndex
        SelectItem(currentItemIndex);
    }

    private void Update()
    {
        // Keep track of the previous item index
        int previousItemIndex = currentItemIndex;

        // Scroll through the inventory items with the mouse wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (currentItemIndex >= items.Length - 1)
            {
                currentItemIndex = 0;
            }
            else
            {
                currentItemIndex++;
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (currentItemIndex <= 0)
            {
                currentItemIndex = items.Length - 1;
            }
            else
            {
                currentItemIndex--;
            }
        }

        // Select an item with the corresponding key
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectItem(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectItem(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectItem(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectItem(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectItem(4);
        }

        // If the current item has changed, select the new item
        if (previousItemIndex != currentItemIndex)
        {
            SelectItem(currentItemIndex);
        }
    }

    // Spawn in the selected item and destroy the previous one
    private void SelectItem(int index)
    {
        if (index < 0 || index >= items.Length)
        {
            return;
        }

        // Destroy the previous item, if there was one
        if (currentItem != null)
        {
            Destroy(currentItem);
        }

        // Instantiate the new item at the spawn point
        currentItem = Instantiate(items[index], itemSpawnPoint.position, itemSpawnPoint.rotation, itemSpawnPoint);
    }

    // Switch to the item at the given index
    public void ActivateItem(int index)
    {
        if (index < 0 || index >= items.Length)
        {
            return;
        }

        currentItemIndex = index;
        SelectItem(currentItemIndex);
    }

    // Get the current item
    public GameObject GetCurrentItem()
    {
        return currentItem;
    }
}