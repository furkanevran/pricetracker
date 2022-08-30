namespace PriceTracker.API.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class PatternAttribute : Attribute
{
    public string Pattern { get; }

    public PatternAttribute(string pattern)
    {
        Pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
    }
}