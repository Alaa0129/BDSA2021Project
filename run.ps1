
# $mainShell = Start-Process powershell -PassThru

# $apiShell = Start-Process powershell -PassThru;

# Register-ObjectEvent -InputObject $mainShell -EventName exited -SourceIdentifier ggg -Action {Stop-Process $apiShell.Id};


start powershell -windowstyle minimized {cd .\BlazorApp.Api; dotnet run; Read-Host}

start powershell -windowstyle minimized {cd .\BlazorApp; dotnet run; Read-Host}

Start-Sleep -Seconds 1.5

start chrome https://localhost:5001