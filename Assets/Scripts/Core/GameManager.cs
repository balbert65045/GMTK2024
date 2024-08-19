using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameMode Mode { get; private set; }
    public UnityEvent<GameMode> GameModeChangedEvent;

    public bool Paused;
    public void SetPaused(bool value) { 
        Paused = value;
    }
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
        if (GameModeChangedEvent == null) GameModeChangedEvent = new UnityEvent<GameMode>();

        Mode = GameMode.PLATFORM_MODE;
        GameModeChangedEvent?.Invoke(Mode);
    }

    public void ToggleGameMode()
    {
        if (Mode == GameMode.BUILD_MODE)
        {
            Mode = GameMode.PLATFORM_MODE;
        }
        else
        {
            Mode = GameMode.BUILD_MODE;
        }
        GameModeChangedEvent?.Invoke(Mode);
    }
}

public enum GameMode
{
    BUILD_MODE,
    PLATFORM_MODE
}
