
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 as base
ENV ASPNETCORE_ENVIRONMENT Production


FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore


# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out


# Build runtime image
FROM base as final
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Personnel.dll"]