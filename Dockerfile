FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY ./bin/Release/net10.0/publish .
ENTRYPOINT ["dotnet", "./InnoShop.UserService.dll"]
