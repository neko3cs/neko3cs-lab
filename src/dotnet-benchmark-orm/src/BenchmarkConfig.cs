using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Toolchains.InProcess.Emit;

namespace DotNetOrmBench;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddJob(Job.Default
            .WithRuntime(CoreRuntime.Core90)
            .WithToolchain(InProcessEmitToolchain.Instance)
            .WithGcServer(true)
            .WithGcConcurrent(true)
            .WithWarmupCount(3)
            .WithIterationCount(10));
        AddDiagnoser(MemoryDiagnoser.Default);

        AddExporter(MarkdownExporter.GitHub);
        AddExporter(HtmlExporter.Default);
        AddExporter(CsvExporter.Default);

        AddColumnProvider(DefaultColumnProviders.Instance);
        AddColumn(StatisticColumn.Min);
        AddColumn(StatisticColumn.Max);
        AddColumn(StatisticColumn.Mean);
        AddColumn(StatisticColumn.Median);
        AddColumn(StatisticColumn.StdDev);
        AddColumn(RankColumn.Arabic);

        WithOrderer(new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest));
    }
}