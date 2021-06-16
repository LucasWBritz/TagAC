#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/TagAC.Apis.Identity/TagAC.Apis.Identity.csproj", "src/TagAC.Apis.Identity/"]
RUN dotnet restore "src/TagAC.Apis.Identity/TagAC.Apis.Identity.csproj"
COPY . .
WORKDIR "/src/src/TagAC.Apis.Identity"
RUN dotnet build "TagAC.Apis.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TagAC.Apis.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TagAC.Apis.Identity.dll"]