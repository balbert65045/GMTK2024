using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public GodPoint currentGodPointTouching;
    public void SetGodPoint(GodPoint point) { currentGodPointTouching = point; }
}
