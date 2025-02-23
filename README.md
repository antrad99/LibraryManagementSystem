# Library Management System
* .NET 9.0 Blazor Web App project in auto render mode per page/component
* In-memory database using Entity Framework Core 
* Open the solution in VS 2022, set the LibraryManagementSystem Project as start up project, run the app.
* To test the APIs separately use the HttpFiles in Visual Studio
* The Unit Tests are in a separate XUnitTests project inside the same solution. 
* PLEASE NOTE: the table for displaying the list of the books is not responsive (it was not in the requirements), it needs some attention if the application is planning to target mobile devices.

# Scalability
* The project is done using .net 9 Blazor Web App in auto rendering mode, so first it runs on the server for fast reply to the users, then it will run on the Web Browser of the user, saving resources (web socket) from server. 
If it requires more resources due to the increased number of users, it can be hosted in a Cloud platform such as Azure, where the App Service and SQL Server can be assigned to more powerful resources to fit the increased demand without changing the architecture.

# Performance
* If the web app is loading very slowly, look first at what percentage of the CPU and RAM is in use for the App Service and SQL Server, if it is already 100% then it needs to be assigned to more powerful resources.
* Look whether the App is calling an external API and whether it is waiting for its reply.
* Look whether the App is making any call to the database and whether the query is very slow in retrieving the data, this could be because the query needs to be optimised, for example by adding a new index in the table. 

# Security
* Currently the APIs are not protected, this could be done by adding the ASP.NET Identity framework to the App and [Authorize] to the APIs.
* One common vulnerability is when the App Secrets (for example: the Connection String of the database) are saved in the app.settings file. One solution is to save them using Manage User Secrets for local development and save them in the App Settings of the App Service or in the Vault for UAT and Production, so they are not exposed in case someone gains access to the ftp folder.
* Another vulnerability is the Cross-site Request Forgery which is fundamentally a problem with the web apps in general not only for .net. The app can be protected by this attack by adding the Antiforgery middleware to the Dependency injection container.