﻿#I "../../bin/"

#r "MBrace.Core.dll"
#r "MBrace.Library.dll"
#r "MBrace.Azure.Runtime.exe"

#r "Microsoft.WindowsAzure.Storage.dll"
#r "Microsoft.ServiceBus.dll"

open Nessos.MBrace
open Nessos.MBrace.Library
open Nessos.MBrace.Azure.Runtime.Common
open Nessos.MBrace.Azure.Runtime.Resources

let conn = System.IO.File.ReadAllLines "/mbrace/conn.txt"

let config = 
    { StorageConnectionString = conn.[0]
      ServiceBusConnectionString = conn.[1] } 

ClientProvider.Activate config

#time "on"

let (!) (task : Async<'T>) = Async.RunSynchronously task

let p = Latch.GetUri "tmp"
let l = ! Latch.Init(p, 0)
let l' = Latch.Get(p)

[|1..10|]
|> Array.map (fun _ -> async { do! l.Increment() |> Async.Ignore })
|> Async.Parallel
|> Async.Ignore
|> Async.RunSynchronously

l.Value

let c = !BlobCell.Init(BlobCell.GetUri "tmp", fun () -> 42)
!c.GetValue<int>()

let q = !Queue.Init(Queue.GetUri "tmp")

!q.Enqueue(42)


!q.TryDequeue<int>()

q.Length


let rs  = !ResultCell.Init(ResultCell.GetUri "tmp")
async { do! Async.Sleep 10000 
        do! rs.SetResult(42) }
|> Async.Start

!rs.AwaitResult()


//MBraceRuntime.WorkerExecutable <- __SOURCE_DIRECTORY__ + "/../../bin/MBrace.Runtime.Azure.exe"
//
//let runtime = MBraceRuntime.InitLocal(4)
//
//let getWordCount inputSize =
//    let map (text : string) = cloud { return text.Split(' ').Length }
//    let reduce i i' = cloud { return i + i' }
//    let inputs = Array.init inputSize (fun i -> "lorem ipsum dolor sit amet")
//    MapReduce.mapReduce map 0 reduce inputs
//
//
//let t = runtime.RunAsTask(getWordCount 2000)
//do System.Threading.Thread.Sleep 3000
//runtime.KillAllWorkers() 
//runtime.AppendWorkers 4
//
//t.Result