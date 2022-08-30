namespace PriceTracker.API.Endpoints;

/// <summary>
/// Maps delegates to specified pattern.
/// <remarks>Only singleton dependency injection should be used at the constructor.</remarks>
/// </summary>
public interface IEndpoint
{
    Delegate? Post => null;
    Delegate? Get => null;
    Delegate? Put => null;
    Delegate? Delete => null;
}
