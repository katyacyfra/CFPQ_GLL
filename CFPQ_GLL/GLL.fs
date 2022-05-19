module CFPQ_GLL.GLL

open CFPQ_GLL.RSM
open CFPQ_GLL.GSS
open CFPQ_GLL.InputGraph
open CFPQ_GLL.SPPF
open FSharpx.Collections
    
let eval (graph:InputGraph) startVertices (query:RSM) (startStates:array<int<rsmState>>) =    
    let reachableVertices = ResizeArray<_>()
    let descriptorToProcess = System.Collections.Generic.Stack<_>()
    
    let gss = GSS()
    let matchedRanges = MatchedRanges()    
    
    let inline addDescriptor (descriptor:Descriptor) =
        if not <| gss.IsThisDescriptorAlreadyHandled descriptor
        then descriptorToProcess.Push descriptor 
        
    startVertices
    |> Array.iter (fun v ->        
        startStates
        |> Array.iter (fun startState ->
            let gssVertex = gss.AddNewVertex(v, startState)            
            Descriptor(v, gssVertex, startState, None)
            |> descriptorToProcess.Push
            )
        )
    
    let handleDescriptor (currentDescriptor:Descriptor) =
        
        gss.AddDescriptorToHandled currentDescriptor
                
        if query.IsFinalState currentDescriptor.RSMState                        
        then
            let startPosition = currentDescriptor.GSSVertex.InputPosition
            if Array.contains startPosition startVertices
            then reachableVertices.Add (startPosition, currentDescriptor.InputPosition)
            
            let matchedRange =
                match currentDescriptor.MatchedRange with             
                | None ->
                    MatchedRange(
                           currentDescriptor.InputPosition
                         , currentDescriptor.InputPosition
                         , currentDescriptor.RSMState
                         , currentDescriptor.RSMState
                         , RangeType.Epsilon
                    )                    
                | Some range -> range
                        
            gss.Pop(currentDescriptor, matchedRange)
            |> ResizeArray.iter (
                fun gssEdge ->                
                    let leftSubRange = gssEdge.Info
                    let rightSubRange =                        
                        MatchedRange(
                            currentDescriptor.GSSVertex.InputPosition
                          , currentDescriptor.InputPosition
                          , match gssEdge.Info with None -> gssEdge.RSMState | Some v -> v.RSMRange.EndPosition
                          , gssEdge.RSMState
                          , RangeType.NonTerminal 0<rsmState>
                        )
                        
                    let newRange = matchedRanges.AddMatchedRange(leftSubRange, rightSubRange)
                    Descriptor(currentDescriptor.InputPosition, gssEdge.GSSVertex, gssEdge.RSMState, Some newRange)
                    |> addDescriptor
                )
            
        let outgoingTerminalEdgesInGraph = graph.OutgoingTerminalEdges currentDescriptor.InputPosition        
            
        let outgoingNonTerminalEdgesInRSM = query.OutgoingNonTerminalEdges currentDescriptor.RSMState
        let outgoingTerminalEdgesInRSM = query.OutgoingTerminalEdges currentDescriptor.RSMState       
        
        outgoingNonTerminalEdgesInRSM
        |> Array.iter (fun edge ->
               let edge = unpackRSMNonTerminalEdge edge
               let newGSSVertex, positionsForPops =
                    gss.AddEdge(currentDescriptor.GSSVertex
                                , edge.State
                                , currentDescriptor.InputPosition
                                , edge.NonTerminalSymbolStartState
                                , currentDescriptor.MatchedRange)
               
               Descriptor(currentDescriptor.InputPosition, newGSSVertex, edge.NonTerminalSymbolStartState, None)
               |> addDescriptor
               positionsForPops
               |> ResizeArray.iter (fun matchedRange ->                   
                   let rightSubRange =
                       MatchedRange(
                            matchedRange.InputRange.StartPosition
                          , matchedRange.InputRange.EndPosition
                          , currentDescriptor.RSMState
                          , edge.State
                          , RangeType.NonTerminal 0<rsmState>
                       )
                                                                                   
                   let leftSubRange = currentDescriptor.MatchedRange
                   let newRange = matchedRanges.AddMatchedRange(leftSubRange, rightSubRange)                       
                   Descriptor(matchedRange.InputRange.EndPosition, currentDescriptor.GSSVertex, edge.State, Some newRange) |> addDescriptor)
        )
        
        outgoingTerminalEdgesInRSM
        |> Array.iter (fun e1 ->
            outgoingTerminalEdgesInGraph
            |> Array.iter (fun e2 ->
                let graphEdge = unpackInputGraphTerminalEdge e2
                let rsmEdge = unpackRSMTerminalEdge e1
                if graphEdge.TerminalSymbol = rsmEdge.TerminalSymbol
                then
                    let currentlyMatchedRange =
                        MatchedRange(
                            currentDescriptor.InputPosition
                            , graphEdge.Vertex
                            , currentDescriptor.RSMState
                            , rsmEdge.State
                            , RangeType.Terminal rsmEdge.TerminalSymbol)
                        
                    let newMatchedRange = matchedRanges.AddMatchedRange (currentDescriptor.MatchedRange, currentlyMatchedRange)                        
                    Descriptor(graphEdge.Vertex, currentDescriptor.GSSVertex, rsmEdge.State, Some newMatchedRange) |> addDescriptor))
    
    let startTime = System.DateTime.Now    
    
    while descriptorToProcess.Count > 0 do
        descriptorToProcess.Pop()
        |> handleDescriptor
    
    printfn $"Query processing total time: %A{(System.DateTime.Now - startTime).TotalMilliseconds} milliseconds"
        
    reachableVertices, matchedRanges
