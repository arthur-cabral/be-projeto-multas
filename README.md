
# be-projeto-multas

Esse repositório tem como objetivo armazenar o código do backend de um projeto relacionado a multas.



## Variáveis de Ambiente

Crie um arquivo chamado appsettings.json na pasta `/API` do projeto e cole o seguinte trecho de código

```bash
  {
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=db-multas;User Id=SA;Password=DbPassword123;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "JWT": {
    "ValidAudience": "https://localhost:7174",
    "ValidIssuer": "https://localhost:7174",
    "Key": "a9f147aa-6bc8-4845-aadf-efca0c9dc9cc",
    "TokenValidityInMinutes": 120,
    "RefreshTokenValidityInMinutes": 60
  }
}
```
## Requisitos

 - .NET 8
 - Visual Studio
 - Docker
 - .NET EF Cli
## Instalação

Inicialmente clone o código do repositório 

```bash
  git clone https://github.com/arthur-cabral/be-projeto-multas
```

Rode o comando abaixo para inicializar o banco sql server

```bash
  docker run -d -p 1433:1433 --name container-sql-server -e SA_PASSWORD=DbPassword123 -e ACCEPT_EULA=Y -e MSSQL_PID=Express -e MSSQL_USER=SA -e MSSQL_PASSWORD=DbPassword123  mcr.microsoft.com/mssql/server
```

Entre na pasta `/API` do projeto e aplique as migrações por meio dos comando abaixo

```bash
  cd API
```

```bash
  dotnet ef database update
```
## Rodando localmente

Entre no diretório do projeto

```bash
  cd API
```

Instale as dependências

```bash
  dotnet restore
```

Inicie o servidor

```bash
  dotnet run --launch-profile https
```
