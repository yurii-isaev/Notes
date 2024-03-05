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
# dotnet ef migrations add News --context NewsDbContext --output-dir DataAccess/Migrations/NewsTables
# dotnet ef database update --context NewsDbContext
# dotnet ef migrations remove --context NewsDbContext -f

# dotnet ef migrations add Employee --context EmployeeDbContext --output-dir DataAccess/Migrations/EmployeeTables
# dotnet ef database update --context EmployeeDbContext -f
# dotnet ef migrations remove --context EmployeeDbContext -f
```

## Commands dotnet-ef for Add Column to News(migrations)
```
# Add Column to News table EmployeeDbContextModelSnapshot
# dotnet ef migrations add NewsAddColumn --context NewsDbContext --output-dir DataAccess/Migrations/NewsTables
# dotnet ef migrations remove --context EmployeeDbContext -f
# dotnet ef database update NewsAddColumn --context NewsDbContext --verbose
```

### Hot reload server
```
# dotnet watch -lp https
```
