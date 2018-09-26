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
[<DisassemblyDiagnoser(printAsm = true, printIL = true)>]
type Benchs() =
    let a = [| 1L..50000L |]
    [<Benchmark>]
    member __.reduceParallelFSharpAsyncs () = reduceParallelAsync (+) a
    [<Benchmark>]
    member __.reduceParallelFSharpTaskBuilder () = reduceParallelTaskBuilder (+) a
    [<Benchmark>]
    member __.reduceParallelFSharpSync () = reduceFsharpSync (+) a
    [<Benchmark(Baseline = true)>]
    member __.reduceParallelCSharpAsyncs () = Benchmarks.ReduceAsyncAwait(Func<_,_,_>(fun a b-> a+b), a)
    [<Benchmark>]
    member __.reduceParallelTPL () = Benchmarks.ReduceTPL(Func<_,_,_>(fun a b-> a+b), a)
    [<Benchmark>]
    member __.reduceParallelValueTasks () = Benchmarks.ReduceVT(Func<_,_,_>(fun a b-> a+b), a)
    [<Benchmark>]
    member __.reduceParallelValueTasksPattern () = Benchmarks.ReduceVTPattern(Func<_,_,_>(fun a b-> a+b), a)
    [<Benchmark>]
    member __.reduceParallelHopac () = reduceParallelHopac (+) a


[<EntryPoint>]
let main argv =
    let summary = BenchmarkRunner.Run<Benchs>();
    0 // return an integer exit code
