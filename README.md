## Descrição 

Projeto de uma API que recebe arquivos para serem julgados, consultar as submissões

## Como rodar 

## Dependencias

- Docker (Compose) - Se preferir rodar via docker
- (AWS CLI)[https://docs.aws.amazon.com/pt_br/cli/latest/userguide/getting-started-install.html]
- Dotnet

### Configuração 

Depois de baixar o AWS CLI, rode o comando `aws configure` para configurar as credenciais de acesso ao S3 e o SQS.
Para executar o sistema é preciso configurar as variáveis de ambiente *AWS_ACCESS_KEY_ID* e *AWS_SECRET_ACCESS_KEY* que são utilizadas para acessar a conta AWS contendo as filas sqs e o bucket S3, para configurar basta executar o comando.

```bash
export AWS_ACCESS_KEY_ID=***
export AWS_SECRET_ACCESS_KEY=***
```

### Utilizando dotnet cli

O projeto foi desenvolvido em C# utilizando o [dotnet](https://dotnet.microsoft.com/en-us/), para instalar o cli basta seguir os passos da [documentação](https://docs.microsoft.com/pt-br/dotnet/core/install/linux-ubuntu). 

A ponte de entrada do projeto é a API que pode ser executada utilizando os comandos

```bash
dotnet restore

dotnet dotnet run --project src/Api/Api.csproj
```

### Utilizando docker compose

Também é possível rodar a aplicação utilizando o docker sem a necessidade de instalar o cli na máquina.

Primeiro basta realizar o build e o run

```bash
docker-compose up --build
```