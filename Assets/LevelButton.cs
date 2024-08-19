using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public void LoadLevel(string name)
    {
        LevelManager.Instance.LoadLevel(name);
    }

    public void ReloadLevel()
    {
        LevelManager.Instance.ReplayLevel();
    }
}
