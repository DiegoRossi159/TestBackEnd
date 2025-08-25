# TestBackEnd 1.0

**TestBackEnd** é um sistema voltado para o gerenciamento de aluguel de motocicletas e de entregadores, desenvolvido em C# utilizando o .NET. A aplicação faz uso das seguintes tecnologias:

- **PostgreSQL** como banco de dados relacional principal.
- **RabbitMQ** para gerenciar a troca de mensagens de forma assíncrona.
- **MongoDB** para persistir registros de notificações provenientes da mensageria.
- **Entity Framework Core** para mapeamento objeto-relacional (ORM).
- **minIO** para o armazenamento e gerenciamento de imagens.

## Configuração de Ambiente

Antes de iniciar, certifique-se de ter os seguintes pré-requisitos instalados:

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

**Portas liberadas**:

- **5432** - PostgreSQL
- **7201** - HTTPS Local
- **15672** - RabbitMQ 
- **27017** - MongoDB
- **9001** - MinIO

---

## Passo a Passo para Configuração

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/DiegoRossi159/TestBackEnd.git
   cd TestBackEnd

2. **Restaure as dependências do projeto:**

   ```bash
   dotnet restore
   
3. **Suba os containers necessários com o Docker Compose:**
   ```bash
   docker-compose up -d

4. **Aplicar as migrações do Entity Framework Core:**
   ```bash
   dotnet ef database update
   ```
   - Certifique que o dotnet-ef esteja instalado globalmente
     ```bash
     dotnet tool install --global dotnet-ef

5. **Execute a aplicação localmente:**
   ```bash
   dotnet run

---

## Acessando os Serviços

- **Swagger HTTPS:** [http://localhost:7201](https://localhost:7201/swagger/index.html)
- **RabbitMQ:** [http://localhost:15672](http://localhost:15672)
  - **Usuário padrão:** `user123`
  - **Senha padrão:** `user@123`
- **minIO:** [http://localhost:9001](http://localhost:9001)
  - **Usuário padrão:** `admin`
  - **Senha padrão:** `admin123`

---

## Casos de uso para teste

- Eu como usuário admin quero cadastrar uma nova moto.
  - Os dados obrigatórios da moto são Identificador, Ano, Modelo e Placa
  - A placa é um dado único e não pode se repetir.
  - Quando a moto for cadastrada a aplicação deverá gerar um evento de moto cadastrada
    - A notificação deverá ser publicada por mensageria.
    - Criar um consumidor para notificar quando o ano da moto for "2024"
    - Assim que a mensagem for recebida, deverá ser armazenada no banco de dados para consulta futura.
- Eu como usuário admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- Eu como usuário admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente
- Eu como usuário admin quero remover uma moto que foi cadastrado incorretamente, desde que não tenha registro de locações.
- Eu como usuário entregador quero me cadastrar na plataforma para alugar motos.
    - Os dados do entregador são( identificador, nome, cnpj, data de nascimento, número da CNHh, tipo da CNH, imagemCNH)
    - Os tipos de cnh válidos são A, B ou ambas A+B.
    - O cnpj é único e não pode se repetir.
    - O número da CNH é único e não pode se repetir.
- Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
    - O formato do arquivo deve ser png ou bmp.
    - A foto não poderá ser armazenada no banco de dados, você pode utilizar um serviço de storage( disco local, amazon s3, minIO ou outros).
- Eu como entregador quero alugar uma moto por um período.
    - Os planos disponíveis para locação são:
        - 7 dias com um custo de R$30,00 por dia
        - 15 dias com um custo de R$28,00 por dia
        - 30 dias com um custo de R$22,00 por dia
        - 45 dias com um custo de R$20,00 por dia
        - 50 dias com um custo de R$18,00 por dia
    - A locação obrigatóriamente tem que ter uma data de inicio e uma data de término e outra data de previsão de término.
    - O inicio da locação obrigatóriamente é o primeiro dia após a data de criação.
    - Somente entregadores habilitados na categoria A podem efetuar uma locação
- Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da locação.
    - Quando a data informada for inferior a data prevista do término, será cobrado o valor das diárias e uma multa adicional
        - Para plano de 7 dias o valor da multa é de 20% sobre o valor das diárias não efetivadas.
        - Para plano de 15 dias o valor da multa é de 40% sobre o valor das diárias não efetivadas.
    - Quando a data informada for superior a data prevista do término, será cobrado um valor adicional de R$50,00 por diária adicional.