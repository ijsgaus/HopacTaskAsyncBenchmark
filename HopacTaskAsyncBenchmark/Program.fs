open Hopac
open Hopac.Infixes
open System.Threading.Tasks
open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open CSharpBencmarks
open FSharpBenchmarks

[<SimpleJob(launchCount = 3, warmupCount = 3, targetCount = 5)>]
[<GcServer(true)>]
[<MemoryDiagnoser>]
[<MarkdownExporterAttribute.GitHub>]
type Benchs() =
    let a = [| 1L..50000L |]
    [<Benchmark>]
    member __.reduceParallelAsyncsBench () = reduceParallelAsync (+) a    
    [<Benchmark(Baseline = true)>]
    member __.reduceParallelTasksBench () = Benchmarks.ReduceParallelTasks(Func<_,_,_>(fun a b-> a+b), a)
    [<Benchmark>]
    member __.reduceParallelHopacBech () = reduceParallelHopac (+) a
    

[<EntryPoint>]
let main argv =
    let summary = BenchmarkRunner.Run<Benchs>();
    0 // return an integer exit code
