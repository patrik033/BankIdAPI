# Build stage
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
EXPOSE 80
EXPOSE 443



# Copy the solution file and restore dependencies
COPY BankIdAPI.sln BankIdAPI.sln
COPY BankAPI/BankAPI.csproj BankAPI/BankAPI.csproj
COPY Contracts/Contracts.csproj Contracts/Contracts.csproj
COPY Entities/Entities.csproj Entities/Entities.csproj
COPY Repository/Repository.csproj Repository/Repository.csproj

RUN dotnet restore

# Copy the rest of the application and publish
COPY . .
WORKDIR /app/BankAPI
RUN dotnet publish -c Release -o /app/out


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:7.0
#For swagger to work
ENV ASPNETCORE_ENVIRONMENT=Development
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "BankAPI.dll"]