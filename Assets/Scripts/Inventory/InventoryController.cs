using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryUI; 
    public GameObject[] startingItems; 
    public GameObject[] carriedItems; 

    private static List<string> savedItems = new List<string>();

    private Slot[] slots;
    private int selectedIndex = 0;

    void Start()
    {
        slots = inventoryUI.GetComponentsInChildren<Slot>();

        foreach (GameObject item in startingItems)
        {
            AddItem(item);
        }

        foreach (GameObject item in carriedItems)
        {
            if (savedItems.Contains(item.name))
            {
                AddItem(item);
            }
        }

        SelectSlot(0);
    }

    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (Keyboard.current[Key.Digit1 + i].wasPressedThisFrame)
            {
                SelectSlot(i);
            }
        }
    }

    public bool AddItem(GameObject item)
    {
        foreach (Slot slot in slots)
        {
            if (!slot.IsEmpty() && slot.currentItem.name == item.name)
            {
                return false;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.SetItem(item);

                if (!savedItems.Contains(item.name))
                {
                    savedItems.Add(item.name);
                }
                return true;
            }
        }

        Debug.Log("Inventory is full");
        return false;
    }

    void SelectSlot(int index)
    {
        selectedIndex = index;
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSelected(i == index);
        }
    }

    public static void ResetInventory()
    {
        savedItems.Clear();
    }
}