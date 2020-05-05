# this goes to Microsoft's own Docker Repository to get the runtime on some version of Linux (not sure which one!)
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
COPY bin/Release/netcoreapp3.1/publish/ App/
WORKDIR /App
ENTRYPOINT ["dotnet", "ConstellationWebApp.dll"]
EXPOSE 80