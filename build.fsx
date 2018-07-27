// include Fake lib
#r @"packages\FAKE\tools\FakeLib.dll"

open Fake
open Fake.AssemblyInfoFile
open Fake.ChangeLogHelper
open System.IO

let binDir = "./bin/release"

let deployDir = "./deploy/"
let deployServiceDir = deployDir + "1.REINFPack.Service/"
let deployApiDir = deployDir + "2.edocsPackApi/"
let deployConsoleDir = deployDir + "3.edocsPackConsole/"

let commonsProject = "REINFPack.csproj"
let serviceProject = "REINFPack.Service.csproj"
let apiProject = "edocsPackApi.csproj"
let consoleProject = "edocsPackConsole.csproj"

let commonsPath = "source/app/REINFPack/"
let servicePath = "source/app/REINFPack.Service/"
let apiPath = "source/app/edocsPackApi/"
let consolePath = "source/app/edocsPackConsole/"
let scriptsPath = "source/Scripts/"

let Executar = (fun filename ->
  let result = ExecProcess (fun info -> 
        info.FileName <- filename 
        info.WorkingDirectory <- "./" 
        info.Arguments <- "") (System.TimeSpan.FromMinutes 5.0)

  if result<> 0 then failwithf "Erro ao executar cmd"
)

Target "Clean" (fun _ -> 
  CleanDirs [(commonsPath + binDir); (servicePath + binDir); (apiPath + binDir); deployDir]
)

Target "API" (fun _ ->
  MSBuild (apiPath + binDir) "WebPublish" [ "Configuration", "Release"; "Platform", "x86"; "DefineConstants", "TRACE"; "WebPublishMethod", "FileSystem"; "publishUrl", binDir + "/publish" ] ([apiPath + apiProject]) |> Log "AppBuild-Output"
)

Target "NPM" (fun _ ->
  Executar "tools\\npm.cmd"
)

Target "Webpack" (fun _ ->
  Executar "tools\\webpack.cmd"
)

Target "Console" (fun _ ->
  MSBuild (consolePath + binDir) "WebPublish" [ "Configuration", "Release"; "Platform", "x86"; "DefineConstants", "TRACE"; "WebPublishMethod", "FileSystem"; "publishUrl", binDir + "/publish" ] ([consolePath + consoleProject]) |> Log "AppBuild-Output"
  FileUtils.cp_r (consolePath + "/dist") (consolePath + binDir + "/publish/dist")
)

Target "Service" (fun _ ->
  MSBuild (servicePath + binDir) "Build" [ "Configuration", "Release"; "Platform", "x86"; "DefineConstants", "TRACE"; ] ([servicePath + serviceProject]) |> Log "AppBuild-Output"
)

Target "Docs" (fun _ ->
  Executar "tools\\docs.cmd"
)

Target "Scripts" (fun _ ->
  FileHelper.CopyDir (deployDir + "/Scripts") scriptsPath allFiles
)

Target "Deploy" (fun _ ->
    FileUtils.mkdir(deployServiceDir)
    FileHelper.CopyDir (deployServiceDir) (servicePath + binDir) allFiles
    FileHelper.CopyDir (deployServiceDir + "/Utils/") ("packages/Inventti.Config/Inventti.Config") allFiles

    FileHelper.CopyDir (deployApiDir) (apiPath + binDir + "/publish") allFiles
    FileHelper.CopyDir (deployConsoleDir) (consolePath + binDir + "/publish") allFiles

    FileHelper.DeleteDir (deployConsoleDir + "/src")
)

"Clean"
  ==> "Service"
  ==> "API"
  ==> "NPM"
  ==> "Webpack"
  ==> "Console"
  ==> "Scripts"
  ==> "Deploy"

// start build
RunTargetOrDefault "Deploy"