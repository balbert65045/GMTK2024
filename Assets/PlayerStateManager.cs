using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    public GodPoint currentGodPointTouching;
    public GridVisualizer currentGridOn;
    public void SetGodPoint(GodPoint point) { currentGodPointTouching = point; }
    public void SetCurrentGridOn(GridVisualizer grid) { currentGridOn = grid; }
}
