namespace BddDotNet.Tests.Services;

public sealed record TraceService
{
    public bool Step1 { get; set; }
    public bool Step2 { get; set; }
    public bool Step3 { get; set; }
    public string? Step4 { get; set; }
    public string[][]? Step5 { get; set; }
    public bool Step6 { get; set; }
    public bool Step7 { get; set; }
    public bool Step8 { get; set; }
}
