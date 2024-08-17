public class WebResource
{
    public int WebCount { get; private set; }

    public WebResource(int defaultWebCount)
    {
        WebCount = defaultWebCount;
    }

    public void IncrementWebCount(int count)
    {
        WebCount += count;
    }

    public void DecrementWebCount(int count)
    {
        WebCount -= count;
    }

    public void SetWebCount(int count)
    {
        WebCount = count;
    }
}
