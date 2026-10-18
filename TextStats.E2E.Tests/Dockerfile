FROM mcr.microsoft.com/playwright/dotnet:v1.63.0-noble

WORKDIR /tests

COPY TextStats.E2E.Tests.csproj ./

RUN dotnet restore

COPY . .

CMD ["dotnet", "test", "--no-restore"]