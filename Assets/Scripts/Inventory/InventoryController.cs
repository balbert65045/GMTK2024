using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InventoryController: MonoBehaviour
{
    public static InventoryController Instance { get; private set; }

    public UnityEvent ItemChangedEvent;
    public UnityEvent<BlockType> SelectedBlockTypeChangedEvent;
    public List<InventoryItem> InventoryItems { get; private set; }
    public BlockSO[] DefaultBlocks;

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

    void Start()
    {
        InventoryItems = new List<InventoryItem>();
        if (ItemChangedEvent == null) ItemChangedEvent = new UnityEvent();
        if (SelectedBlockTypeChangedEvent == null) SelectedBlockTypeChangedEvent = new UnityEvent<BlockType>();

        Initialize();
    }

    void OnDestroy()
    {
        ItemChangedEvent.RemoveAllListeners();
        SelectedBlockTypeChangedEvent.RemoveAllListeners();
    }

    void Initialize()
    {
        if (DefaultBlocks.Length > 0)
        {
            foreach (BlockSO blockSO in DefaultBlocks)
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