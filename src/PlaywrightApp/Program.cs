using BddDotNet;
using BddDotNet.Components;
using BddDotNet.CSharpExpressions;
using BddDotNet.Playwright.DefaultComponents;
using Microsoft.Testing.Platform.Builder;
using PlaywrightApp;

var builder = await TestApplication.CreateBuilderAsync(args);

var services = builder.AddBddDotNet();
services.EnableCSharpExpressions<Expressions>();
services.PlaywrightForSinglePageChromium(new() { Headless = false });

services.Component<Button>("checkout > continue to checkout").Options(".btn-primary");
services.Component<Input>("checkout > first name").Options("#firstName");
services.Component<Input>("checkout > last name").Options("#lastName");
services.Component<Label>("checkout > username error message").Options("#username ~ .invalid-feedback");

services.SourceGeneratedGherkinScenarios();
services.SourceGeneratedGherkinSteps();

using var testApp = await builder.BuildAsync();
return await testApp.RunAsync();
