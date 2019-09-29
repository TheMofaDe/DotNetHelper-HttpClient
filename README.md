# DotNet Starter Template


#### *DotNet Starter Template is a starter kit for building .NET libraries and application. This project defined by the following things.* 


1. Using a folder structure that is based on 
[Microsoft Standard][1]. and that is also heavily used through-out the open source community.
          
| Folder Name | Description |
| ------ | ------ |
| src | Main projects (the product code) |
| tests | Test projects |
| docs | Documentation stuff, markdown files, help files etc. |
| samples | (optional) - Sample projects |
| lib | Things that can NEVER exist in a nuget package |
| artifacts | Build outputs go here. Doing a build.cmd/build.sh generates artifacts here (nupkgs, dlls, pdbs, etc.) |
| build | Build customizations scripts|
 
<br/> 

2. Using generic scripts that will implement the **DEV** in **DEV**OPS  
     A. You will have to make a few changes to one script file to apply your project specific configurations *eg Project Name & Target Frameworks*  
     
## Getting Started

##### Step #1 
Clone or download this repository
```bash
git clone https://github.com/TheMofaDe/DotNet-Starter-Template.git 
```

##### Step #2 
Open folder in Visual Studio & open the Test.sln file in the root folder and start creating new or add your existing .NET project to the solution 
<br/> 
##### Step #3 
Search & Replace all occurance the text DotNetHelper-HttpClient with your actual project name

##### Step #4 
Rename Test.sln to your actual project name

##### Step #5 
Update the *build/project.cake* file to contain your main project name  

## See it in Action  
###### After completing the steps above. Just execute the build.ps1 to see the following     
* **Versioning** *: version the application using [GitVersion]*  
- **Clean** *: remove any & all files from a previous build of the current version of the application*   
+ **Build** *: compiles the application*     
- **Test** *: execution of unit test*  
+ **Documentation** *: generation of documention of source code API into a static html thats customizable using [DocFX]*  
+ **Nuget Packages** *: create nuget package for the application*  
- **Zip** *: create zip file that contains the output of the build*  
+ **MSI Installer** *: creates a msi using [WiX]*



## Documentation
For more information, please refer to the [Officials Docs][Docs]

<!-- Links. -->
## Solution Template
[![badge](https://img.shields.io/badge/Built%20With-DotNet--Starter--Template-orange.svg)](https://github.com/TheMofaDe/DotNet-Starter-Template)


<!-- Links. -->

[1]:  https://gist.github.com/davidfowl/ed7564297c61fe9ab814

[Cake]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Azure DevOps]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[AppVeyor]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[GitVersion]: https://gitversion.readthedocs.io/en/latest/
[Nuget]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Chocolately]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[WiX]: http://wixtoolset.org/
[DocFx]: https://dotnet.github.io/docfx/
[Github]: https://github.com/TheMofaDe/DotNetHelper-HttpClient


<!-- Documentation Links. -->
[Docs]: https://themofade.github.io/DotNetHelper-HttpClient/index.html
[Docs-API]: https://themofade.github.io/DotNetHelper-HttpClient/api/DotNetHelper-HttpClient.html
[Docs-Tutorials]: https://themofade.github.io/DotNetHelper-HttpClient/tutorials/index.html
[Docs-samples]: https://dotnet.github.io/docfx/
[Changelogs]: https://dotnet.github.io/docfx/


<!-- BADGES. -->

[nuget-downloads]: https://img.shields.io/nuget/dt/DotNetHelper-HttpClient.svg?style=flat-square
[tests]: https://img.shields.io/appveyor/tests/TheMofaDe/DotNetHelper-HttpClient.svg?style=flat-square
[coverage-status]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=Windows

[azure-windows]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=Windows
[azure-linux]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=Linux
[azure-macOS]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=macOS
[app-veyor]: https://ci.appveyor.com/project/TheMofaDe/DotNetHelper-HttpClient









<!-- 

# DotNetHelper-HttpClient

#### *description.* 

|| [**Documentation**][Docs] • [**API**][Docs-API] • [**Tutorials**][Docs-Tutorials] ||  [**Change Log**][Changelogs] • || [**View on Github**][Github]|| 

| Package  | Tests | Code Coverage |
| :-----:  | :---: | :------: |
| ![Build Status][nuget-downloads]  | ![Build Status][tests]  | [![codecov](https://codecov.io/gh/TheMofaDe/DotNetHelper-HttpClient/branch/master/graph/badge.svg)](https://codecov.io/gh/TheMofaDe/DotNetHelper-HttpClient) |


| Continous Integration | Windows | Linux | MacOS | 
| :-----: | :-----: | :-----: | :-----: |
| **AppVeyor** | [![Build status](https://ci.appveyor.com/api/projects/status/9mog32m4mejqyd3i?svg=true)](https://ci.appveyor.com/project/TheMofaDe/DotNetHelper-HttpClient)  | | |
| **Azure Devops** | ![Build Status][azure-windows]  | ![Build Status][azure-linux]  | ![Build Status][azure-macOS] | 

## Features

## Getting Started

## Documentation
For more information, please refer to the [Officials Docs][Docs]

## Solution Template
[![badge](https://img.shields.io/badge/Built%20With-DotNet--Starter--Template-orange.svg)](https://github.com/TheMofaDe/DotNet-Starter-Template)



[Cake]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Azure DevOps]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[AppVeyor]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[GitVersion]: https://gitversion.readthedocs.io/en/latest/
[Nuget]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Chocolately]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[WiX]: http://wixtoolset.org/
[DocFx]: https://dotnet.github.io/docfx/
[Github]: https://github.com/TheMofaDe/DotNetHelper-HttpClient

[Docs]: https://themofade.github.io/DotNetHelper-HttpClient/index.html
[Docs-API]: https://themofade.github.io/DotNetHelper-HttpClient/api/DotNetHelper-HttpClient.html
[Docs-Tutorials]: https://themofade.github.io/DotNetHelper-HttpClient/tutorials/index.html
[Docs-samples]: https://dotnet.github.io/docfx/
[Changelogs]: https://dotnet.github.io/docfx/


[nuget-downloads]: https://img.shields.io/nuget/dt/DotNetHelper-HttpClient.svg?style=flat-square
[tests]: https://img.shields.io/appveyor/tests/TheMofaDe/DotNetHelper-HttpClient.svg?style=flat-square
[coverage-status]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=Windows
[azure-windows]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=Windows
[azure-linux]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=Linux
[azure-macOS]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper-HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=macOS
[app-veyor]: https://ci.appveyor.com/project/TheMofaDe/DotNetHelper-HttpClient

-->