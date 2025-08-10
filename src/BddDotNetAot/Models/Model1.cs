namespace BddDotNetAot.Models;

#pragma warning disable CA1051 // Do not declare visible instance fields

public class Model1(int first)
{
    public int first = first;
    public decimal? third;
    public string? Second { get; set; }
}
