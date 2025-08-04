# Overview

Modern opensource BDD framework for C# and .NET with gherkin support

[![.github/workflows/build.yml](https://github.com/Romfos/BddDotNet/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/Romfos/BddDotNet/actions/workflows/build.yml)

[![BddDotNet](https://img.shields.io/nuget/v/BddDotNet?label=BddDotNet)](https://www.nuget.org/packages/BddDotNet)
[![BddDotNet.Gherkin.SourceGenerator](https://img.shields.io/nuget/v/BddDotNet.Gherkin.SourceGenerator?label=BddDotNet.Gherkin.SourceGenerator)](https://www.nuget.org/packages/BddDotNet.Gherkin.SourceGenerator)

# Requirements
- .NET 8 SDK or never (required for build phase)
- .NET 8+ or .NET Framework 4.7.2+ for target execution runtime (We recommend .NET 9 as a default option)
- Visual Studio 2022 or Visual Studio Code
- [Reqnroll plugin](https://marketplace.visualstudio.com/items?itemName=Reqnroll.ReqnrollForVisualStudio2022) for Visual Studio 2022 or [Cucumber plugin](https://marketplace.visualstudio.com/items?itemName=CucumberOpen.cucumber-official) for Visual Studio Code

# Nuget packages links  
- https://www.nuget.org/packages/BddDotNet
- https://www.nuget.org/packages/BddDotNet.Gherkin.SourceGenerator

# Key points

Comparing with Reqnroll (or Specflow, other popular framework in .NET Ecosystem) this framework has following difference:
-	Microsoft.Extensions.* based
-	Microsoft testing platform as a backend. No hell with different unit tests providers.
-	Source generator for features compilation & step registration
-	Code first approach with builder pattern
-	Extensibility via public interfaces and DI
- Modular. Small and fast library. All extra features are provided as separate nuget packages
-	No reflection usage (or quite limited)
-	Support .NET 8+ (.NET 9 is a recommended option) and .NET Framework 4.7.2+ 
-	AOT & Trimming friendly. More info: [Native AOT deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)
-	Nullable reference types and other modern dotnet features support

# Gherkin syntax example
```gherkin
Feature: Guess the word

  # The first example has two steps
  Scenario: Maker starts a game
    When the Maker starts a game
    Then the Maker waits for a Breaker to join

  # The second example has three steps
  Scenario: Breaker joins a game
    Given the Maker has started a game with the word "silky"
    When the Breaker joins the Maker's game
    Then the Breaker must guess a word with 5 characters
```

# CSharp syntax scenario example

Program.cs content:
```csharp
using BddDotNet;
using Microsoft.Testing.Platform.Builder;

var builder = await TestApplication.CreateBuilderAsync(args);
var services = builder.AddBddDotNet();

services.Scenario<Program>("feature1", "scenario1", async context =>
{
    await context.Given("this is given step");
    await context.When("this is when step");
    await context.Then("this is then step");
});

services.Given(new("this is given step"), () =>
{
    Console.WriteLine("This is the given step.");
});

services.When(new("this is when step"), () =>
{
    Console.WriteLine("This is the when step.");
});

services.Then(new("this is then step"), () =>
{
    Console.WriteLine("This is the then step.");
});

using var testApp = await builder.BuildAsync();
return await testApp.RunAsync();

```
# How to use
1) Create new console application (In this example we will use .NET 9  with gherkin syntax)
2) Install required nuget packages
- https://www.nuget.org/packages/BddDotNet
- https://www.nuget.org/packages/BddDotNet.Gherkin.SourceGenerator
4) Configure test application in Program.cs
```csharp
using DemoAppNamespace;
using BddDotNetAot;
using Microsoft.Testing.Platform.Builder;

var builder = await TestApplication.CreateBuilderAsync(args);

var services = builder.AddBddDotNet();
services.SourceGeneratedGherkinScenarios();
services.SourceGeneratedGherkinSteps();

using var testApp = await builder.BuildAsync();
return await testApp.RunAsync();
```
5) Add .*feature files and Step classes. For example:

DemoApp.Feature:
```gherkin
@feature-tag
Feature: DemoApp feature

A short summary of the feature

@scenario-tag-1
Scenario: simple steps
    Given this is simple given step
    When this is simple when step
    Then this is simple then step
```
Steps.cs
```csharp
using BddDotNet.Gherkin;

namespace BddDotNetAot.Steps;

internal sealed class Steps1
{
    [Given("this is simple given step")]
    public void Step1()
    {
    }

    [When("this is simple when step")]
    public void Step2()
    {
    }

    [Then("this is simple then step")]
    public void Step3()
    {
    }
}
```


  
