````markdown
# 🖧 Estudo de Microserviços com .NET

Este projeto é um estudo prático para entender como microserviços funcionam, como eles se comunicam entre si utilizando **ASP.NET Core**, **Controllers** e **HttpClient**, e como centralizar o acesso a esses serviços com **API Gateway (Ocelot)**.

## 📂 Estrutura do Projeto

- **MeuServicoApi**  
  Serviço principal que expõe o endpoint `/hello`.

- **MeuOutroServicoApi**  
  Serviço secundário que faz uma requisição HTTP para o `MeuServicoApi` e retorna a resposta.

- **ApiGateway**  
  Gateway que usa o **Ocelot** para rotear chamadas de um único ponto de entrada para os microserviços.

## ▶️ Como Rodar os Serviços

Abra três terminais (um para cada serviço) e execute os comandos abaixo:

### 1️⃣ Rodar o **MeuServicoApi**

```bash
cd MeuServicoApi
dotnet run --urls "http://localhost:5000"
````

Endpoint disponível:
[http://localhost:5000/hello](http://localhost:5000/hello)

### 2️⃣ Rodar o **MeuOutroServicoApi**

```bash
cd MeuOutroServicoApi
dotnet run --urls "http://localhost:5001"
```

Endpoint disponível:
[http://localhost:5001/Teste/chamar-servico](http://localhost:5001/Teste/chamar-servico)

Este endpoint faz a chamada para o `MeuServicoApi` e retorna um JSON com a resposta.

### 3️⃣ Rodar o **ApiGateway (Ocelot)**

```bash
cd ApiGateway
dotnet run --urls "http://localhost:8000"
```

Agora você pode acessar os microserviços através do **Gateway**.
Por exemplo, o `MeuServicoApi` pode ser acessado via:

[http://localhost:8000/meuservico/hello](http://localhost:8000/meuservico/hello)

> O comportamento é definido no arquivo **ocelot.json**, que mapeia as rotas "Upstream" (no Gateway) para as rotas "Downstream" (nos serviços).

## 🌐 Fluxo de Comunicação

1. **Cliente** → acessa `http://localhost:8000/meuservico/hello` (via Gateway)
2. **ApiGateway (Ocelot)** → encaminha a requisição para `http://localhost:5000/hello`
3. **MeuServicoApi** → responde com `{ "message": "Olá do MeuServicoApi!" }`
4. **ApiGateway** → devolve a resposta para o cliente.

Ou, sem o gateway:

1. **Cliente** → acessa `http://localhost:5001/Teste/chamar-servico`
2. **MeuOutroServicoApi** → faz uma requisição GET para `http://localhost:5000/hello`
3. **MeuServicoApi** → responde com `{ "message": "Olá do MeuServicoApi!" }`
4. **MeuOutroServicoApi** → devolve uma resposta agregada para o cliente.

## 🛠 Tecnologias Utilizadas

* **.NET 8** (Web API)
* **Controllers** para rotas
* **HttpClient** para comunicação entre serviços
* **Swagger** para documentação automática
* **Ocelot** para API Gateway e roteamento centralizado

## 💡 Observações

* Caso a porta do `MeuServicoApi` seja alterada, lembre-se de atualizar a URL usada no `TesteController` do `MeuOutroServicoApi` **e** no arquivo `ocelot.json` do `ApiGateway`.
* Ambos os serviços precisam estar rodando para o fluxo funcionar corretamente.
* Este projeto é apenas um estudo e pode ser expandido com:

  * Configuração via `appsettings.json` para armazenar URLs de outros serviços
  * Docker Compose para subir os serviços juntos
  * Integração com mensageria (ex.: RabbitMQ, Kafka)
  * Autenticação e autorização no API Gateway

## 📸 Exemplo de Resposta

A chamada para `http://localhost:5001/Teste/chamar-servico` deve retornar:

```json
{
  "mensagem": "MeuOutroServicoApi fez a chamada com sucesso!",
  "respostaDoOutroServico": {
    "message": "Olá do MeuServicoApi!"
  }
}
```

A chamada para `http://localhost:8000/meuservico/hello` (via Gateway) deve retornar:

```json
{
  "message": "Olá do MeuServicoApi!"
}
```
