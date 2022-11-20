module Benchmarks_CFPQ


open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open BenchmarkDotNet.Jobs
open CFPQ_GLL.GLL
open Benchmarks.Program

//Java all paths --- warning: ugly names of graphs in the results, custom names are not supported yet by BenchmarkDotNet
//https://github.com/dotnet/BenchmarkDotNet/issues/1634
[<SimpleJob (RuntimeMoniker.Net50)>]
type Benchmarks() =
   member this.loadGraph () =  seq { for s in ["avrora"; "batik"; "eclipse"; "fop";"h2"; "luindex"; "lusearch";
                                                 "sunflow"; "tomcat"; "xalan"] ->
       loadJavaGraphFromCSV ($"/home/jblab/projects/eshemetova/CFPQ_GLL_F#/benchmarks/java/graphs/{s}/data.csv") }
   //"/home/jblab/projects/eshemetova/CFPQ_GLL_F#/benchmarks/java/graphs/{s}/data.csv"
   //"/home/cyfra/CFPQ_GLL/Graphs/java_data/graphs/{s}/data.csv"
                                     
   [<Benchmark>]
   [<ArgumentsSource("loadGraph")>]
   member this.reachabilityOnly (graph) =
        let gr, mapping = graph 
        runAllPairs gr (javaRsm mapping) Mode.AllPaths

[<EntryPoint>]
let benchIt  argv =
    BenchmarkRunner.Run<Benchmarks>() |> ignore
    0