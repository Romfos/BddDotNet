namespace BddDotNet.Components.Web;

public interface IEnabled
{
    Task<bool> IsEnabledAsync();
}
