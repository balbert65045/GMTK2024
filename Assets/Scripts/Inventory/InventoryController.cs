using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController: MonoBehaviour
{
    public static InventoryController Instance { get; private set; }

    public UnityEvent ItemChangedEvent;
    public List<InventoryItem> InventoryItems { get; private set; }

    [SerializeField] BlockSO[] defaultBlocks;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void OnDestroy()
    {
        ItemChangedEvent.RemoveAllListeners();
    }

    void Start()
    {
        InventoryItems = new List<InventoryItem>();

        Initialize();
    }

    void Initialize()
    {
        if (defaultBlocks.Length > 0)
        {
            foreach (BlockSO blockSO in defaultBlocks)
            {
                AddItem(blockSO);
            }
        }
    }

    public void AddItem(BlockSO blockSO)
    {
        if (SearchForItem(blockSO) < 0)
        {
            InventoryItems.Add(new InventoryItem(blockSO));
            ItemChangedEvent?.Invoke();
        }
    }

    public int SearchForItem(BlockSO blockSO)
    {
        for (int i = 0; i < InventoryItems.Count; i++)
        {
            if (InventoryItems[i].BlockData.Type == blockSO.Type)
            {
                return i;
            }
        }
        return -1;
    }

    public void RemoveItem(InventoryItem item)
    {
        if (SearchForItem(item.BlockData) > -1)
        {
            InventoryItems.Remove(item);
            ItemChangedEvent?.Invoke();
        }
    }
}