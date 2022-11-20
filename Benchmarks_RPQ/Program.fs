module Benchmarks_RPQ.Program

open Benchmarks_RPQ.BenchRPQCore
open Benchmarks_RPQ.BenchRPQEclass
open Benchmarks_RPQ.BenchRPQEnzyme
open Benchmarks_RPQ.BenchRPQGo
open BenchmarkDotNet.Running
                                             
[<EntryPoint>]
let benchIt  argv =
    BenchmarkRunner.Run<BenchmarksRPQCore>() |> ignore
    BenchmarkRunner.Run<BenchmarksRPQEclass>() |> ignore
    BenchmarkRunner.Run<BenchmarksRPQEnzyme>() |> ignore
    BenchmarkRunner.Run<BenchmarksRPQGo>() |> ignore
    0