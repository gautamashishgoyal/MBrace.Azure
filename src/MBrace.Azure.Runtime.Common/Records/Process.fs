﻿namespace Nessos.MBrace.Azure.Runtime.Common

open Microsoft.ServiceBus
open Microsoft.ServiceBus.Messaging
open Microsoft.WindowsAzure.Storage
open Microsoft.WindowsAzure.Storage.Table
open Nessos.MBrace
open Nessos.MBrace.Azure.Runtime
open System
open System.Diagnostics
open System.Net
open System.Runtime.Serialization

type ProcessState = 
    | Initialized
    | Killed
    | Completed
    override this.ToString() = 
        match this with
        | Initialized -> "Initialized"
        | Killed -> "Killed"
        | Completed -> "Completed"

type ProcessEntity(pk, pid, pname, cancellationUri, state, createdt, completedt, completed, resultUri) = 
    inherit TableEntity(pk, pid)
    member val Id  : string = pid with get, set
    member val Name : string = pname with get, set
    member val State : string = state with get, set
    member val TimeCreated : DateTime = createdt with get, set
    member val TimeCompleted : DateTime = completedt with get, set
    member val Completed : bool = completed with get, set
    member val ResultUri : string = resultUri with get, set
    member val CancellationUri : string = cancellationUri with get, set
    new () = new ProcessEntity(null, null, null, null, null, Unchecked.defaultof<_>, Unchecked.defaultof<_>, false, null)

type ProcessMonitor internal (table : string) = 
    let pk = "process"
    
    member this.CreateRecord(pid : string, name, ctsUri) = async { 
        let now = DateTime.UtcNow
        let e = new ProcessEntity(pk, pid, name, ctsUri, ProcessState.Initialized.ToString(), now, now, false, null)
        do! Table.insert<ProcessEntity> table e
        return e
    }

    member this.SetKilled(pid : string) = async {
        let! e = Table.read<ProcessEntity> table pk pid
        e.State <- ProcessState.Killed.ToString()
        e.TimeCompleted <- DateTime.UtcNow
        e.Completed <- true
        let! e' = Table.merge table e
        return ()
    }

    member this.SetCompleted(pid : string, result : string) = async {
        let! e = Table.read<ProcessEntity> table pk pid
        e.State <- ProcessState.Completed.ToString()
        e.TimeCompleted <- DateTime.UtcNow
        e.Completed <- true
        e.ResultUri <- result
        let! e' = Table.merge table e
        return ()
    }

    member this.GetProcess(pid : string) = async {
        return! Table.read<ProcessEntity> table pk pid
    }

    member this.GetProcesses () = async {
        return! Table.readBatch<ProcessEntity> table pk
    }