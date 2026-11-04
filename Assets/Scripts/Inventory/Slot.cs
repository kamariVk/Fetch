using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Image icon;  
    public GameObject currentItem; 

    void Awake()
    {
        icon.enabled = false;
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public void SetItem(GameObject item)
    {
        currentItem = item;

        Sprite sprite = null;

        ItemIcon itemIcon = item.GetComponent<ItemIcon>();
        if (itemIcon != null)
        {
            sprite = itemIcon.icon;
        }
        else
        {
            SpriteRenderer sr = item.GetComponentInChildren<SpriteRenderer>(true);
            if (sr != null) sprite = sr.sprite;
        }

        if (sprite != null)
        {
            icon.sprite = sprite;
            icon.enabled = true;
        }
     
    }

    public void ClearItem()
    {
        currentItem = null;
        icon.sprite = null;
        icon.enabled = false;
    }

    public void SetSelected(bool selected)
    {
        GetComponent<Image>().color = selected ? Color.yellow : Color.white;
    }
}