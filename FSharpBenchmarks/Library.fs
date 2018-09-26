module FSharpBenchmarks

open Hopac
open Hopac.Infixes
open System.Threading.Tasks
open System
open FSharp.Control.Tasks.V2.ContextInsensitive

let reduceParallelAsync<'a> f (ie: 'a array) =
    let rec reduce (ie : 'a array, f, start, finish) =
        async {
            match (finish - start + 1) with
            | 1 -> 
                return ie.[start]
            | 2 -> 
                return f ie.[start] ie.[start+1]
            | len ->
                let h = len / 2 + start
                let! o1 = reduce(ie, f, start, h-1)
                let! o2 = reduce(ie, f, h, finish)
                return f o1 o2
        }
    match ie.Length with
    | 0 -> failwith "Sequence contains no elements"
    | _ -> (reduce (ie, f, 0, ie.Length-1)) |> Async.RunSynchronously

let reduceParallelTaskBuilder<'a> f (ie: 'a array) =
    let rec reduce (ie : 'a array, f, start, finish) =
        task {
            match (finish - start + 1) with
            | 1 -> 
                return ie.[start]
            | 2 -> 
                return f ie.[start] ie.[start+1]
            | len ->
                let h = len / 2 + start
                let! o1 = reduce(ie, f, start, h-1)
                let! o2 = reduce(ie, f, h, finish)
                return f o1 o2
        }
    match ie.Length with
    | 0 -> failwith "Sequence contains no elements"
    | _ -> (reduce (ie, f, 0, ie.Length-1)).Result

let reduceFsharpSync<'a> f (ie: 'a array) =
    let rec reduce (ie : 'a array, f, start, finish) =
        match (finish - start + 1) with
        | 1 -> ie.[start]
        | 2 -> f ie.[start] ie.[start+1]
        | len ->
            let h = len / 2 + start
            let o1 = reduce(ie, f, start, h-1)
            let o2 = reduce(ie, f, h, finish)
            f o1 o2
    match ie.Length with
    | 0 -> failwith "Sequence contains no elements"
    | _ -> reduce (ie, f, 0, ie.Length-1)

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