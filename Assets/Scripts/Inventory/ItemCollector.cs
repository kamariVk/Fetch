using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    private InventoryController inventoryController;

    void Start()
    {
        inventoryController = GetComponent<InventoryController>();

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item"))
        {
            bool itemAdded = inventoryController.AddItem(collision.gameObject);
            if (itemAdded)
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

}
