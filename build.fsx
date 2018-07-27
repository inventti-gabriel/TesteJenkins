// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.ChangeLogHelper
open Fake.Testing
open System.IO

let deployDiretorio = "./deploy/"

let projetoSolucao = "TesteJenkins.sln"
let projetoDiretorio = "source/"

let Executar = (fun filename ->
  let result = ExecProcess (fun info -> 
        info.FileName <- filename 
        info.WorkingDirectory <- "./" 
        info.Arguments <- "") (System.TimeSpan.FromMinutes 5.0)

  if result<> 0 then failwithf "Erro ao executar cmd"
)

Target "Clean" (fun _ -> 
  CleanDirs [deployDiretorio]
)

Target "Compilar" (fun _ ->
  MSBuild (projetoDiretorio + "/bin") "Build" [ "Configuration", "Release"; "Platform", "x86"; "DefineConstants", "TRACE"; ] ([projetoDiretorio + projetoSolucao]) |> Log "AppBuild-Output"
)

Target "Testes" (fun _ ->
    !! (projetoDiretorio + "/bin/*Testes.dll")
    |> NUnit (fun p ->
          {p with ToolPath = "packages/NUnit.Runners/tools/"}) )



Target "Docs" (fun _ ->
  Executar "packages/Edocs.Documentacao/build.bat html source/doc"
)

"Clean"
  ==> "Compilar"
//  ==> "Testes"

// start build
RunTargetOrDefault "Compilar"