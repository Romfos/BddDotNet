using BddDotNet.Steps;
using System.Text.RegularExpressions;

namespace BddDotNet.Internal.Models;

internal sealed record Step(
    StepType StepType,
    Regex Pattern,
    Func<IServiceProvider, Delegate> HandlerFactory);