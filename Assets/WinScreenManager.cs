using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenManager : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    
    public void ShowWinScreen()
    {
        Canvas.SetActive(true);
    }
}
