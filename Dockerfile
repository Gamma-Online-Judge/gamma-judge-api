FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

COPY . .

WORKDIR /app/src/Api

RUN dotnet restore 
RUN dotnet publish -c release -o /app/out --no-self-contained --no-restore

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
ENV LANG=en_US.UTF-8
ENV ASPNETCORE_URLS=http://*:5138
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5138
ENTRYPOINT ["dotnet", "Api.dll"]