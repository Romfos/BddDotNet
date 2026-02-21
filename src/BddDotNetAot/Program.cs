using BddDotNet;
using BddDotNet.Arguments;
using BddDotNet.Gherkin.Models;
using BddDotNetAot.Models;
using BddDotNetAot.Transformations;
using Microsoft.Testing.Platform.Builder;

var builder = await TestApplication.CreateBuilderAsync(args);

var services = builder.AddBddDotNet();
services.SourceGeneratedGherkinScenarios();
services.SourceGeneratedGherkinSteps();

services.ModelTransformation<Model1>();
services.ArgumentTransformation<DictionaryTransformation>();

using var testApp = await builder.BuildAsync();
return await testApp.RunAsync();
