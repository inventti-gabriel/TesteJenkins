// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.ChangeLogHelper
open System.IO

let deployDir = "./deploy/"
let deployServiceDir = deployDir + "1.Executor"

let executorProject = "Executor.csproj"
let executorPath = "source/app/Executor/"

let Executar = (fun filename ->
  let result = ExecProcess (fun info -> 
        info.FileName <- filename 
        info.WorkingDirectory <- "./" 
        info.Arguments <- "") (System.TimeSpan.FromMinutes 5.0)

  if result<> 0 then failwithf "Erro ao executar cmd"
)

Target "Clean" (fun _ -> 
  CleanDirs [deployDir]
)

Target "Executor" (fun _ ->
  MSBuild (executorPath + "/bin") "Build" [ "Configuration", "Release"; "Platform", "x86"; "DefineConstants", "TRACE"; ] ([executorPath + executorProject]) |> Log "AppBuild-Output"
)

Target "Docs" (fun _ ->
  Executar "packages/Edocs.Documentacao/build.bat html source/doc"
)

"Clean"
  ==> "Executor"

// start build
RunTargetOrDefault "Executor"