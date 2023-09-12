# Ensek Tech Test

## Database Creation

This project uses Entity Framework Core as the ORM to talk to the underlying SQL Server database. To create the database before running the application for the first time, please follow these instructions:

* Open a command window in the `src\EnsekTechTest.Persistence` folder
* Run the `dotnet-ef database update` command

## Known Issues

The following list the known issues that I did not have time to resolve before submitting the project:

* The SQL connection string is hardcoded to `Server=(LocalDb)\\MSSQLLocalDB;Database=EnsekTechTest;Trusted_Connection=True;`
* The unit of work is committed after each individual meter reading is added. It would be more performant to add all meter reading to the repository and then commit the unit of work at the end
* The functional test project uses the same database as the locally running instance, causing the tests to pollute the database