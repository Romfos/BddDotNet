using BddDotNet;
using BddDotNet.Components.Browser;
using BddDotNet.Components.Routing;
using BddDotNet.Playwright.DefaultComponents;
using Microsoft.Testing.Platform.Builder;
using PlaywrightApp;

var builder = await TestApplication.CreateBuilderAsync(args);

var services = builder.AddBddDotNet();

services.SourceGeneratedGherkinScenarios();
services.SourceGeneratedGherkinSteps();

services.ComponentRoutingSystem();
services.EnableBrowserContracts();

services.SinglePageGoogleChromePlaywright(new() { Headless = false });

services.Component<Button>("checkout > continue to checkout").Options(".btn-primary");

using var testApp = await builder.BuildAsync();
return await testApp.RunAsync();
