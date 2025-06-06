FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER root
RUN apt-get update
RUN apt-get install procps -y
RUN apt-get install wget -y && apt-get install curl -y
RUN mkdir /root/.vs-debugger
RUN curl -sSL https://aka.ms/getvsdbgsh -o /root/.vs-debugger/GetVsDbg.sh && \
    bash /root/.vs-debugger/GetVsDbg.sh -v latest -l /vsdbg
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Chat.API/Chat.API.csproj", "Chat.API/"]
COPY ["Chat.Contracts/Chat.Contracts.csproj", "Chat.Contracts/"]
COPY ["Chat.Infrastructure/Chat.Infrastructure.csproj", "Chat.Infrastructure/"]
COPY ["Chat.Domain/Chat.Domain.csproj", "Chat.Domain/"]
COPY ["Chat.Common/Chat.Common.csproj", "Chat.Common/"]
COPY ["Chat.Application/Chat.Application.csproj", "Chat.Application/"]
RUN dotnet restore "./Chat.API/Chat.API.csproj"
COPY . .
WORKDIR "/src/Chat.API"
RUN dotnet build "./Chat.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Debug
RUN dotnet publish "./Chat.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Chat.API.dll"]