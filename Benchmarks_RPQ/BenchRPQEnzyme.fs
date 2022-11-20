module Benchmarks_RPQ.BenchRPQEnzyme
open System.Collections.Generic
open CFPQ_GLL.GLL
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Jobs
open Benchmarks.Program
open Benchmarks_RPQ.RPQUtils
    
[<SimpleJob (RuntimeMoniker.Net50)>]
type BenchmarksRPQEnzyme() =
   
   [<Params("Q4_4", "Q9_5", "Q10_5")>]
   member val query = "" with get, set
   
   [<Params("1", "100", "500")>]
   member val nodesNumber = "" with get, set
   
   member val graph = loadRPQGraphFromCSV "/home/jblab/projects/eshemetova/Graphs/rdf/enzyme/enzyme.csv" enzymeMap
             
   [<Benchmark>]
   member this.enzyme () =
    let nodes = loadNodesFormCSV $"/home/jblab/projects/eshemetova/RPQ_GLL_F#/RPQ_Data-dev/MS_Reachability/enzyme/src_verts/{this.nodesNumber}.txt"
    let q =
        match this.query with
              |  "Q4_4" -> enzymeQ4_4
              |  "Q9_5" -> enzymeQ9_5
              |  "Q10_5" -> enzymeQ10_5
              |  _ -> enzymeQ4_4
    let multipleSources, _ = this.graph.ToCfpqCoreGraph (HashSet nodes)
    let reachable = eval multipleSources q Mode.ReachabilityOnly
    let reach =
           match reachable with
              | QueryResult.MatchedRanges _ -> q.OriginalStartState.NonTerminalNodes.ToArray().Length
              | QueryResult.ReachabilityFacts x -> x |> Seq.fold (fun x v -> x + v.Value.Count) 0
    printf($"Reachable: %A{reach}")
        
