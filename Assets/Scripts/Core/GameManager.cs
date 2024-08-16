using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameMode Mode { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Mode = GameMode.BUILD_MODE;
    }

    public void ToggleGameMode()
    {
        if (Mode == GameMode.BUILD_MODE)
            Mode = GameMode.PLATFORM_MODE;
        else
            Mode = GameMode.BUILD_MODE;
    }
}

public enum GameMode
{
    BUILD_MODE,
    PLATFORM_MODE
}
