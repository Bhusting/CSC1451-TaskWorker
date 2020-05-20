#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CSC1451-TaskWorker.csproj", "CSC1451-TaskWorker/"]
COPY ["../Domain/Domain.csproj", "Domain/"]
RUN dotnet restore "CSC1451-TaskWorker.csproj"
COPY . .
WORKDIR "/src/CSC1451-TaskWorker"
RUN dotnet build "CSC1451-TaskWorker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CSC1451-TaskWorker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CSC1451-TaskWorker.dll"]