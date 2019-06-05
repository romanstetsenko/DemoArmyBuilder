module Presenter

open FSharpPlus
open FSharpPlus.Data
open Xenko.Core.Mathematics
open Xenko.Engine
open Ext.Xenko.Engine

let readModel name =
    let aux (ctx : Game) =
        let pf = ctx.Content.Load<Prefab>(name)
        let es = pf.Instantiate()
        if length es = 1 then head es
        else es |> Entity.fromRange
    aux |> Reader

open RomanArmy

let rec readUnit u =
    let readManyAndTranslate tf units = 
        units |>> readUnit |> sequence |>> mapi (tf >> (fun (x,y,z) ->  Vector3(float32 x, float32 y, float32 z) |> Entity.translate) ) 
    let readCorp comm subs tfComm tfSubs = 
        comm |> readManyAndTranslate tfComm 
        >>= (fun cs -> 
            subs |> readManyAndTranslate tfSubs
            |>> (fun ss -> cs ++ ss)
        )
        |>> Entity.fromList

    match u with
    | Decanus | Legionary -> 
        readModel "LegionaryMock"
    | Contubernium(decanus, units) -> 
        readCorp [decanus] units (fun _ -> 0, 0, 3) (fun i -> 0, 0, -i) 
    | Signifer
    | Tesserarius
    | Optio
    | PrimiOrdines -> 
        readModel "LegionaryMock"
    | Century(comms, subs) -> 
        readCorp comms subs (fun i -> (i * 2 + 6), 0, 7) (fun i -> i * 2, 0, 0)
    | Cohort(centuries) -> 
        readCorp [] centuries (fun _ -> 0, 0, 0) (fun i -> i * 25, 0, 0)
    | LegatusLegionis -> 
        readModel "Legionary"
    | Imperial(comm, cohorts) -> 
        readCorp [comm] cohorts (fun _ -> 72, 0, 175) (fun i -> 0, 0, i * 20)