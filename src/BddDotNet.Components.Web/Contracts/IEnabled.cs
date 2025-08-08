namespace BddDotNet.Components.Web.Contracts;

public interface IEnabled
{
    Task<bool> IsEnabledAsync();
}
