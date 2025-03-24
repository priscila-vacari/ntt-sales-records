# Projeto - API Restful de Vendas

## 📋 Visão Geral
Esta é uma API Restful desenvolvida em .NET 8, utilizando os padrões do DDD (Domain-Driven Design) para referenciar entidades de outros domínios por meio do padrão External Identities e desnormalização de descrições de entidades.

A API gerencia registros de vendas e implementa as seguintes tecnologias e conceitos:

- Mediator Pattern

- Banco de Dados PostgreSQL

- Publicação de eventos com Rebus

- Boas práticas: SOLID, Clean Code, DRY, YAGNI, Object Calisthenics

- Testes unitários com xUnit, NSubstitute e Bogus


## 🚀 Tecnologias Utilizadas
- **.NET 8:** Framework principal para a aplicação.

- **Entity Framework Core:** ORM para manipulação do banco de dados.

- **Serilog:** Sistema de logs estruturados.

- **AutoMapper**

- **FluentValidation**

- **Swagger**

- **Rebus** (mensageria) [disabled]

- **SQL Server:** Banco de dados relacional.

- **XUnit, Bogus e NSubstitute:** Validação de dados e testes unitários.

- **Rate Limiting:** Prevenção de ataques de força bruta.

- **Git Flow e Commit semântico**.


## 📌 Arquitetura
A aplicação segue um padrão de divisão em camadas:

- API: Camada de apresentação (Controllers, Middlewares)

- Application: Regras de negócio, handlers, factories e strategies.

- Domain: Entidades

- Infra: Repositórios e acesso a dados


## 🎯 Requisitos Técnicos

1. Framework: **.NET 8**

2. Padrão de arquitetura: Divisão em camadas

3. Registro de logs: **Utilizar Serilog**

4. Banco de dados: SQL Server, podendo ser ajustado para PostgreSQL

5. Controle de versão: **Aplicar Git Flow e Commit semântico**

6. Boas práticas: **Clean Code, SOLID, DRY, YAGNI e Object Calisthenics**

7. APIs REST:

- Seguir boas práticas de **RESTful**

- Utilizar **respostas HTTP apropriadas** (200, 201, 400, 404, etc.)

8. Testes unitários com xUnit, NSubstitute e Bogus.


## ✅ Requisitos funcionais

**Funcionalidades**

A API permite a gestão de vendas com os seguintes campos:

- Número da venda

- Data da venda

- Cliente

- Valor total da venda

- Filial onde a venda foi realizada

- Lista de produtos

   - Quantidades

   - Preços unitários

   - Descontos aplicados

   - Valor total de cada item

- Status de cancelamento (Cancelado/Ativo)

**Publicação de Eventos**

Cada operação na venda dispara eventos registrados no log:

SaleCreated - Venda criada

SaleModified - Venda modificada

SaleCancelled - Venda cancelada

ItemCancelled - Item cancelado

**Regras de Negócio**

A API implementa as seguintes regras para cálculo de desconto e restrições:

1. Compras acima de 4 itens idênticos recebem 10% de desconto.

2. Compras entre 10 e 20 itens idênticos recebem 20% de desconto.

3. Não é permitido vender mais de 20 itens idênticos.

4. Compras abaixo de 4 itens não podem ter desconto.


## 📂 Estrutura do Projeto
Abaixo está a organização principal do projeto:
```
/src
  /Sales.API
    /logs
        - log-api-YYYYMMDD.txt
    /controllers
        /v1
            - SaleController.cs
  /Sales.Application
    /interfaces
        - IDiscountFactory.cs
        - IDiscountStrategy.cs
    /factories
        - DiscountFactory.cs
    /handlers
        - CreateSaleHandler.cs
        - DeleteSaleHandler.cs
        - GetAllSalesHandler.cs
        - GetSaleByIdHandler.cs
        - UpdateSaleHandler.cs
    /strategies
        - DiscountStrategy.cs
    - ApplicationDependencyRegister.cs
  /Sales.Domain
    /entities
        - Sale.cs
        - SaleItem.cs
  /Sales.Infra
    /interfaces
        - IRepository.cs
        - ISaleRepository.cs
    /repositories
        - Repository.cs
        - SaleRepository.cs
    - InfraDependencyRegister.cs
  /tests
    /api
        /controllers
            - SaleControllerTests.cs
    /application
        /handlers
            - CreateSaleHandlerTests.cs
            - DeleteSaleHandlerTests.cs
            - GetAllSalesHandlerTests.cs
            - GetSaleByIdHandlerTests.cs
            - UpdateSaleHandlerTests.cs
    /infra
        - RepositoryTests.cs   
  /doc
    Sales.postman_collection.json
Sales.sln
readme.md
```

## 🔧 Configuração e Execução Local
Siga as etapas abaixo para configurar e executar a aplicação localmente:

**1. Pré-requisitos**

Certifique-se de que você possui os seguintes itens instalados:

- **SDK do .NET 8:** [Baixar aqui](https://dotnet.microsoft.com/pt-br/download)

- **SQL Server** (ou outro banco compatível configurado no `appsettings.json`).

- Um editor de texto, como **Visual Studio** ou **Visual Studio Code**.

**2. Clonar o Repositório**

Clone este repositório para sua máquina local:

```
bash
git clone https://github.com/priscila-vacari/ntt-sales-records.git
cd ntt-sales-records
```

**3. Configurar o Banco de Dados**

1. Certifique-se de que seu banco de dados SQL Server está configurado e rodando.

2. Atualize o arquivo `appsettings.json` com a string de conexão:
```
json
"ConnectionStrings": {
  "SalesConnection": "Server=SEU_SERVIDOR;Database=SEU_BANCO;User Id=SEU_USUARIO;Password=SUA_SENHA;Encrypt=False;Pooling=true;"
}
```

3. Execute as migrações para preparar o banco de dados:

```
bash
dotnet ef database update --project src\Sales.Infra --startup-project src\Sales.API
```

**4. Rodar o Projeto**

1. Compile e execute a aplicação:
```
bash
dotnet build
dotnet run
```

2.  Acesse a aplicação em:

API: https://localhost:7057/api

Worker: Logs serão exibidos no terminal e no diretório `/logs`.

**5. Testar os Endpoints**

Use **Postman** ou **cURL** para testar os endpoints.

[collection postman](https://github.com/priscila-vacari/ntt-sales-records/blob/main/doc/Sales.postman_collection.json)

## 🛡️ Boas Práticas e Padrões

**1. Logs Estruturados:**

- Todos os eventos importantes são registrados nos arquivos de log.

- Automaticamente será salvo o `correlation_id` correspondente à requisição, a fim de rastrear o fluxo por diversas partes do sistema, facilitando a depuração e o monitoramento de problemas.

- Logs disponíveis em `/logs/`.

**2. Segurança:**

- Proteção contra força bruta (rate limiting) aplicada globalmente.

**3. Validação:**

- Todas as entradas do usuário são validadas com o FluentValidation para garantir consistência e segurança.

**4. Factory Pattern:**

- Encapsulada a lógica de cálculo dos descontos para evitar acoplamento direto e facilitando as expansões futuras sem impactar o código já existente e facilitando a manutenção.

- Melhora a testabilidade e garante mais segurança e controle.

**5. Strategy Pattern:**

- Desacoplamento da lógica de cálculo de desconto onde as regras podem mudar com o tempo, preparando o código para futuras alterações sem impacto significativo.

- Código mais limpo e organizado.

- Mais flexibilidade e melhora na testabilidade.


## 🧪 Testes

O projeto inclui testes unitários para todas as funcionalidades principais. 
Os testes são implementados com xUnit, NSubstitute e Bogus.
Execute os testes com:

```
bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator "-reports:TestResults\**\*.cobertura.xml" "-targetdir:coveragereport" "-reporttypes:Html"
```

## 🛠️ Ferramentas Adicionais

**Swagger UI:**

- Acesse `https://localhost:7057/swagger/index.html` para explorar e testar as APIs interativamente.

**Serilog Dashboard (opcional):**

- Integre visualizações de logs com ferramentas como Seq ou Kibana para uma análise avançada.

## ❗️ Pendências

1. Implementar autenticação e autorização JWT (JSON Web Token).
2. Implementar containerização.
3. Alterar conexão para PostgreSQL
4. Criar um nuget package de conexão com banco de dados.
5. Aumentar cobertura de código.
6. Implementar uso de filas como RabbitMQ para melhorar ainda mais a escalabilidade no recebimento das requisições de vendas e disparos de eventos juntamente com a criação de workers de processamento.


## 📜 Licença

N/A.