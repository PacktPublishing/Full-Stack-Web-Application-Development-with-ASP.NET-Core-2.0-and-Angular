mkdir src test

cd src

mkdir Macaria.Core Macaria.Infrastructure Macaria.API Macaria.SPA

cd Macaria.Core

dotnet new classlib -f netcoreapp2.1

cd ..\Macaria.Infrastructure

dotnet new classlib -f netcoreapp2.1

dotnet add reference ..\Macaria.Core\Macaria.Core.csproj

cd ..\Macaria.API

dotnet new webapi

dotnet add reference ..\Macaria.Core\Macaria.Core.csproj

dotnet add reference ..\Macaria.Infrastructure\Macaria.Infrastructure.csproj

cd ..\Macaria.SPA

dotnet new angular

cd ..\..\test

mkdir IntegrationTests UnitTests

cd IntegrationTests

dotnet new xunit

dotnet add reference ..\..\src\Macaria.Core\Macaria.Core.csproj

dotnet add reference ..\..\src\Macaria.Infrastructure\Macaria.Infrastructure.csproj

dotnet add reference ..\..\src\Macaria.API\Macaria.API.csproj

cd ..\UnitTests

dotnet new xunit

dotnet add reference ..\..\src\Macaria.Core\Macaria.Core.csproj

dotnet add reference ..\..\src\Macaria.Infrastructure\Macaria.Infrastructure.csproj

dotnet add reference ..\..\src\Macaria.API\Macaria.API.csproj

cd ..\..

dotnet new sln -n Macaria

dotnet sln add .\src\Macaria.Core\Macaria.Core.csproj .\src\Macaria.Infrastructure\Macaria.Infrastructure.csproj .\src\Macaria.API\Macaria.API.csproj .\src\Macaria.SPA\Macaria.SPA.csproj .\test\IntegrationTests\IntegrationTests.csproj .\test\UnitTests\UnitTests.csproj