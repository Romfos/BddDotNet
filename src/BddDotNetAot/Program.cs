using BddDotNet;
using BddDotNetAot;
using BddDotNetAot.Transformations;
using Microsoft.Testing.Platform.Builder;

var builder = await TestApplication.CreateBuilderAsync(args);

var services = builder.AddBddDotNet();
services.SourceGeneratedGherkinScenarios();
services.SourceGeneratedGherkinSteps();

services.ArgumentTransformation<DictionaryArgumentTransformation>();

using var testApp = await builder.BuildAsync();
return await testApp.RunAsync();
