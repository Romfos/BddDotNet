namespace BddDotNet.Components.WebContracts;

public interface IEnabled
{
    Task<bool> IsEnabledAsync();
}
