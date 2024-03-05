# Projeto de APIs: API de Produtos e API de Pedidos

Este projeto consiste em duas APIs: uma para gerenciar produtos e outra para gerenciar pedidos. Abaixo estão as informações essenciais para entender e executar o projeto.

## Descrição
As APIs de produtos e pedidos permitem que os usuários realizem operações relacionadas a produtos e pedidos. A API de produtos permite adicionar, atualizar, listar e excluir produtos (operações CRUD). A API de pedidos permite criar e consultar pedidos.

## Arquitetura
Os projetos foram estruturados seguindo o Clean Architecture e utilizam as seguintes tecnologias:

- ASP.NET Core: Framework para desenvolvimento de APIs.
Entity Framework Core: Para interagir com o banco de dados.
Swagger: Documentação interativa das APIs.
- PostgreSQL: Banco de dados para armazenar informações de produtos e pedidos.
- RabbitMQ: Serviço de mensageria para processar pedidos de forma assíncrona (pagamento e envio).
- Entity Framework Core: Para interagir com o banco de dados PostgreSQL.
- Swagger: Documentação interativa das APIs.
- Docker: Para conteinerização dos serviços.

## Pré-requisitos
Para executar o projeto, é necessário ter instalado:
- .NET Core SDK
- PostgreSQL
- RabbitMQ
- Docker (engine e compose)

Opcionais:
- pgAdmin/Dbeaver, para navegar pelas tabelas do banco de dados.
- RabbitMQ Management Plugin, para gerenciar exchanges, routing keys, queues e enviar mensagens para testes.

## Executando o projeto com Docker Compose
- Clone este repositório.
- Abra o terminal na pasta raiz do projeto.
- Navegue até a pasta ```deploy/local``` e execute ```docker compose up -d```
- Caso seja feita alguma alteração em alguma API, rode o comando ```docker compose up -d --build users-api products-api orders-api``` para aplicar as mudanças e subir as APIs novamente

Quando todos os containers estiverem rodando, as APIs estarão disponíveis em:
- users-api: http://localhost:5154/swagger
- products-api: http://localhost:5155/swagger
- orders-api: http://localhost:5156/swagger

## Exemplos de requisições
TBD

## Licença
Este projeto está licenciado sob a MIT License.