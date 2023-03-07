# Backend-Banco-Digital-Green

Codifica��o de Backend Banco Digital Green em C# utilizando o framework ASP.NET Core e MongoDB para o banco de dados. 
Tamb�m utilizamos o Kafka como middleware de mensageria para a comunica��o ass�ncrona entre as diferentes partes do sistema.
Utilizando o framework ASP.NET Core 6.0. O ASP.NET Core � um framework open-source para desenvolvimento de aplica��es web, 
que permite a cria��o de aplica��es multiplataforma para Windows, macOS e Linux. Ele � modular, possui uma arquitetura de middleware 
e � altamente escal�vel e perform�tico.

A aplica��o utiliza o banco de dados NoSQL MongoDB como armazenamento de dados. O MongoDB � um banco de dados orientado a documentos, 
que utiliza um modelo de dados flex�vel e escal�vel horizontalmente.

Al�m disso, a aplica��o faz uso do Apache Kafka, um sistema de mensagens distribu�do que permite a comunica��o ass�ncrona entre diferentes 
componentes da aplica��o. O Kafka � altamente escal�vel e tolerante a falhas, sendo frequentemente utilizado em sistemas de alta performance.

Para a documenta��o dos endpoints da API, � utilizado o Swagger UI, que permite a visualiza��o e teste dos endpoints de forma interativa.

Tamb�m � utilizado o Docker para empacotar e distribuir a aplica��o em um ambiente isolado, tornando o processo de deploy mais f�cil 
e consistente.

Abaixo acessos os Endpoints da Aplica��o
Controllers
AcoesController
Respons�vel pelas opera��es relacionadas a a��es de ativos digitais, como compra e venda.

GET api/acoes: Retorna todas as a��es dispon�veis para compra.
GET api/acoes/{id}: Retorna uma a��o espec�fica pelo seu ID.
POST api/acoes: Permite a compra de uma a��o, criando uma nova transa��o.
PUT api/acoes/{id}: Atualiza o pre�o de uma a��o espec�fica pelo seu ID.
DELETE api/acoes/{id}: Remove uma a��o espec�fica pelo seu ID.
AtivoDigitalController
Respons�vel por gerenciar os ativos digitais dispon�veis para negocia��o.

GET api/ativosdigitais: Retorna todos os ativos digitais dispon�veis para negocia��o.
GET api/ativosdigitais/{id}: Retorna um ativo digital espec�fico pelo seu ID.
POST api/ativosdigitais: Cria um novo ativo digital.
PUT api/ativosdigitais/{id}: Atualiza um ativo digital espec�fico pelo seu ID.
DELETE api/ativosdigitais/{id}: Remove um ativo digital espec�fico pelo seu ID.
PagamentoController
Respons�vel pelas opera��es de pagamento, como cria��o de cart�es de cr�dito e pagamento de faturas.

GET api/pagamento/cartoescredito: Retorna todos os cart�es de cr�dito cadastrados.
GET api/pagamento/cartoescredito/{id}: Retorna um cart�o de cr�dito espec�fico pelo seu ID.
POST api/pagamento/cartoescredito: Cria um novo cart�o de cr�dito.
PUT api/pagamento/cartoescredito/{id}: Atualiza um cart�o de cr�dito espec�fico pelo seu ID.
DELETE api/pagamento/cartoescredito/{id}: Remove um cart�o de cr�dito espec�fico pelo seu ID.
POST api/pagamento/pagarfatura: Realiza o pagamento de uma fatura.
TransacaoController
Respons�vel pelas opera��es de transa��es, como cria��o e consulta.

GET api/transacao: Retorna todas as transa��es realizadas.
GET api/transacao/{id}: Retorna uma transa��o espec�fica pelo seu ID.
POST api/transacao: Cria uma nova transa��o.
DELETE api/transacao/{id}: Remove uma transa��o espec�fica pelo seu ID.
TransferenciaController
Respons�vel pelas opera��es de transfer�ncia entre contas.

GET api/transferencia: Retorna todas as transfer�ncias realizadas.
GET api/transferencia/{id}: Retorna uma transfer�ncia espec�fica pelo seu ID.
POST api/transferencia: Cria uma nova transfer�ncia.
DELETE api/transferencia/{id}: Remove uma transfer�ncia espec�fica pelo seu ID.
UsuarioController
Respons�vel pelas opera��es de usu�rios, como cria��o, consulta e autentica��o.

GET api/usuario: Retorna todos os usu�rios cadastrados.
GET api/usuario/{id}: Retorna um usu�rio espec�fico pelo seu ID.
POST api/usuario: Cria um novo usu�rio.