
# Ensek Tech Test

## Database Creation

This project uses Entity Framework Core as the ORM to talk to the underlying SQL Server database. To create the database before running the application for the first time, please follow these instructions:

* Open the [persistencesettings.json](src/EnsekTechTest.Persistence/persistencesettings.json) file and update the SQL connection string
* Open a command window in the [`src\EnsekTechTest.Persistence`](src\EnsekTechTest.Persistence) folder
* Run the `dotnet-ef database update` command (requires the EF Core CLI to be installed)

## Running the Application
The backend for the application is an ASP.NET Web API that can be started the same way as running any other .NET application (e.g running `dotnet run` against the [EnsekTechTest project](src/EnsekTechTest), opening the solution in Visual Studio and pressing `F5`, etc). The base address for the backend API will be `https://localhost/7264` by default.

The frontend of the application is an Angular web application that can be started by following these instructions:

* Open a command window in the [`src\EnsekTechTest.UI`](src\EnsekTechTest.UI) folder
* Run the `npm install` command (requires the NPM CLI to be installed)
* Run the `npm start` command
* Navigate to the web application's base address in a browser
The base address for the frontend web application will be `http://localhost:4200` by default.

## Automated Tests
The solution contains a number of unit tests, as well as a functional test that spins up an instance of the API, uploads a CSV and then asserts that the relevant meter readings we saved to the database and the correct response was returned.

## Known Issues

The following lists the known issues that I did not have time to resolve before submitting the project:

* The unit of work is committed after each individual meter reading is added. It could be more performant to add all meter reading to the repository and then commit the unit of work at the end
* The functional test project uses the same database as the locally running instance, causing the tests to pollute the database
* The web application does not change to any kind of loading screen to indicate that the API is currently being called so if you upload a file and get the same response back to back it doesn't look like anything is happening
* There are no front end unit tests