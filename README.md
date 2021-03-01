# MyDietAPI
Web API of MyDiet made with Asp.NET Core 3.1
This project includes API, JWT Authentication, Swagger UI, Logging with NLog, AutoMapper and Unit Tests (stack: XUnit, Moq, FluentAssertions)

## What do you need
You will need at least: 
- Visual Studio 2017/2019 with the last .NET Core SDK
- SQL Server Express/Developer installed and active
- SQL Server Management Studio 15.0

## How To Use
- Open ".sln" file with Visual Studio
- Right click on the project and select "Manage User Secret" and paste these lines:

  > "ConnectionStrings": {
    "DefaultConnection": "Server={SERVER_CONNECTION_STRING};Database=MyDiet;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
- Replace the "{SERVER_CONNECTION_STRING}" tag with your SQL Server connection string
- Go to Project -> Properties -> Debug and change the variable
  
  > LOG_PATH
  
  with your path for logging
- Go to Tools -> Manage Nuget Packages -> Console Nuget Packages
- Once the PowerShell Console will be open, paste these lines:

  > Add-Migration init
  > Update-Database
  
  This will ensure the creation of the database with all the migrations applied
  
#### Run the project and try it out.

## Future improvements
- Increase validation and business logic

## License
MyDietAPI is available under the MIT license. See the LICENSE file for more info.
