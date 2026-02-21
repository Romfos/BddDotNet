namespace BddDotNet.Configuration;

public sealed class BddDotNetConfiguration
{
    public bool Parallel { get; set; } = false;

    public int MaxDegreeOfParallelism { get; set; } = Environment.ProcessorCount;
}
