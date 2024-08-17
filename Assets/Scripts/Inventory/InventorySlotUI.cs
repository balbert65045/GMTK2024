using UnityEngine;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour
{
    public GameObject Icon;
    public Button SlotButton;
    public InventoryItem inventoryItem { get; private set; }

    public void AddItem(InventoryItem newItem)
    {
        inventoryItem = newItem;
        if (setImage())
        {
            Image image = Icon.GetComponent<Image>();
            image.sprite = inventoryItem.BlockData.Icon;
        }
        else
        {
            Debug.LogWarning("Couldn't Set Inventory Slot");
            SlotButton.interactable = false;
        }
        SlotButton.interactable = true;
    }

    private bool setImage()
    {
        Image image = Icon.GetComponent<Image>();
        image.sprite = inventoryItem.BlockData.Icon;
        if (image == null)
            return false;
        else
            return true;
    }

    public void OnSlotButton()
    {
        Debug.Log("Button Clicked: " + inventoryItem.BlockData.Type.ToString());
        InventoryController.Instance.SelectedBlockTypeChangedEvent?.Invoke(inventoryItem.BlockData.Type);
    }
}
