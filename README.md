# Projeto de APIs: API de Produtos e API de Pedidos

Este projeto consiste em duas APIs: uma para gerenciar produtos e outra para gerenciar pedidos. Abaixo estão as informações essenciais para entender e executar o projeto.

## Descrição
As APIs de produtos e pedidos permitem que os usuários realizem operações relacionadas a produtos e pedidos. A API de produtos permite adicionar, atualizar, listar e excluir produtos (operações CRUD). A API de pedidos permite criar e consultar pedidos.

## Arquitetura
O projeto segue uma arquitetura RESTful e utiliza as seguintes tecnologias:

- ASP.NET Core: Framework para desenvolvimento de APIs.
Entity Framework Core: Para interagir com o banco de dados.
Swagger: Documentação interativa das APIs.
- SQLite: Banco de dados para armazenar informações de produtos e pedidos.
- RabbitMQ: Serviço de mensageria para processar pedidos de forma assíncrona (pagamento e envio).
- Entity Framework Core: Para interagir com o banco de dados SQLite.
- Swagger: Documentação interativa das APIs.
- Docker: Para conteinerização dos serviços.

## Pré-requisitos
Para executar o projeto, é necessário ter instalado:
- .NET Core SDK
- SQLite
- RabbitMQ
- Docker (engine e compose)

Opcionais:
- SQLite Browser, para navegar pelas tabelas do banco de dados
- RabbitMQ Management Plugin, para gerenciar exchanges, routing keys, queues e enviar mensagens para testes.

## Executando o projeto
- Clone este repositório.
- Abra o terminal na pasta raiz do projeto.
- Execute o seguinte comando para restaurar as dependências:
```dotnet restore```
- Configure a conexão com o banco de dados no arquivo appsettings.json.
- Execute as migrações para criar o banco de dados:
dotnet ef database update
- Inicie o projeto: ```dotnet run```

Quando a aplicação estiver rodando, acesse a documentação das APIs em http://localhost:5000/swagger.

## Contribuição
Sinta-se à vontade para contribuir com melhorias, correções de bugs ou novos recursos. Abra uma issue ou envie um pull request!

## Licença
Este projeto está licenciado sob a MIT License.