## 🔽 Tutorial 05 🔒

> ⚠️ **Warning**
> Note: This tutorial requires tools that are not yet available for .NET Core, i.e., the tutorial does only work on Windows.

### Task 1

In the project **Task1-Template.zip** you will find a template for a simple Web service and a template for its WSDL1.1 description (_ProfileService.wsdl_).

<img src="./ProfileService.svg" width="514" height="700" style="display: block; margin-left: auto; margin-right: auto;" role="presentation" alt="ProfileService Diagram">

1. Based on the above diagram complete the WSDL description.
2. Based on the WSDL description above, generate a service stub class:
   - Go to Start → All Programs → Microsoft Visual Studio → Microsoft Visual Studio Tools → Developer Command Prompt
   - cd PATH_TO_YOUR_PROJECT
   - **`wsdl.exe /server ProfileService.wsdl /out:ProfileService.asmx.cs`** (Replaces the existing file with the newly generated one)
3. Complete the implementation of the `PrintProfile` operation by returning a concatenation of its parameters.
   - Remove all the abstract-keywords and start the service.
   - Test its functionality in the browser by sending a GET request to the service URL: [http://localhost:5000/ProfileService.asmx](http://localhost:5000/ProfileService.asmx)

> ℹ️ **Info**
> Opening the image in a new tab will allow you to copy texts.

### Task 2

1. In the given solution **Task2-Template.zip** create a subproject named Service, which should be a simple Web service for sorting an array of integers (input: `int[]`, output: `int[]`):
   - Select _File → New project → C# → ASP.NET Web Application (.NET Framework) → Empty (Uncheck Configure for HTTPS) → Create_
   - In the newly added project, _Right Click → Add → New Item → WebService (ASMX) → Add (Name: SortService.asmx)_
   - Complete the class `SortService.asmx.cs` by implementing a method `sort` for sorting arrays of integers.
   - Start the service (endpoint should be at [http://localhost:{PORT}/SortService.asmx](http://localhost:{PORT}/SortService.asmx)) and download the automatically generated WSDL file into the directory of the SortServiceClient project.
2. Implement the service client in the subproject Client, which should invoke the service above. Use an automatically generate proxy class:
   - Go to _Start → All Programs → Microsoft Visual Studio → Microsoft Visual Studio Tools → Developer Command Prompt_
   - cd _PATH_TO_THE_PROJECT_\
   - **`wsdl.exe YOUR_WSDL_NAME.wsdl /out:SortServiceProxy.cs`** (Replaces the existing file with the newly generated one)
   - Run both subprojects to test the communication

### Task 3

Write down a WSDL2.0 file with one single operation called “Substring”, which should extract characters from a given string input in the given range `start` and `end`. Create a Web service and a corresponding client based on this WSDL2.0 file. Test your solution by running the both projects in parallel.
