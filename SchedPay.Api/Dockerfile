# build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY SchedPay.Domain/SchedPay.Domain.csproj SchedPay.Domain/
COPY SchedPay.Application/SchedPay.Application.csproj SchedPay.Application/
COPY SchedPay.Api/SchedPay.Api.csproj SchedPay.Api/

RUN dotnet restore SchedPay.Api/SchedPay.Api.csproj

COPY . .
RUN dotnet publish SchedPay.Api/SchedPay.Api.csproj -c Release -o /app/publish

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:5100
EXPOSE 5100
ENTRYPOINT ["dotnet", "SchedPay.Api.dll"]
