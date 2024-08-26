using BenchmarkDotNet.Attributes;

namespace Moniker.PerformanceTests;

public class NameGeneratorBenchmark
{
    [Benchmark]
    public string GenerateMoby() => NameGenerator.Generate(MonikerStyle.Moby);

    [Benchmark]
    public string GenerateMoniker() => NameGenerator.Generate(MonikerStyle.Moniker);
}