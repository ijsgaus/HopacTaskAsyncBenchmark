open Hopac
open Hopac.Infixes
open System.Threading.Tasks
open System
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


let reduceParallelTasks<'a> f (ie: 'a array) = 
        let rec reduceRec f (ie: 'a array) = 
            match ie.Length with
            | 1 -> ie.[0]
            | 2 -> f ie.[0] ie.[1]
            | len -> 
                let h = len / 2
                let o1 = Task.Run(fun _ -> reduceRec f ie.[0..h - 1])
                let o2 = Task.Run(fun _ -> reduceRec f ie.[h..])
                f o1.Result o2.Result
        match ie.Length with
        | 0 -> failwith "Sequence contains no elements"
        | _ -> reduceRec f ie



let reduceParallelAsync<'a> f (ie: 'a array) = 
    let rec reduceRec f (ie: 'a array) = 
        async { 
            match ie.Length with
            | 1 -> return ie.[0]
            | 2 -> return f ie.[0] ie.[1]
            | len -> 
                let h = len / 2
                let! o1a = Async.StartChild <| reduceRec f ie.[0..h - 1]
                let! o2 = reduceRec f ie.[h..]
                let! o1 = o1a
                return f o1 o2
        }
    match ie.Length with
    | 0 -> failwith "Sequence contains no elements"
    | _ -> Async.RunSynchronously <| reduceRec f ie



let reduceParallelHopac<'a> f (a: 'a array) = 
    let rec reduceRec (f, ie: 'a array) = 
        match ie.Length with
        | 1 -> Job.result ie.[0]
        | 2 -> Job.result (f ie.[0] ie.[1])
        | len -> 
            let h = len / 2
            reduceRec (f, ie.[0..h - 1]) <*> Job.delayWith reduceRec (f, ie.[h..]) >>- fun (x, y) -> f x y
    match a.Length with
    | 0 -> failwith "Sequence contains no elements"
    | _ -> run <| reduceRec (f, a)

[<SimpleJob(launchCount = 3, warmupCount = 3, targetCount = 5)>]
[<GcServer(true)>]
[<MemoryDiagnoser>]
[<MarkdownExporterAttribute.GitHub>]
type Benchs() =
    let a = [| 1L..50000L |]
    [<Benchmark>]
    member __.reduceParallelAsyncsBench () = reduceParallelAsync (+) a    
    [<Benchmark(Baseline = true)>]
    member __.reduceParallelTasksBench () = reduceParallelTasks (+) a
    [<Benchmark>]
    member __.reduceParallelHopacBech () = reduceParallelHopac (+) a


     

[<EntryPoint>]
let main argv =
    let summary = BenchmarkRunner.Run<Benchs>();
    0 // return an integer exit code
