using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] GameObject InventoryUIPanel;
    [SerializeField] Transform InventorySlotsContainer;
    [SerializeField] GameObject slotPrefab;

    InventoryController inventoryController;
    List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        inventoryController = InventoryController.Instance;
        inventoryController.ItemChangedEvent?.AddListener(UpdateSlotsUI);
        GameManager.Instance.GameModeChangedEvent?.AddListener(ToggleInventoryUIPanel);
        // TODO: Highlight if item selected through hotkeys

        UpdateSlotsUI();
        ToggleInventoryUIPanel(GameManager.Instance.Mode);
    }

    void OnDestroy()
    {
        GameManager.Instance.GameModeChangedEvent.RemoveListener(ToggleInventoryUIPanel);
    }

    void UpdateSlotsUI()
    {
        if (inventoryController.InventoryItems != null)
        {
            Debug.Log("InventoryItems Count: " + inventoryController.InventoryItems.Count);
            if (slots.Count > inventoryController.InventoryItems.Count)
            {
                for (int j = inventoryController.InventoryItems.Count; j < slots.Count; j++)
                {
                    Destroy(slots[j]);
                    slots.RemoveAt(j);
                }
            }

            for (int i = 0; i < inventoryController.InventoryItems.Count; i++)
            {
                if (i > slots.Count -1 || slots[i] == null)
                {
                    slots.Add(Instantiate(slotPrefab, InventorySlotsContainer));
                    slots[i].transform.SetParent(InventorySlotsContainer);
                }
                slots[i].GetComponent<InventorySlotUI>()?.AddItem(inventoryController.InventoryItems[i]);
            }
        }
    }

    public void ToggleInventoryUIPanel(GameMode mode)
    {
        if (mode == GameMode.BUILD_MODE)
            InventoryUIPanel.SetActive(true);
        else
            InventoryUIPanel.SetActive(false);
    }
}
