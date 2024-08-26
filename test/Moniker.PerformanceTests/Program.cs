using BenchmarkDotNet.Running;
using Moniker.PerformanceTests;

new BenchmarkSwitcher(Benchmarks.All).Run(args, new BenchmarkConfig());