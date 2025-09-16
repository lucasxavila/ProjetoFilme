FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ProjetoFilme/ProjetoFilme.csproj", "ProjetoFilme/"]
RUN dotnet restore "ProjetoFilme/ProjetoFilme.csproj"
COPY . .
WORKDIR "/src/ProjetoFilme"
RUN dotnet build "ProjetoFilme.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/runtime:8.0 AS final
WORKDIR /app
COPY --from=build /app/build .
ENTRYPOINT ["dotnet", "ProjetoFilme.dll"]