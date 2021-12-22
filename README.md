# BDSA2021Project IPhone4

## Introduction

Hello and welcome to our project bank application. This application allows users (Students and Supervisors) to log-in using Azure Active Directory with there credentials. Upon the first log-in their information will be stored in the local database for use in the application. Depending on which roles you are delecated in AAD you level of access/features are different.
A user without any roles will only have access to browse the projects.
A user of the student role has the ability to browse the projects in the bank, apply for a project, create a request, browse their sent requests and change their profile name.
A user of the supervisor role has the ability to browse the projects in the bank, create a new project, browse the requests they have received, create a project from the information in a received request and change their profile name.

We have assigned you to both roles to showcase all the features, but keep in mind that the product is made with the idea that a user is only asigned to one or the other role. Below are provided some test users which you can use to log-in to get the true experience for how the finalized product show function or if you are having troubles logging-in.

1. Student Tester

- Email: Student@iphone4projectbank.onmicrosoft.com
- Password: 15febe9e-b1b8-420a-b778-2afd6394ae7e

2. Supervisor Tester

- Email: Supervisor@iphone4projectbank.onmicrosoft.com
- Password: 378531bf-ed83-458d-bc87-d87667469232

## Set-up

1. Before running the application we would like you to setup and run an SQL server in container using the following commands

```powershell
$ password = New-Guid
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$ database = "PBank"
$ connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"
```

2. And then add the user-secrets to the BlazorApp.API using the following commands

```powershell
cd BlazorApp.API
dotnet user-secrets set "ConnectionStrings:PBank" "$connectionString"
```

3. And finally add a migration using the database.ps1 script in the project root folder and run the application using run.ps1:

```powershell
database.ps1
run.ps1
```
