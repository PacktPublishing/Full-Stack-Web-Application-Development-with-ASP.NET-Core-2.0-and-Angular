# Full-Stack-Web-Application-Development-with-ASP.NET-Core-2.0-and-Angular
Full Stack Web Application Development with ASP.NET Core 2.0 and Angular [Video], Published by Packt

## .NET Archictecture Concepts Covered
* Solution Organiztion and Project organiztion
* Command Query Responsibility Seperation
* REST API
* Integration and Unit Testing

## Entity Framework Core 2.1 Concepts Covered
* Query Filters
* Migrations
* Testing using the In-Memory Database
* Output Logging DbContext
* Overriding DbContext methods to add custom functionality
* Using the ModelBuilder

## ASP.NET Core 2.1 Concepts Covered
* Using the Controller attribute 
* Using the Angular SPA Template
* Using the dotnet CLI
* Attribute Routing
* SignalR
* Advanced Testing and Mocking
* Basic OAuth2 User Identity Implemetation
* Basic Change Tracking Implementation
* Validation using the `FluentValidation` package

## Angular Concepts Covered
* Using the Angular CLI
* Using Angular Material Components
* Building custom re-usable components
* Using Third Party Components (Ag Grid)
* Localiziation
* Using RxJS 6
* Referencing the Angular Style Guide
* Testing
* Error Handling
* Handling realtime streams

## Running the Application
1. Install `Node.js 8.9` or higher.
2. Install `Visual Studio 15.7` or higher
3. Install `.NET Framework 2.1`
4. Navigate to `src/Macaria.SPA/ClientApp` and run `npm install` to install app dependencies
5. Navigate to `src/Macaria.SPA/ClientApp` and run `ng serve` run the application using the Angular CLI
6. Navigate to `src/Macaria.API` and run `dotnet run ci` to install and seed the database
7. Navigate to `src/Macaria.API` and run `dotnet watch run` to run the application. Make a note of the url the api is listening on.
8. Open `src/Macaria.SPA/ClientApp/src/app/app.module.ts` and set the base url to the value from step 7.
9. Navigate to `src/Macaria.SPA` and run `dotnet watch run` and open the application to the url specificed in the console.

## High Level Architecture Decisions
