FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
COPY *.csproj ./
RUN dotnet restore 
COPY . ./
RUN dotnet publish FinanceControl.csproj -c Release -o out

EXPOSE 3001
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT [ "dotnet", "FinanceControl.dll" ]