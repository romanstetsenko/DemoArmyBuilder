module Presenter

open FSharpPlus
open FSharpPlus.Data
open Xenko.Core.Mathematics
open Xenko.Engine
open Ext.Xenko.Engine
open Xenko.Rendering
open Xenko.Extensions

let readPrefab name =
    let aux (ctx : Game) =
        let pf = ctx.Content.Load<Prefab>(name)
        let es = pf.Instantiate()
        if length es = 1 then head es
        else es |> Entity.fromRange
    aux |> Reader

let readMaterial name = 
    let aux (ctx:Game) = 
        ctx.Content.Load<Material>(name)
    aux |> Reader

let readModel name =
    let aux (ctx:Game) = 
        ctx.Content.Load<Model>(name)
    aux |> Reader

open RomanArmy
open DemoArmyBuilder
open Xenko.Graphics.GeometricPrimitives
open Xenko.Graphics

let rec readUnit (addInstance:TransformComponent -> unit) u  =
    let readEmptyModel _ : Reader<_, _> =
        let e = new Entity(Vector3.Zero)
        e.Transform.Scale <- Vector3(0.5f, 2.f, 0.5f )
        addInstance(e.Transform)
        Reader.Return e

    let readManyAndTranslate tf units = 
        units |>>  (readUnit addInstance) |> sequence |>> mapi (tf >> (fun (x,y,z) ->  Vector3(float32 x, float32 y, float32 z) |> Entity.translate) )
    
    let readCorp comm subs tfComm tfSubs = 
        comm 
        |> readManyAndTranslate tfComm 
        >>= (fun cs -> 
            subs |> readManyAndTranslate tfSubs
            |>> (fun ss -> cs ++ ss)
        )|>> Entity.fromList
    match u with
    | Decanus | Legionary -> 
        readEmptyModel "LegionaryMock"
    | Contubernium(decanus, units) -> 
        readCorp [decanus] units (fun _ -> 0, 0, 3) (fun i -> 0, 0, -i) 
    | Signifer
    | Tesserarius
    | Optio
    | PrimiOrdines -> 
        readEmptyModel "LegionaryMock"
    | Century(comms, subs) -> 
        readCorp comms subs (fun i -> (i * 2 + 6), 0, 7) (fun i -> i * 2, 0, 0)
    | Cohort(centuries) -> 
        readCorp [] centuries (fun _ -> 0, 0, 0) (fun i -> i * 25, 0, 0)
    | LegatusLegionis -> 
        readEmptyModel "LegionaryMock"
    | Imperial(comm, cohorts) -> 
        readCorp [comm] cohorts (fun _ -> 72, 0, 175) (fun i -> 0, 0, i * 20)

let readGraphicsDevice () = 
    fun (ctx:Game) ->
        ctx.GraphicsDevice
    |> Reader

let readUnitEnch u  = 
    let newMultiMesh (m:Model) = //(entity:Entity) = 
        readMaterial "Materials/marble/marble"//"CubeVertexColourMaterial"
        >>= (fun mat -> 
            readGraphicsDevice()
            |>> (fun graphicsDevice -> 
                let ourPrimitive = GeometricPrimitive.Cube.New(device = graphicsDevice, size = 1.0f)
                let ourPrimitive2 = GeometricPrimitive.Cube.New( size = 1.0f)
                let primitiveMeshDraw = ourPrimitive.ToMeshDraw()
                let mm = PoorMansMultiMesh()
                //let modelMeshDraw = entity.Get<ModelComponent>().Model.Meshes.[0].Draw
                let rawModelMeshDraw = m.Meshes.[0].Draw
                let m = Mesh()
                m.Draw <- rawModelMeshDraw
                mm.Mesh <- m
                mm.Material <- mat
                mm
            )
        )
        
    readModel "Cube_2" 
    >>= newMultiMesh
    >>= (fun (mm:PoorMansMultiMesh) ->  
        readUnit ( mm.AddInstance >> ignore ) u 
        |>> (fun (entity:Entity) -> 
            entity.Add(mm); entity)
    )