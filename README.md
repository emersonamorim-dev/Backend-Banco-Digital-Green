# Backend-Banco-Digital-Green

Codificação de Backend Banco Digital Green em C# utilizando o framework ASP.NET Core e MongoDB para o banco de dados. 
Também utilizamos o Kafka como middleware de mensageria para a comunicação assíncrona entre as diferentes partes do sistema.
Utilizando o framework ASP.NET Core 6.0. O ASP.NET Core é um framework open-source para desenvolvimento de aplicações web, 
que permite a criação de aplicações multiplataforma para Windows, macOS e Linux. Ele é modular, possui uma arquitetura de middleware 
e é altamente escalável e performático.

A aplicação utiliza o banco de dados NoSQL MongoDB como armazenamento de dados. O MongoDB é um banco de dados orientado a documentos, 
que utiliza um modelo de dados flexível e escalável horizontalmente.

Além disso, a aplicação faz uso do Apache Kafka, um sistema de mensagens distribuído que permite a comunicação assíncrona entre diferentes 
componentes da aplicação. O Kafka é altamente escalável e tolerante a falhas, sendo frequentemente utilizado em sistemas de alta performance.

Para a documentação dos endpoints da API, é utilizado o Swagger UI, que permite a visualização e teste dos endpoints de forma interativa.

Também é utilizado o Docker para empacotar e distribuir a aplicação em um ambiente isolado, tornando o processo de deploy mais fácil 
e consistente.

# Abaixo acessos os Endpoints da Aplicação
Controllers
AcoesController
Responsável pelas operações relacionadas a ações de ativos digitais, como compra e venda.
<br>
GET api/acoes: Retorna todas as ações disponíveis para compra.
GET api/acoes/{id}: Retorna uma ação específica pelo seu ID.
POST api/acoes: Permite a compra de uma ação, criando uma nova transação.
PUT api/acoes/{id}: Atualiza o preço de uma ação específica pelo seu ID.
DELETE api/acoes/{id}: Remove uma ação específica pelo seu ID.
AtivoDigitalController
Resp<br>onsável por gerenciar os ativos digitais disponíveis para negociação.
<br>
GET api/ativosdigitais: Retorna todos os ativos digitais disponíveis para negociação.
GET api/ativosdigitais/{id}: Retorna um ativo digital específico pelo seu ID.
POST api/ativosdigitais: Cria um novo ativo digital.
PUT api/ativosdigitais/{id}: Atualiza um ativo digital específico pelo seu ID.
DELETE api/ativosdigitais/{id}: Remove um ativo digital específico pelo seu ID.
PagamentoController
Responsável pelas operações de pagamento, como criação de cartões de crédito e pagamento de faturas.
<br>
GET api/pagamento/cartoescredito: Retorna todos os cartões de crédito cadastrados.
GET api/pagamento/cartoescredito/{id}: Retorna um cartão de crédito específico pelo seu ID.
POST api/pagamento/cartoescredito: Cria um novo cartão de crédito.
PUT api/pagamento/cartoescredito/{id}: Atualiza um cartão de crédito específico pelo seu ID.
DELETE api/pagamento/cartoescredito/{id}: Remove um cartão de crédito específico pelo seu ID.
POST api/pagamento/pagarfatura: Realiza o pagamento de uma fatura.
TransacaoController
Responsável pelas operações de transações, como criação e consulta.
<br>
GET api/transacao: Retorna todas as transações realizadas.
GET api/transacao/{id}: Retorna uma transação específica pelo seu ID.
POST api/transacao: Cria uma nova transação.
DELETE api/transacao/{id}: Remove uma transação específica pelo seu ID.
TransferenciaController
Responsável pelas operações de transferência entre contas.
<br>
GET api/transferencia: Retorna todas as transferências realizadas.
GET api/transferencia/{id}: Retorna uma transferência específica pelo seu ID.
POST api/transferencia: Cria uma nova transferência.
DELETE api/transferencia/{id}: Remove uma transferência específica pelo seu ID.
UsuarioController
Responsável pelas operações de usuários, como criação, consulta e autenticação.
<br>
GET api/usuario: Retorna todos os usuários cadastrados.
GET api/usuario/{id}: Retorna um usuário específico pelo seu ID.
POST api/usuario: Cria um novo usuário.
