using BddDotNet.Components.Composition;
using BddDotNet.Playwright.DefaultComponents;

namespace PlaywrightApp.Pages;

internal sealed class CheckoutPage
{
    [Route("continue to checkout")]
    [Options(".btn-primary")]
    public required Button ContinueToCheckout { get; set; }

    [Route("first name")]
    [Options("#firstName")]
    public required Input FirstName { get; set; }

    [Route("last name")]
    [Options("#lastName")]
    public required Input LastName { get; set; }

    [Route("username error message")]
    [Options("#username ~ .invalid-feedback")]
    public required Label UsernameErrorMessage { get; set; }
}
