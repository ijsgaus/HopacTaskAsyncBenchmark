module FSharpBenchmarks

open Hopac
open Hopac.Infixes
open System.Threading.Tasks
open System

let reduceParallelAsync<'a> f (ie: 'a array) = 
    let rec reduceRec f (start, finish) =
        let reduce = reduceRec f
        async {
            match (finish - start + 1) with
            | 1 -> return ie.[start]
            | 2 -> return f ie.[start] ie.[start+1]
            | len -> 
                let h = len / 2 + start
                let! o1a = reduce(start, h-1) |> Async.StartChild
                let! o2 = reduce(h, finish)
                let! o1 = o1a
                return f o1 o2
        }
    match ie.Length with
    | 0 -> failwith "Sequence contains no elements"
    | _ -> Async.RunSynchronously <| reduceRec f (0, ie.Length-1)

let reduceParallelHopac<'a> f (a: 'a array) = 
    let rec reduceRec f (start, finish) =
        let reduce = reduceRec f 
        match (finish - start + 1) with
        | 1 -> Job.result a.[start]
        | 2 -> Job.result (f a.[start] a.[start+1])
        | len -> 
            let h = len / 2 + start
            reduce (start, h-1) <*> Job.delayWith reduce (h, finish) >>- fun (x, y) -> f x y
    match a.Length with
    | 0 -> failwith "Sequence contains no elements"
    | _ -> run <| reduceRec f (0, a.Length-1)