# BDSA2021Project IPhone4

## Introduction

Hello and welcome to our project bank application. This application allows users (Students and Supervisors) to log-in using Azure Active Directory with their credentials (An invite to join our directory should've been sent to your ITU emails). Upon the first log-in a Guid representing the user as their ID will be stored in the local database, along with their name, for use in the application. Depending on which roles you are delecated in AAD you level of access/features will be different.
A user without any roles will only have access to the homepage meaning that they can browse the projects in the bank.
A user of the student role has the ability to browse the projects in the bank, apply for a project, create a request, browse their sent requests and change their profile name.
A user of the supervisor role has the ability to browse the projects in the bank, create a new project, browse the requests they have received, create a project from the information in a received request and change their profile name.

We have assigned you to both roles to showcase all the features, but keep in mind that the product is made with the idea that a user is only asigned to one or the other role. Below are provided some test users which you can use to log-in to get the true experience for how the finalized product will function or if you are having troubles logging-in with your credentials.

1. Student Tester

- Email: Student@iphone4projectbank.onmicrosoft.com
- Password: 15febe9e-b1b8-420a-b778-2afd6394ae7e

2. Supervisor Tester

- Email: Supervisor@iphone4projectbank.onmicrosoft.com
- Password: 378531bf-ed83-458d-bc87-d87667469232

## Explanation of Select Features

We strived to create features that were easy to use and self-explanatory in how to use them and we feel that most of the features in the finalized product are just that. This means that it is unnecessary to explicitly explain how to use most of the features like, create a project/request, searching, changing display name, etc. There are, however, two features that we didn't quite have the time to fully create the optimal user experience, resulting in them being not totally intuitive or including some bugs. These two features are that of the tagging of projects and sorting projects by tags. We will therefore now explain how to use these and their shortcomings.

1. Tagging a project

Projects have the ability to have various tags applied to them, making it easier for students to search the Project Bank and find projects that interest them. To apply tags to a project, which is done during the creation of said project, a supervisor needs to first write the name of the tag in the text field, then click the blue "Add Tag" button, and then finally click on the newly added blue button with the name of the written tag on it such that it turns black. If the supervisor has changed their mind they can click on the now black button, such that it turns blue again to remove the tag. When the supervisor is satisfied with the tags and information of the project he can click "Submit" to finalize the project and add it to the bank. This will redirect him to the homepage. If the tag he applied to the project was never before used in any project, it will now have been added to the tag filtering options displayed at the top of the homepage.

2. Filtering project by tag

The filtering of projects by tag was created such that it would be easier for students to sort the projects in the bank by their preferences. This is especially useful when there are lots of projects in the bank. In the current state of the product, if a user wants to sort the projects on the homepage by a tag they have to click on one of the blue tags in the tag filtering options at the top of the page. This will change the color to black and the displayed projects will now all include the selected tag. However, there are some bugs; re-clicking a tag will not remove the filtering meaning that if a user wants to reset their filtering they have to refresh the page. Furthermore, it doesn't currently support multi-filtering, meaning that if a user were to select two tags it will not show projects with either one of the two tags or both but instead just filter by the last clicked tag.

## Set-up

As our application uses Docker and EntityFramework these two tools are required before use.

1. Before running the application we would like you to setup and run an SQL server in container using the following commands

```powershell
$password = New-Guid
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=$password" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
$database = "PBank"
$connectionString = "Server=localhost;Database=$database;User Id=sa;Password=$password"
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

A Chrome Browser will open on the localhost with port 5001. It might have some trouble loading the first time as the application needs a few seconds to start up, meaning a refresh of the page might be needed.
