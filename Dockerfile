# Use official .NET image as base
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5288

# Use SDK image to build the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy only necessary project files first
COPY ["gift_card_csharp_postgres.csproj", "./"]
RUN dotnet restore "./gift_card_csharp_postgres.csproj"

# Copy all other files **EXCEPT Dockerfile**
COPY . .   
RUN rm -f Dockerfile  # <-- 🚀 Prevents Dockerfile from being copied to the project folder

# Build and publish the project
RUN dotnet build "./gift_card_csharp_postgres.csproj" -c Release -o /app/build
RUN dotnet publish "./gift_card_csharp_postgres.csproj" -c Release -o /app/publish

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "gift_card_csharp_postgres.dll"]