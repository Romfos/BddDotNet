
# Overview

Modern BDD framework for c# and .NET with gherkin support

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

services.Scenario<Program>("feature1", "scenario1", async scenario =>
{
    await scenario.Given("this is given step");
    await scenario.When("this is when step");
    await scenario.Then("this is then step");
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

More info: https://github.com/Romfos/BddDotNet
