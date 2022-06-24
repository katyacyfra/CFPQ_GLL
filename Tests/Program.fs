﻿
open CFPQ_GLL
open CFPQ_GLL.GLL
open CFPQ_GLL.InputGraph
open Tests.InputGraph
open CFPQ_GLL.SPPF
open Expecto


let config = {FsCheckConfig.defaultConfig with maxTest = 10000}

  
let go() =
    let graph = InputGraph([|TerminalEdge(0<graphVertex>, 0<terminalSymbol>, 0<graphVertex>)
                             TerminalEdge(0<graphVertex>, 1<terminalSymbol>, 0<graphVertex>)                             
                           |])
    let startV = [|0<graphVertex>|]
    let q = Tests.GLLTests.simpleLoopRSMForDyckLanguage

    let result = GLL.eval graph startV q GLL.AllPaths

    match result with
    | QueryResult.MatchedRanges ranges -> 
      let sppf = ranges.ToSPPF startV
      let actual = TriplesStoredSPPF sppf
      
      actual.ToDot "1.dot"
      
      //GLLTests.dumpResultToConsole actual
    | _ -> failwith "Unexpected result." 
    0
 
[<EntryPoint>]
let main argv =    
    Tests.runTestsWithCLIArgs [] [||] (testList "all tests" [Tests.GLLTests.tests])
    //go ()
        
    