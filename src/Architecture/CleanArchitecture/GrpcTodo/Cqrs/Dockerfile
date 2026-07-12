# .NET 10.0 Dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/Exp.Todo.Application/Exp.Todo.Application.csproj", "src/Exp.Todo.Application/"]
COPY ["src/Exp.Todo.Domain/Exp.Todo.Domain.csproj", "src/Exp.Todo.Domain/"]
COPY ["src/Exp.Todo.Infrastructure/Exp.Todo.Infrastructure.csproj", "src/Exp.Todo.Infrastructure/"]
COPY ["src/Exp.Todo.GrpcApi/Exp.Todo.GrpcApi.csproj", "src/Exp.Todo.GrpcApi/"]
RUN dotnet restore "src/Exp.Todo.GrpcApi/Exp.Todo.GrpcApi.csproj"
COPY . .
WORKDIR "/src/src/Exp.Todo.GrpcApi"
RUN dotnet build "Exp.Todo.GrpcApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Exp.Todo.GrpcApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Exp.Todo.GrpcApi.dll"]
