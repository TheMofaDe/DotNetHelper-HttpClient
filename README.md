# DotNetHelper-HttpClient

#### DotNetHelper-HttpClient is a simple lightweight library for executing restful requests. Easy Integration with polly. Support both asynchronous and synchronous operation

|| [**Change Log**][Changelogs] • || [**View on Github**][Github]|| 

| Package  | Tests | Code Coverage |
| :-----:  | :---: | :------: |
| ![Build Status][nuget-downloads]  | ![Build Status][tests]  | [![codecov](https://codecov.io/gh/TheMofaDe/DotNetHelper-HttpClient/branch/master/graph/badge.svg)](https://codecov.io/gh/TheMofaDe/DotNetHelper-HttpClient) |


| Continous Integration | Windows | Linux | MacOS | 
| :-----: | :-----: | :-----: | :-----: |
| **AppVeyor** | [![Build status](https://ci.appveyor.com/api/projects/status/9mog32m4mejqyd3i?svg=true)](https://ci.appveyor.com/project/TheMofaDe/DotNetHelper-HttpClient)  | | |
| **Azure Devops** | ![Build Status][azure-windows]  | ![Build Status][azure-linux]  | ![Build Status][azure-macOS] | 

## Features

* All apis are available in both asynchronous and synchronous operation

#### Get string from rest api
~~~csharp
var client = new RestClient();
var json = client.GetString($"https://jsonplaceholder.typicode.com/todos/1",  Method.Get);
~~~

#### Get Stream from rest api
~~~csharp
var client = new RestClient();
var stream = client.GetStream($"https://jsonplaceholder.typicode.com/todos/1",  Method.Get);
~~~

#### Get bytes[] from rest api
~~~csharp
var client = new RestClient();
var bytes = client.GetBytes($"https://jsonplaceholder.typicode.com/todos/1",  Method.Get);
~~~

#### Get HttpResponseMessage from rest api
~~~csharp
var client = new RestClient();
var httpResponse = client.GetHttpResponse($"https://jsonplaceholder.typicode.com/todos/1",  Method.Get);
~~~

#### Get Generic Type from rest api
~~~csharp
var client = new RestClient();
// The first parameter takes a Func<string, T> this allows you to implement your own deserializer 
// In doing so this will prevent the library from needing to depending on third party libraries for serialization 
var employee = client.Get(JsonConvert.DeserializeObject<Employee>,$"https://jsonplaceholder.typicode.com/todos/1", Method.Get);
~~~


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
[azure-windows]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=Windows
[azure-linux]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=Linux
[azure-macOS]: https://dev.azure.com/Josephmcnealjr0013/DotNetHelper.HttpClient/_apis/build/status/TheMofaDe.DotNetHelper-HttpClient?branchName=master&jobName=macOS
[app-veyor]: https://ci.appveyor.com/project/TheMofaDe/DotNetHelper-HttpClient

