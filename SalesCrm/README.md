## Commands dotnet-ef for first initial
```
# dotnet tool update --global dotnet-ef --version 7.0.0
# dotnet ef migrations add Initial
# dotnet ef database update
# dotnet ef migrations remove
# dotnet ef migrations add Auth --context AuthDbContext --output-dir DataAccess/Migrations/AuthDb
# dotnet ef database update --context AuthDbContext
# dotnet ef migrations remove --context AuthDbContext
```

## Commands dotnet-ef for add data sets
```
# dotnet ef migrations add News --context NewsDbContext
# dotnet ef database update --context NewsDbContext
```

### Hot reload server
```
# dotnet watch -lp https
```
