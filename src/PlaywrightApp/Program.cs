using BddDotNet;
using BddDotNet.Components.Composition;
using BddDotNet.Gherkin.CSharpExpressions;
using BddDotNet.Playwright.DefaultComponents;
using Microsoft.Testing.Platform.Builder;
using PlaywrightApp;
using PlaywrightApp.Pages;

var builder = await TestApplication.CreateBuilderAsync(args);

var services = builder.AddBddDotNet();

services.CSharpExpressions<Expressions>();
services.SinglePageChromiumPlaywright(new() { Headless = false });

services.CollectComponentsAndOptions<ApplicationRootPage>();

services.SourceGeneratedGherkinScenarios();
services.SourceGeneratedGherkinSteps();

using var testApp = await builder.BuildAsync();
return await testApp.RunAsync();
