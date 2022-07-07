# squares-api-exercise

## Functional Details

Solution exposes 4 endpoints
<br/>
> `api/point` : <br/>
>>`POST` : `/import` Accepts list of points <br/>
>>> where Point is ```{  X = 0,  Y = 0 }```<br/>

>>`POST` : Accepts Point <br/>

>>`DELETE` : Accepts Point as parameter

> `api/square` : <br/>
>> `GET`: Returns list for squares with points

<br/><br/>
Swagger -> `/swagger/index.html`

## Technical Details
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

### Using Docker
Run following commands from project directory
```
docker build -t squareapi .
docker run -d --rm 5000:80 --name squareapi squareapi
```
