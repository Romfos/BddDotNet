namespace BddDotNet.Extensibility;

public interface ITestContext
{
    string Feature { get; }
    string Scenario { get; }
}
