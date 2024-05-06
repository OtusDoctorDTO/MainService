FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /MainService
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM  mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /MainService
COPY --from=base /MainService/out ./
EXPOSE 8080
ENTRYPOINT [ "dotnet", "MainServiceWebApi.dll", "--environment=Test" ]
