cd .\BlazorApp.Infrastructure\

ECHO "|-----------------|" "|Dropping Database|" "|-----------------|"
ECHO y | dotnet ef database drop -s ..\BlazorApp.Api\

ECHO "|-----------------------|" "|Removing last migration|" "|-----------------------|"
dotnet ef migrations remove -s ..\BlazorApp.Api\

ECHO "|--------------------|" "|Adding new migration|" "|--------------------|"
$name = New-Guid
dotnet ef migrations add $name -s ..\BlazorApp.Api\

ECHO "|-----------------|" "|Updating Database|" "|-----------------|"
dotnet ef database update -s ..\BlazorApp.Api\
cd ..