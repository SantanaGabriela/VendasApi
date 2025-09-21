# 🛒 Projeto de Microserviços - Vendas & Estoque

Este projeto é composto por duas APIs (`VendasApi` e `EstoqueApi`) que se comunicam via **RabbitMQ** para gerenciar vendas e atualizar o estoque automaticamente.

- 🔗 VendasApi: https://github.com/SantanaGabriela/VendasApi  
- 🔗 EstoqueApi: https://github.com/SantanaGabriela/EstoqueApi

---

## 🚀 Tecnologias utilizadas
- **.NET 8**
- **Entity Framework Core**
- **SQL Server**
- **JWT (JSON Web Token)** para autenticação
- **Swagger / Swashbuckle** para documentação
- **RabbitMQ** (mensageria)
- **Docker Desktop** (para rodar containers locais)

---

## 📋 Pré-requisitos (local)
- [.NET SDK 8.x](https://dotnet.microsoft.com/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [SQL Server] (local ou container)
- Git

---

## 📦 Pacotes NuGet (VendasApi)
Conforme o `VendasApi.csproj`, os pacotes utilizados são:

- `Microsoft.AspNetCore.Authentication.JwtBearer` (8.0.20) — autenticação JWT  
- `Microsoft.EntityFrameworkCore.Design` (9.0.8) — design-time tools EF Core  
- `Microsoft.EntityFrameworkCore.SqlServer` (9.0.8) — provider SQL Server  
- `Microsoft.EntityFrameworkCore.Tools` (9.0.8) — migrations / tools  
- `Microsoft.VisualStudio.Azure.Containers.Tools.Targets` (1.22.1) — suporte containers no VS  
- `RabbitMQ.Client` (7.1.2) — cliente RabbitMQ  
- `Swashbuckle.AspNetCore` (6.6.2) — Swagger  
- `System.IdentityModel.Tokens.Jwt` (8.14.0) — manipulação tokens JWT

> **Obs:** Verifique o `EstoqueApi.csproj` e adicione/ajuste pacotes no `EstoqueApi` conforme necessário (ex.: `RabbitMQ.Client`, EF Core, Swagger, JWT, etc). Se o `EstoqueApi` não possuir `RabbitMQ.Client`, adicione com:
> ```bash
> cd EstoqueApi
> dotnet add package RabbitMQ.Client
> ```

---

## ⚙️ Configuração do ambiente

### 1️⃣ Subir o RabbitMQ (Docker)
Execute no terminal:
```bash
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4-management
```

- Painel de gerenciamento: http://localhost:15672  
- **Usuário:** `guest`  
- **Senha:** `guest`

---

### 2️⃣ Banco de dados (SQL Server)
Configure o SQL Server localmente ou via container e ajuste a *connection string* em cada `appsettings.json`.

Exemplo de `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=VendasDb;User Id=sa;Password=SuaSenhaAqui;TrustServerCertificate=True;"
}
```

Para aplicar migrações e criar o banco:
```bash
cd VendasApi
dotnet ef database update

# se houver migrações também no EstoqueApi:
cd ../EstoqueApi
dotnet ef database update
```

> Se você ainda não criou migrações, crie com:
> ```bash
> dotnet ef migrations add InitialCreate
> dotnet ef database update
> ```

---

### 3️⃣ Configuração JWT
Configure a chave secreta no `appsettings.json`:
```json
"Jwt": {
  "Key": "sua-chave-super-secreta",
  "Issuer": "suaaplicacao",
  "Audience": "suaaplicacao"
}
```
> Em produção use variáveis de ambiente / secret manager — **não** deixe chaves hardcoded.

---

## ▶️ Como executar (passo-a-passo)

1. Clone os repositórios:
```bash
git clone https://github.com/SantanaGabriela/VendasApi.git
git clone https://github.com/SantanaGabriela/EstoqueApi.git
```

2. Restaurar pacotes:
```bash
cd VendasApi
dotnet restore

cd ../EstoqueApi
dotnet restore
```

3. Subir RabbitMQ (ver passo 1) e garantir que o SQL Server esteja rodando.

4. Rodar as APIs **na ordem** (importante):
- Primeiro: **VendasApi**
  ```bash
  cd VendasApi
  dotnet run
  ```
- Depois: **EstoqueApi**
  ```bash
  cd ../EstoqueApi
  dotnet run
  ```

5. Acesse o Swagger (ajuste portas conforme `launchSettings.json` / configuração local):
- VendasApi → `http://localhost:5000/swagger` (ou porta definida no projeto)  
- EstoqueApi → `http://localhost:5001/swagger` (ou porta definida no projeto)

---

## 🧩 Como funciona (visão geral)
1. Usuário cria uma venda via `VendasApi`.  
2. `VendasApi` grava a venda no banco e publica uma **mensagem** no **RabbitMQ** (ex.: fila `vendas.executadas`).  
3. `EstoqueApi` consome a mensagem, processa e atualiza o estoque no banco.  
4. Ambas as APIs usam JWT para autenticação e Swagger para documentação.

---

## 🔧 Dicas e observações
- Ajuste portas e URLs em `launchSettings.json` se já houver serviços usando as portas padrão.  
- Se usar SQL Server local com certificado, pode ser necessário `TrustServerCertificate=True` para desenvolvimento.  
- Se houver problemas de conexão com RabbitMQ, verifique se o Docker Desktop está ativo e se as portas 5672/15672 não estão bloqueadas.  
- Em desenvolvimento, use `appsettings.Development.json` para valores locais e `appsettings.Production.json` / variáveis de ambiente para produção.  
- Em produção, nunca expor `guest/guest` do RabbitMQ — crie usuário/senha específicos e ajuste as permissões.

---

## 🐞 Troubleshooting rápido
- **`dotnet ef` não encontrado:** instale o Entity Framework tools:
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- **Erro de conexão com SQL Server:** verifique `Server`, `User Id`, `Password`, e se a instância do SQL está ativa.  
- **RabbitMQ não acessível:** confirme `docker ps` e se o container está rodando; veja logs do container.

---

## ♻️ Como adicionar um pacote (exemplo)
Para adicionar pacotes NuGet manualmente:
```bash
cd VendasApi
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package RabbitMQ.Client
```

---

## ✅ Resumo rápido dos comandos principais
```bash
# Clonar
git clone https://github.com/SantanaGabriela/VendasApi.git
git clone https://github.com/SantanaGabriela/EstoqueApi.git

# Restaurar
cd VendasApi
dotnet restore
cd ../EstoqueApi
dotnet restore

# Subir RabbitMQ
docker run -it --rm --name rabbitmq -p 5672:5672 -p 15672:15672 rabbitmq:4-management

# Rodar APIs (ordem)
cd VendasApi
dotnet run

cd ../EstoqueApi
dotnet run
```
