using BddDotNet.Components.Composition;

namespace PlaywrightApp.Pages;

internal sealed class ApplicationRootPage
{
    [Route("checkout")]
    public required CheckoutPage Checkout { get; set; }
}
