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

## Exemplos de algumas requisições
- Registrar um usuário: ```POST /api/users```
    - Requisição:
        ```
        {
            "firstName": "Nome",
            "lastName": "Sobrenome",
            "email": "user@mail.com",
            "password": "senha123"
        }
        ```
    - Resposta (sucesso):
        ```
        {
            "firstName": "Nome",
            "lastName": "Sobrenome",
            "email": "user@mail.com",
            "password": "senha123"
        }
        ```
- Logar um usuário: ```POST /api/users/login```
    - Requisição:
        ```
        {
            "email": "user@mail.com",
            "password": "senha123"
        }
        ```
    - Resposta (sucesso): token de autenticação (JWT)
- Registrar um produto: ```POST /api/products```
    - Requisição:
        ```
        {
            "name": "Mouse",
            "description": "Sem fio e com RGB!",
            "quantity": 121,
            "price": 255.99
        }
        ```
    - Resposta:
        ```
        {
            "id": 1
        }
        ```
- Registrar um pedido: ```POST /api/orders```
    - Requisição:
        ```
        {
            "productId": 1,
            "quantity": 3
        }
        ```
    - Resposta:
        ```
        {
            "id": 1
        }
        ```
- Listar todos os produtos: ```GET /api/products```
    - Resposta:
        ```
        [
            {
                "id": 1,
                "name": "Mouse",
                "description": "Sem fio e com RGB!",
                "quantity": 121
                "price": 255.99
            },
            {
                "id": 2,
                "name": "Teclado",
                "description": "Com fio e sem RGB...",
                "quantity": 2
                "price": 15.00
            }
        ]
        ```
- Listar todos os pedidos do usuário logado: ```GET /api/orders```
    - Resposta:
        ```
        [
            {
                "id": 1,
                "productId": 1,
                "userId": "fef659d3-6946-40bd-aaf8-52df14497249",
                "productSnapshot": {
                    "id": 1,
                    "name": "Mouse",
                    "price": 225.99
                },
                "quantity": 3,
                "status": 4,
                "createdAt": "2024-03-01T20:32:21.385205Z"
            },
            {
                "id": 1,
                "productId": 1,
                "userId": "fef659d3-6946-40bd-aaf8-52df14497249",
                "productSnapshot": {
                    "id": 2,
                    "name": "Teclado",
                    "price": 15.00
                },
                "quantity": 2,
                "status": 4,
                "createdAt": "2024-03-02T21:11:48.659874Z"
            }
        ]
        ```

## Fluxo de processamento de um novo pedido
Ao receber um pedido, o serviço de pedidos (`orders-api`) valida se o produto existe e se há quantidade suficiente do mesmo para o pedido. Em caso de sucesso, o pedido é gravado no banco e uma mensagem do tipo `order-created` é postada na fila `orders-created`. O mesmo serviço consome essa mensagem e processa os próximos passos, atualizando o status de `Criado` para `AwaitingPayment`. Na sequência, o status muda para `PaymentConfirmed`, e por fim muda para `AwaitingShipping`. Para testar o resto do fluxo, é preciso enviar mensagens manualmente para a fila `order-shipping-status-changed` para simular mudanças no status do envio. Os status possíveis são: `Shipped`, `EnRoute`, `Delivered` ou `NotDelivered`.

## Licença
Este projeto está licenciado sob a MIT License.