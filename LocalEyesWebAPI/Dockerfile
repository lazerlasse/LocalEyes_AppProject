#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001
ENV MySpecialSettings_DbPWD="tdi87269xsu+!"
ENV MySpecialSettings_SendGridKey="ap86vtbz5py9xqrhgcz76qh98233qn"
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LocalEyesWebAPI/LocalEyesWebAPI.csproj", "LocalEyesWebAPI/"]
RUN dotnet restore "LocalEyesWebAPI/LocalEyesWebAPI.csproj"
COPY . .
WORKDIR "/src/LocalEyesWebAPI"
RUN dotnet build "LocalEyesWebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LocalEyesWebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocalEyesWebAPI.dll"]