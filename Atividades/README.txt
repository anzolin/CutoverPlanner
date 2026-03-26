{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=cutover.db"
  }
}

dotnet ef migrations add InitialCreate -p src/Infrastructure/CutoverManager.Infrastructure.csproj -s src/Web/CutoverManager.Web.csproj
dotnet ef database update -p src/Infrastructure/CutoverManager.Infrastructure.csproj -s src/Web/CutoverManager.Web.csproj

dotnet ef database drop -p src/Infrastructure/CutoverManager.Infrastructure.csproj -s src/Web/CutoverManager.Web.csproj
dotnet ef database update -p src/Infrastructure/CutoverManager.Infrastructure.csproj -s src/Web/CutoverManager.Web.csproj

dotnet ef migrations add FixIdColumns -p src/Infrastructure/CutoverManager.Infrastructure.csproj -s src/Web/CutoverManager.Web.csproj
dotnet ef database update -p src/Infrastructure/CutoverManager.Infrastructure.csproj -s src/Web/CutoverManager.Web.csproj