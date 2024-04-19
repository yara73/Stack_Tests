using System.Globalization;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using Perfolizer.Horology;

namespace Stack.Benchmark.Benchmark.Config;

public class BenchmarkConfig : ManualConfig
{
    public BenchmarkConfig()
    {
        AddColumnProvider(DefaultColumnProviders.Instance);
        AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
        AddExporter(DefaultConfig.Instance.GetExporters().ToArray());
        AddDiagnoser(DefaultConfig.Instance.GetDiagnosers().ToArray());
        AddAnalyser(DefaultConfig.Instance.GetAnalysers().ToArray());
        AddJob(DefaultConfig.Instance.GetJobs().ToArray());

        var summaryStyle = new SummaryStyle(
            CultureInfo.CurrentCulture, 
            printUnitsInHeader: true, 
            sizeUnit: SizeUnit.KB, 
            timeUnit: TimeUnit.Microsecond, 
            printUnitsInContent: false);


        WithSummaryStyle(summaryStyle);
        Orderer = new DefaultOrderer(SummaryOrderPolicy.FastestToSlowest);
    }
}