namespace DemoArmyBuilder

open Xenko.Engine
open Hopac
open System.Threading.Tasks
open RomanArmy
open FSharpPlus.Data

type DemoArmyBuilderGame() = 
    inherit Game()

    override this.LoadContent () =
        base.LoadContent() 
        |> fun t -> Job.fromUnitTask (fun () -> t) 
        |> Job.map (fun _ ->
            let currentScene = this.Content.Load<Scene>("MainScene")
            this.SceneSystem.SceneInstance <- new SceneInstance(this.Services, currentScene) 
            
            (this :> Game)
            |> Reader.run (Presenter.readUnitEnch imperial)
            |> currentScene.Entities.Add
        )
        |> startAsTask :> Task
        
        
    override this.Update(gameTime)  =
        base.Update(gameTime)
