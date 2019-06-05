module Ext.Xenko.Engine

open Xenko.Engine
open FSharpPlus
open Xenko.Engine.Design
open Xenko.Core.Mathematics

type Entity with
    static member addChild (e:Entity) = e.AddChild
    
    static member fromRange (es:Entity seq) = 
        let e = new Entity()
        es |> iter e.AddChild
        e
    static member fromList (es:Entity list) = 
        let e = new Entity()
        es |> iter e.AddChild
        e
    static member clone (e:Entity) = EntityCloner.Clone(e)
    static member translate (v: Vector3) (e:Entity) = e.Transform.Position <- e.Transform.Position + v ; e