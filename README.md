# squares-api-exercise

## Functional

Solution exposes 3 endpoints
<br/>
> `api/points` : <br/>
>>`POST` : Accepts list of points <br/>
>>> where Point is ```{  X = 0,  Y = 0 }```<br/>

>>`DELETE` : Accepts id as parameter

> `api/squares` : <br/>
>> `GET`: Returns list for squares with points


## Technical
Solution is built on `.net 6` web api, and using `SQL Server` as a persistant layer by default (but can be modified to use any datastores).
<br/>
### Building this application
Project requires `.net 6 SDKs` to be installed.
```
dotnet build
dotnet run
```
the above command can be used to build and run the application in the project directory.

**Note**: As mentioned earlier project required SQL server by default, so `appsettings.json` file needs to be updated with proper connection string or modify the `Program.cs` file to inject differnt persistance layer.
