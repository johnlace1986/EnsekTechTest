# Ensek Tech Test

## Database Creation

This project uses Entity Framework Core as the ORM to talk to the underlying SQL Server database. To create the database before running the application for the first time, please follow these instructions:

* Open the [persistencesettings.json](src/EnsekTechTest.Persistence/persistencesettings.json) file and update the SQL connection string
* Open a command window in the `src\EnsekTechTest.Persistence` folder
* Run the `dotnet-ef database update` command

## Known Issues

The following list the known issues that I did not have time to resolve before submitting the project:

* The unit of work is committed after the meter readings for each account are added. It could be more performant to add all meter readings for all accounts to the repository and then commit the unit of work at the end
* The functional test project uses the same database as the locally running instance, causing the tests to pollute the database