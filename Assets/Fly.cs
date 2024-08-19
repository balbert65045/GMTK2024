using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    [SerializeField] int WebAmount = 20;
    public int GetWebAmount() { return WebAmount; }
    bool isEaten = false;
    public void SetIsEatenTrue() { isEaten = true; }
    public bool GetIsEaten() { return isEaten; }
}
