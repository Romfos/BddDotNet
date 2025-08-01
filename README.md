# Overview

Modern BDD framework for c# and .NET with gherkin support

[![.github/workflows/build.yml](https://github.com/Romfos/BddDotNet/actions/workflows/build.yml/badge.svg?branch=main)](https://github.com/Romfos/BddDotNet/actions/workflows/build.yml)

# Requirements
- .NET 8+ or .NET Framework 4.7.2+. We recommend .NET 9 as a default option
- Visual Studio 2022 or Visual Studio Code
- [Reqnroll plugin](https://marketplace.visualstudio.com/items?itemName=Reqnroll.ReqnrollForVisualStudio2022) for Visual Studio 2022 or [Cucumber plugin](https://marketplace.visualstudio.com/items?itemName=CucumberOpen.cucumber-official) for Visual Studio Code

# Nuget packages links  
- https://www.nuget.org/packages/BddDotNet
- https://www.nuget.org/packages/BddDotNet.Gherkin.SourceGenerator

# Benefits

Comparing with Reqnroll (or Specflow, other popular framework in .NET Ecosystem) this framework has following benefits:
-	Code first approach with builder pattern
-	Extensibility via public interfaces and DI
- Modular. Small and fast library. All extra features are provided as separate nuget packages
-	No reflection usage (or quite limited)
-	Support .NET 8+ (Recommended option) and .NET Framework 4.7.2+ 
-	AOT & Trimming friendly. More info: [Native AOT deployment](https://learn.microsoft.com/en-us/dotnet/core/deploying/native-aot/)
-	Nullable reference types and other modern dotnet features
-	Microsoft testing platform as a backend. No hell with different unit tests providers.
-	Microsoft.Extensions.* based
-	Source generator for features compilation & step registration
