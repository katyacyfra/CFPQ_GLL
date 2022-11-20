module Benchmarks_RPQ.RPQUtils
open System.Collections.Generic
open Tests
open Tests.InputGraph
open CFPQ_GLL.RSM
open CFPQ_GLL.Common


let loadRPQGraphFromCSV file (callLabelsMappings:Dictionary<_,_>) =
    let edges = ResizeArray<_>()
    System.IO.File.ReadLines file
    |> Seq.map (fun s -> s.Split " ")
    |> Seq.iter (fun a ->
        if callLabelsMappings.ContainsKey a.[2]
        then
            edges.Add (Tests.InputGraph.TerminalEdge (a.[0] |> int |> LanguagePrimitives.Int32WithMeasure
                                               , callLabelsMappings.[a.[2]] |> fst |> LanguagePrimitives.Int32WithMeasure
                                               , a.[1] |> int |> LanguagePrimitives.Int32WithMeasure))
        else edges.Add (Tests.InputGraph.TerminalEdge (a.[0] |> int |> LanguagePrimitives.Int32WithMeasure,
                                                 10000<terminalSymbol>, //sucks
                                                 a.[1] |> int |> LanguagePrimitives.Int32WithMeasure))
            )
    InputGraph <| edges.ToArray()

let coreMap =
    let res = Dictionary<_,_>()
    res.Add("first",(0,10))
    res.Add("subClassOf",(1,11))
    res.Add("type",(2,12))
    res.Add("domain",(3,13))
    res.Add("isDefinedBy",(4,14))
    res.Add("rest",(5,15))
    res.Add("label",(6,16))
    res.Add("range",(7,17))
    res.Add("comment",(8,18))
    res.Add("seeAlso",(9,19))
    res
let coreQ4_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([0<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,1<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,2<terminalSymbol>,0<rsmState>)  
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,0<rsmState>)|])
    RSM([|box|], box)
    
let coreQ4_5v2 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([1<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,1<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,2<terminalSymbol>,1<rsmState>)  
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,0<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,1<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,2<terminalSymbol>,1<rsmState>)  
                  TerminalEdge(1<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,4<terminalSymbol>,1<rsmState>)|])
    RSM([|box|], box)
    
let coreQ5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([2<rsmState>]),
                [|TerminalEdge(0<rsmState>,2<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,2<terminalSymbol>,2<rsmState>)|])
    RSM([|box|], box)
    
let coreQ10_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([1<rsmState>]),
                [|TerminalEdge(0<rsmState>,2<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,5<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,6<terminalSymbol>,1<rsmState>)  
                  TerminalEdge(0<rsmState>,7<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,8<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,9<terminalSymbol>,1<rsmState>)|])
    RSM([|box|], box)
    
let eclassMap =
    let res = Dictionary<_,_>()
    res.Add("type",(0,10))
    res.Add("subPropertyOf",(1,11))
    res.Add("comment",(2,12))
    res.Add("subClassOf",(3,13))
    res.Add("creator",(4,14))
    res.Add("label",(5,15))
    res.Add("domain",(6,16))
    res.Add("range",(7,17))
    res.Add("imports",(8,18))
    res
let eclassQ4_4 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([0<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,1<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,2<terminalSymbol>,0<rsmState>)  
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,0<rsmState>)|])
    RSM([|box|], box)
     
let eclassQ9_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([1<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,2<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,5<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,0<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,2<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,5<terminalSymbol>,1<rsmState>)|])
    RSM([|box|], box)
    
let eclassQ10_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([1<rsmState>]),
                [|TerminalEdge(0<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,5<terminalSymbol>,1<rsmState>)  
                  TerminalEdge(0<rsmState>,6<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,7<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,8<terminalSymbol>,1<rsmState>)|])
    RSM([|box|], box)

let enzymeMap =
    let res = Dictionary<_,_>()
    res.Add("broaderTransitive",(0,10))
    res.Add("altLabel",(1,11))
    res.Add("type",(2,12))
    res.Add("subClassOf",(3,13))
    res.Add("label",(4,14))
    res.Add("comment",(5,15))
    res
let enzymeQ4_4 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([0<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,1<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,2<terminalSymbol>,0<rsmState>)  
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,0<rsmState>)|])
    RSM([|box|], box)
     
let enzymeQ9_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([1<rsmState>]),
                [|TerminalEdge(0<rsmState>,1<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,2<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,5<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,1<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,2<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,5<terminalSymbol>,1<rsmState>)|])
    RSM([|box|], box)
    
let enzymeQ10_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([1<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,1<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,2<terminalSymbol>,1<rsmState>)  
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,5<terminalSymbol>,1<rsmState>)|])
    RSM([|box|], box)

let goMap =
    let res = Dictionary<_,_>()
    res.Add("type",(0,10))
    res.Add("hasOBONamespace",(1,11))
    res.Add("subClassOf",(2,12))
    res.Add("label",(3,13))
    res.Add("annotatedTarget",(4,14))
    res.Add("id",(5,15))
    res.Add("annotatedSource",(6,16))
    res.Add("hasExactSynonym",(7,17))
    res.Add("hasDbXref",(8,18))
    res.Add("annotatedProperty",(9,19))
    res
let goQ4_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([0<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,1<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,2<terminalSymbol>,0<rsmState>)  
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,0<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,0<rsmState>)|])
    RSM([|box|], box)
     
let goQ9_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([1<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,5<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,6<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,0<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,3<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,5<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,6<terminalSymbol>,1<rsmState>)|])
    RSM([|box|], box)
    
let goQ10_5 =
    let box,_ = GLLTests.makeRsmBox(Dictionary(), 0<rsmState>, HashSet([1<rsmState>]),
                [|TerminalEdge(0<rsmState>,0<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,4<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,6<terminalSymbol>,1<rsmState>)  
                  TerminalEdge(0<rsmState>,7<terminalSymbol>,1<rsmState>)
                  TerminalEdge(0<rsmState>,8<terminalSymbol>,1<rsmState>)
                  TerminalEdge(1<rsmState>,9<terminalSymbol>,1<rsmState>)|])
    RSM([|box|], box)