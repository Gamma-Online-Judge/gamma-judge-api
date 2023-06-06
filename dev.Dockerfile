FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . .

RUN dotnet restore 

EXPOSE 5138
ENTRYPOINT ["dotnet", "dotnet", "run", "--project", "src/Api/Api.csproj"]