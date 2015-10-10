﻿namespace MBrace.Azure.Tests.Store

open NUnit.Framework
open MBrace.Core
open MBrace.Core.Tests
open MBrace.Core.Internals
open MBrace.Runtime
open MBrace.Runtime.Components
open MBrace.ThreadPool
open MBrace.Azure
open MBrace.Azure.Store
open MBrace.Azure.Tests

// BlobStore tests

[<AbstractClass; TestFixture>]
type ``Azure BlobStore Tests``(config : Configuration, localWorkers : int) = 
    inherit ``CloudFileStore Tests``(parallelismFactor = 10)
    let session = new ClusterSession(config, localWorkers)
    
    [<TestFixtureSetUp>]
    member __.Init() = session.Start()
    
    [<TestFixtureTearDown>]
    member __.Fini() = session.Stop()
    
    override __.FileStore = session.Cluster.GetResource<ICloudFileStore>()
    override __.Serializer = session.Cluster.GetResource<ISerializer>()
    override __.IsCaseSensitive = false
    override __.Run(workflow : Cloud<'T>) = session.Cluster.Run workflow
    override __.RunLocally(workflow : Cloud<'T>) = session.Cluster.RunLocally workflow

[<TestFixture>]
type ``BlobStore Tests - Standalone Cluster - Remote Storage``() =
    inherit ``Azure BlobStore Tests``(mkRemoteConfig (), 4)

[<TestFixture>]
type ``BlobStore Tests - Standalone Cluster - Storage Emulator``() = 
    inherit ``Azure BlobStore Tests``(mkEmulatorConfig (), 4)

[<TestFixture>]
type ``BlobStore Tests - Remote Cluster - Remote Storage``() = 
    inherit ``Azure BlobStore Tests``(mkRemoteConfig (), 0)


// CloudAtom tests


[<AbstractClass; TestFixture>]
type ``Azure CloudAtom Tests``(config : Configuration, localWorkers : int) = 
    inherit ``CloudAtom Tests``(parallelismFactor = 5)
    let session = new ClusterSession(config, localWorkers)
    
    [<TestFixtureSetUp>]
    member __.Init() = session.Start()
    
    [<TestFixtureTearDown>]
    member __.Fini() = session.Stop()
    
    override __.IsSupportedNamedLookup = true
    override __.Run wf = session.Cluster.Run wf
    override __.RunLocally wf = session.Cluster.RunLocally wf
    override __.Repeats = 1

[<TestFixture>]
type ``CloudAtom Tests - Standalone Cluster - Remote Storage``() = 
    inherit ``Azure CloudAtom Tests``(mkRemoteConfig (), 4)

[<TestFixture>]
type ``CloudAtom Tests - Standalone Cluster - Storage Emulator``() = 
    inherit ``Azure CloudAtom Tests``(mkEmulatorConfig (), 4)
    override __.Repeats = 3

[<TestFixture>]
type ``CloudAtom Tests - Remote Cluster - Remote Storage``() = 
    inherit ``Azure CloudAtom Tests``(mkRemoteConfig (), 0)


// CloudQueue Tests


[<AbstractClass; TestFixture>]
type ``Azure CloudQueue Tests``(config : Configuration, localWorkers : int) = 
    inherit ``CloudQueue Tests``(parallelismFactor = 10)
    let session = new ClusterSession(config, localWorkers)
    
    [<TestFixtureSetUp>]
    member __.Init() = session.Start()
    
    [<TestFixtureTearDown>]
    member __.Fini() = session.Stop()
    
    override __.Run wf = session.Cluster.Run wf
    override __.RunLocally wf = session.Cluster.RunLocally wf
    override __.IsSupportedNamedLookup = true

[<TestFixture>]
type ``CloudQueue Tests - Standalone Cluster - Remote Storage``() = 
    inherit ``Azure CloudQueue Tests``(mkRemoteConfig (), 4)

[<TestFixture>]
type ``CloudQueue Tests - Remote Cluster - Remote Storage``() = 
    inherit ``Azure CloudQueue Tests``(mkRemoteConfig (), 0)


// CloudDictionary tests


[<AbstractClass; TestFixture>]
type ``Azure CloudDictionary Tests``(config : Configuration, localWorkers : int) = 
    inherit ``CloudDictionary Tests``(parallelismFactor = 5)
    let session = new ClusterSession(config, localWorkers)
    
    [<TestFixtureSetUp>]
    member __.Init() = session.Start()
    
    [<TestFixtureTearDown>]
    member __.Fini() = session.Stop()
    
    override __.Run wf = session.Cluster.Run wf
    override __.RunLocally wf = session.Cluster.RunLocally wf
    override __.IsInMemoryFixture = false
    override __.IsSupportedNamedLookup = true

[<TestFixture>]
type ``CloudDictionary Tests - Standalone Cluster - Storage Emulator``() = 
    inherit ``Azure CloudDictionary Tests``(mkEmulatorConfig (), 4)

[<TestFixture>]
type ``CloudDictionary Tests - Standalone Cluster - Remote Storage``() = 
    inherit ``Azure CloudDictionary Tests``(mkRemoteConfig (), 4)

[<TestFixture>]
type ``CloudDictionary Tests - Remote Cluster - Remote Storage``() = 
    inherit ``Azure CloudDictionary Tests``(mkRemoteConfig (), 0)


// CloudValue tests


[<AbstractClass; TestFixture>]
type ``Azure CloudValue Tests``(config : Configuration, localWorkers : int) = 
    inherit ``CloudValue Tests``(parallelismFactor = 5)
    let session = new ClusterSession(config, localWorkers)
    
    [<TestFixtureSetUp>]
    member __.Init() = session.Start()
    
    [<TestFixtureTearDown>]
    member __.Fini() = session.Stop()
    
    override __.Run wf = session.Cluster.Run wf
    override __.RunLocally wf = session.Cluster.RunLocally wf
    override __.IsSupportedLevel _ = true

[<TestFixture>]
type ``CloudValue Tests - Standalone Cluster - Storage Emulator``() = 
    inherit ``Azure CloudValue Tests``(mkEmulatorConfig (), 4)

[<TestFixture>]
type ``CloudValue Tests - Standalone Cluster - Remote Storage``() = 
    inherit ``Azure CloudValue Tests``(mkRemoteConfig (), 4)

[<TestFixture>]
type ``CloudValue Tests - Remote Cluster - Remote Storage``() = 
    inherit ``Azure CloudValue Tests``(mkRemoteConfig (), 0)