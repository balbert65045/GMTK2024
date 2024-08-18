using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class InventorySlotUI : MonoBehaviour
{
    public TMP_Text CostText;
    public GameObject Icon;
    public Button SlotButton;
    public InventoryItem inventoryItem { get; private set; }
    [SerializeField] BlockType blockType;

    public void AddItem(InventoryItem newItem)
    {
        inventoryItem = newItem;
        blockType = inventoryItem.BlockData.Type;
        if (setImage())
        {
            Image image = Icon.GetComponent<Image>();
            image.sprite = inventoryItem.BlockData.Icon;
            SlotButton.interactable = true;
        }
        else
        {
            Debug.LogWarning("Couldn't Set Inventory Slot");
            SlotButton.interactable = false;
        }
        setCost(inventoryItem.BlockData.Cost);
    }

    private void setCost(int cost)
    {
        CostText.text = cost.ToString();
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
        Debug.Log("Button Clicked: " + blockType.ToString());
        InventoryController.Instance.SelectedBlockTypeChangedEvent?.Invoke(blockType);
    }
}
