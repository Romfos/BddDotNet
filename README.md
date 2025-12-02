# Overview

Modern opensource BDD framework for C# and .NET with gherkin support

[![.github/workflows/build.yml](https://github.com/Romfos/BddDotNet/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/Romfos/BddDotNet/actions/workflows/build.yml)

| Description                                                    | Package                                                                                                                                                                                                                    |
|----------------------------------------------------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| Modern opensource BDD framework for C# and .NET                | [![BddDotNet](https://img.shields.io/nuget/v/BddDotNet?label=BddDotNet)](https://www.nuget.org/packages/BddDotNet)                                                                                                         |
| Add support for Gherkin language. Source generator.            | [![BddDotNet.Gherkin.SourceGenerator](https://img.shields.io/nuget/v/BddDotNet.Gherkin.SourceGenerator?label=BddDotNet.Gherkin.SourceGenerator)](https://www.nuget.org/packages/BddDotNet.Gherkin.SourceGenerator)         |
| Add support for C# expressions in Gherkin step arguments       | [![BddDotNet.Gherkin.CSharpExpressions](https://img.shields.io/nuget/v/BddDotNet.Gherkin.CSharpExpressions?label=BddDotNet.Gherkin.CSharpExpressions)](https://www.nuget.org/packages/BddDotNet.Gherkin.CSharpExpressions) |
| Add argument transformations for Gherkin tables into C# models | [![BddDotNet.Gherkin.Models](https://img.shields.io/nuget/v/BddDotNet.Gherkin.Models?label=BddDotNet.Gherkin.Models)](https://www.nuget.org/packages/BddDotNet.Gherkin.Models)                                             |

# How to use

Wiki: [Setup .NET 10 project with Gherkin support](https://github.com/Romfos/BddDotNet/wiki/Setup-.NET-10-project-with-Gherkin-support)

# Comparing with alternatives

Comparing with Reqnroll (or Specflow, other popular framework in .NET Ecosystem) this framework has following difference:
- Microsoft.Extensions.* based
Microsoft testing platform as a backend. No hell with different unit tests providers as it was before in Specflow/Reqnroll.
- Source generator for features compilation & step registration
- Code first approach with builder pattern
- Extensibility via public interfaces and DI
- Modular. Small and fast library. All extra features are provided as separate nuget packages
- No or limited reflection usage. Most of the code is totally reflection free.
- Support .NET 8+  and .NET Framework 4.7.2+ runtimes(we recommend to use .NET 10 as best option if possible)
- AOT & Trimming friendly. More info: [Native AOT deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)
- Nullable reference types and other modern dotnet features support

# BddDotNet.Gherkin.SourceGenerator usage example
Allow you to use Gherkin scenarios:

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

```csharp
internal sealed class SampleSteps
{
    [When("the Maker starts a game")]
    public void Sample2()
    {
        Console.WriteLine("the Maker starts a game");
    }

    [Then("the Maker waits for a Breaker to join")]
    public void Sample3()
    {
        Console.WriteLine("the Maker waits for a Breaker to join");
    }
}

```

# BddDotNet usage example
Alllow you to use builder syntax for building steps & scenarios

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


  
