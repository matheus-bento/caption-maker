# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
ARG BASE_FILE_PATH
USER root
RUN mkdir -p $BASE_FILE_PATH
RUN chown $APP_UID $BASE_FILE_PATH
USER $APP_UID
WORKDIR /app
EXPOSE 80

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CaptionMaker.Files/CaptionMaker.Files.csproj", "CaptionMaker.Files/"]
RUN dotnet restore "./CaptionMaker.Files/CaptionMaker.Files.csproj"
COPY . .
WORKDIR "/src/CaptionMaker.Files"
RUN dotnet build "./CaptionMaker.Files.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CaptionMaker.Files.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaptionMaker.Files.dll"]