FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Gestor_Projetos_Tarefas.Api/Gestor_Projetos_Tarefas.Api.csproj", "./Gestor_Projetos_Tarefas.Api/"]
COPY ["Gestor_Projetos_Tarefas.Database/Gestor_Projetos_Tarefas.Database.csproj", "./Gestor_Projetos_Tarefas.Database/"]
COPY ["Gestor_Projetos_Tarefas.Domain/Gestor_Projetos_Tarefas.Domain.csproj", "./Gestor_Projetos_Tarefas.Domain/"]
RUN dotnet restore "./Gestor_Projetos_Tarefas.Api/Gestor_Projetos_Tarefas.Api.csproj"
COPY . .
WORKDIR "/src/Gestor_Projetos_Tarefas.Api"
RUN dotnet build "./Gestor_Projetos_Tarefas.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Gestor_Projetos_Tarefas.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gestor_Projetos_Tarefas.Api.dll"]