using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class InventoryButton : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GetComponentInParent<InventorySlotUI>().OnSlotButton();
        GridSelectionManager.Instance.DraggingFromDock = true;
    }
}
