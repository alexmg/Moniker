using BenchmarkDotNet.Attributes;

namespace Moniker.PerformanceTests;

public class NameGeneratorBenchmark
{
    [Benchmark]
    public string GenerateMoby() => NameGenerator.Generate(MonikerStyle.Moby);

    [Benchmark]
    public string GenerateMoniker() => NameGenerator.Generate(MonikerStyle.Moniker);

    [Benchmark]
    public void GenerateMobyPair() => NameGenerator.Generate(MonikerStyle.Moby, out _, out _);

    [Benchmark]
    public void GenerateMonikerPair() => NameGenerator.Generate(MonikerStyle.Moniker, out _, out _);
}