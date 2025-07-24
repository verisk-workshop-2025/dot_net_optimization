using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using SalesAnalyzer.Lib.Services;
using System.Text;

namespace SalesAnalyzer.Benchmarks
{
    [SimpleJob(runtimeMoniker: RuntimeMoniker.Net80)]
    [MemoryDiagnoser]
    public class SalesProcessorBenchmarks
    {
        //SalesProcessor processor;

        //[GlobalSetup]
        //public void Setup()
        //{
        //    processor = new SalesProcessor();

        //    processor.Initialize();
        //    //processor.InitializeV2();
        //    //processor.InitializeV3();
        //}

        [Benchmark]
        public string Initialize()
        {
            var processor = new SalesProcessor();

            processor.Initialize();

            return "Initialize";
        }


        //[Benchmark]
        //public string InitializeV2()
        //{
        //    var processor = new SalesProcessor();

        //    processor.InitializeV2();

        //    return "InitializeV2";
        //}

        //[Benchmark]
        //public string InitializeV3()
        //{
        //    var processor = new SalesProcessor();

        //    processor.InitializeV3();

        //    return "InitializeV3";
        //}


        //[Benchmark]
        //public string Process()
        //{
        //    var sb = new StringBuilder();
        //    sb.AppendLine("Process: -------------------");

        //    foreach (var branch in processor.Branches)
        //    {
        //        var highestSellingCountItem = processor.FindHighestSellingCountItem(branch.Key);
        //        var lowestSellingCountItem = processor.FindLowestSellingCountItem(branch.Key);
        //        sb.AppendLine($"Branch: {branch}, HSC - {highestSellingCountItem}, LSC - {lowestSellingCountItem}");
        //    }

        //    return sb.ToString();
        //}

        //[Benchmark]
        //public string ProcessV2()
        //{
        //    var sb = new StringBuilder();
        //    sb.AppendLine("Process: -------------------");

        //    foreach (var branch in processor.Branches)
        //    {
        //        var (highestSellingCountItem, lowestSellingCountItem) = processor.GetReportsForHighestAndLowestSellingCount(branch.Key);
        //        sb.AppendLine($"Branch: {branch}, HSC - {highestSellingCountItem}, LSC - {lowestSellingCountItem}");
        //    }

        //    return sb.ToString();
        //}
    }
}
