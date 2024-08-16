using UnityEngine;
using UnityEngine.Events;

public class WebResourceController : MonoBehaviour
{
    public static WebResourceController Instance;

    [SerializeField] int defaultWebCount = 100;

    private WebResource webResource;

    public UnityEvent<int> WebResourceChangedEvent;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        if (WebResourceChangedEvent == null) WebResourceChangedEvent = new UnityEvent<int>();

        webResource = new WebResource(defaultWebCount);
        WebResourceChangedEvent?.Invoke(GetWebCount());
    }

    private void OnDestroy()
    {
        WebResourceChangedEvent.RemoveAllListeners();
    }

    public void IncrementWebCount(int count)
    {
        webResource.IncrementWebCount(count);
        WebResourceChangedEvent?.Invoke(GetWebCount());
    }

    public void DecrementWebCount(int count)
    {
        webResource.DecrementWebCount(count);
        WebResourceChangedEvent?.Invoke(GetWebCount());
    }

    public void SetWebCount(int count)
    {
        webResource.SetWebCount(count);
        WebResourceChangedEvent?.Invoke(GetWebCount());
    }

    public int GetWebCount()
    {
        return webResource.WebCount;
    }
}
