module Benchmarks_RPQ.BenchRPQCore
open Benchmarks_RPQ.RPQUtils
open System.Collections.Generic
open CFPQ_GLL.GLL

open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Jobs
open Benchmarks.Program
    
[<SimpleJob (RuntimeMoniker.Net50)>]
type BenchmarksRPQCore() =
   
   [<Params("Q4_5", "Q5", "Q10_5")>]
   member val query = "" with get, set
   
   [<Params("1", "100", "500")>]
   member val nodesNumber = "" with get, set
   
   member val graph = loadRPQGraphFromCSV "/home/jblab/projects/eshemetova/Graphs/rdf/core/core.csv" coreMap
             
   [<Benchmark>]
   member this.core () =
    let nodes = loadNodesFormCSV $"/home/jblab/projects/eshemetova/RPQ_GLL_F#/RPQ_Data-dev/MS_Reachability/core/src_verts/{this.nodesNumber}.txt"
    let q =
        match this.query with
              |  "Q4_5" -> coreQ4_5
              |  "Q5" -> coreQ5
              |  "Q10_5" -> coreQ10_5
              |  _ -> coreQ4_5
    let multipleSources, _ = this.graph.ToCfpqCoreGraph (HashSet nodes)
    let reachable = eval multipleSources q Mode.ReachabilityOnly
    let reach =
           match reachable with
              | QueryResult.MatchedRanges _ -> q.OriginalStartState.NonTerminalNodes.ToArray().Length
              | QueryResult.ReachabilityFacts x -> x |> Seq.fold (fun x v -> x + v.Value.Count) 0
    printf($"Reachable: %A{reach}")
        
