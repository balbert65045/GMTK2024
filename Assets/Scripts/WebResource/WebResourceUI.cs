using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WebCountUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI webCountText;

    // Start is called before the first frame update
    void Start()
    {
        WebResourceController.Instance.WebResourceChangedEvent?.AddListener(UpdateWebCountText);

        UpdateWebCountText(0);
    }

    void UpdateWebCountText(int webCount)
    {
        webCountText.text = "Web Count: " + webCount;
    }
}
