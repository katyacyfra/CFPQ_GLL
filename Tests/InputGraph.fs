module Tests.InputGraph

open CFPQ_GLL
open CFPQ_GLL.InputGraph

[<Measure>] type graphVertex
[<Measure>] type inputGraphTerminalEdge

//[<CustomEquality>]
type Vertex = int<graphVertex>    
    //member this.Value = v
    
    //override this.ToString() =
    //    string v
    
    //override this.GetHashCode() = int v
    //override this.Equals(y: obj) =
       // y :? Vertex && (y :?> Vertex).Value = v 

type DemoInputGraphEdge =    
    | TerminalEdge of Vertex*int<terminalSymbol>*Vertex
    
[<Struct>]
type InputGraphTerminalEdge =
    val Vertex : int<graphVertex>
    val TerminalSymbol : int<terminalSymbol>
    new (vertex, terminalSymbol) = {Vertex = vertex; TerminalSymbol = terminalSymbol}

[<Struct>]
type InputGraphVertexMutableContent =
    val OutgoingTerminalEdges : ResizeArray<InputGraphEdge<Vertex>>    
    new (terminalEdges) = {OutgoingTerminalEdges = terminalEdges}


type InputGraph (edges) =
    let vertices = System.Collections.Generic.Dictionary<Vertex, InputGraphVertexMutableContent>()
      
    let addVertex v =
        if not <| vertices.ContainsKey v
        then vertices.Add(v,InputGraphVertexMutableContent(ResizeArray<_>()))
        vertices.[v]
        
    let addEdges edges =        
        edges
        |> Array.iter (function                         
                        | TerminalEdge (_from, smb, _to) ->
                            let vertexContent = addVertex _from
                            addVertex _to |> ignore
                            InputGraphEdge(smb, _to)  |> vertexContent.OutgoingTerminalEdges.Add
                       )        
    
    do addEdges edges

    new () = InputGraph([||])
    
    interface IInputGraph<Vertex> with
        member this.GetOutgoingEdges v = vertices.[v].OutgoingTerminalEdges
    
    member this.OutgoingTerminalEdges v =
        vertices.[v].OutgoingTerminalEdges    
    member this.NumberOfVertices() = vertices.Count
    member this.AllVertices() = vertices.Keys |> Array.ofSeq
    member this.ToDot(drawSink, file) =
        seq{
            yield "digraph InputGraph{"
            yield "node [shape = plaintext]"
            
            for kvp in vertices do
                for edge in kvp.Value.OutgoingTerminalEdges do
                    yield $"%i{kvp.Key} -> %i{edge.TargetVertex} [label=%i{edge.TerminalSymbol}]"
      
            yield "}"
        }
        |> fun data -> System.IO.File.WriteAllLines(file, data)
            
    member this.RemoveOutgoingEdges v = vertices.[v].OutgoingTerminalEdges.Clear()    
    member this.AddEdges edges =
        addEdges edges

    member this.AddVertex v = addVertex v |> ignore        