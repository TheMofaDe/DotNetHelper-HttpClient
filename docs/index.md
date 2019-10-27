# DotNetHelper-HttpClient

#### DotNetHelper-HttpClient is a simple lightweight library for execute restful requests. Easy Integration with polly. Support both asynchronous and synchronous operation

|| [**View on Github**][Github] || 


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
// In doing this allows this library to not depend on third party libraries which locks 
// developer to using certain version
var employee = client.Get(JsonConvert.DeserializeObject<Employee>,$"https://jsonplaceholder.typicode.com/todos/1", Method.Get);
~~~




<!-- Links. -->

[1]:  https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[2]: http://themofade.github.io/$safeprojectname$

[Cake]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Azure DevOps]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[AppVeyor]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[GitVersion]: https://gitversion.readthedocs.io/en/latest/
[Nuget]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[Chocolately]: https://gist.github.com/davidfowl/ed7564297c61fe9ab814
[WiX]: http://wixtoolset.org/
[DocFx]: https://dotnet.github.io/docfx/
[Github]: https://github.com/TheMofaDe/$safeprojectname$


<!-- Documentation Links. -->
[Docs]: https://themofade.github.io/$safeprojectname$/index.html
[Docs-API]: https://themofade.github.io/$safeprojectname$/api/$safeprojectname$.Attribute.html
[Docs-Tutorials]: https://themofade.github.io/$safeprojectname$/tutorials/index.html
[Docs-samples]: https://dotnet.github.io/docfx/
[Changelogs]: https://dotnet.github.io/docfx/