
open System.Collections.Generic
open CFPQ_GLL
open CFPQ_GLL.Common
open CFPQ_GLL.GLL
open CFPQ_GLL.InputGraph
open CFPQ_GLL.RSM
open Tests.InputGraph
open CFPQ_GLL.SPPF
open Expecto
open Tests.Benchmark



let config = {FsCheckConfig.defaultConfig with maxTest = 10000}

  
let go() =
    let graph = InputGraph([|TerminalEdge(0<inputGraphVertex>, 0<terminalSymbol>, 0<inputGraphVertex>)
                             TerminalEdge(0<inputGraphVertex>, 1<terminalSymbol>, 0<inputGraphVertex>)                             
                           |])
    let startV = [|0<inputGraphVertex>|]
    let q = Tests.GLLTests.simpleLoopRSMForDyckLanguage ()
    
    let startVertices,_ = graph.ToCfpqCoreGraph (HashSet startV)
    let result = GLL.eval startVertices q GLL.AllPaths

    match result with
    | QueryResult.MatchedRanges ranges -> 
      let sppf = q.OriginalStartState.NonTerminalNodes.ToArray()
      let actual = TriplesStoredSPPF (sppf, Dictionary())
      
      actual.ToDot "1.dot"
      
      Tests.GLLTests.dumpResultToConsole actual
    | _ -> failwith "Unexpected result." 
    0
 
[<EntryPoint>]
let main argv =
    //Tests.runTestsWithCLIArgs [] [||] (testList "debug tests" [Tests.GLLTests.``Two loops with common vertex, simple loop RSM for Dyck language``])
    //--graph /home/cyfra/CFPQ_GLL/Graphs/java_data/graphs/batik/data.csv --mode all-paths --tasktype all-pairs --query java
    //BenchmarkRunner.Run<Benchmarks>() |> ignore
    //0
   
    Tests.runTestsWithCLIArgs [] [||] (testList "all tests" [Tests.GLLTests.tests])
    
    //Tests.runTestsWithCLIArgs [] [||] (testList "all tests" [Tests.DistancesTests.tests])
    //go ()
        
    