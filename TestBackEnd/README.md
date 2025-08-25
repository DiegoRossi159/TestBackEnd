# TestBackEnd 1.0

**TestBackEnd** � um sistema voltado para o gerenciamento de aluguel de motocicletas e de entregadores, desenvolvido em C# utilizando o .NET. A aplica��o faz uso das seguintes tecnologias:

- **PostgreSQL** como banco de dados relacional principal.
- **RabbitMQ** para gerenciar a troca de mensagens de forma ass�ncrona.
- **MongoDB** para persistir registros de notifica��es provenientes da mensageria.
- **Entity Framework Core** para mapeamento objeto-relacional (ORM).
- **minIO** para o armazenamento e gerenciamento de imagens.

## Configura��o de Ambiente

Antes de iniciar, certifique-se de ter os seguintes pr�-requisitos instalados:

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

## Passo a Passo para Configura��o

1. **Clone o reposit�rio:**

   ```bash
   git clone https://github.com/DiegoRossi159/TestBackEnd.git
   cd TestBackEnd

2. **Restaure as depend�ncias do projeto:**

   ```bash
   dotnet restore
   
3. **Suba os containers necess�rios com o Docker Compose:**
   ```bash
   docker-compose up -d

4. **Aplicar as migra��es do Entity Framework Core:**
   ```bash
   dotnet ef database update
   ```
   - Certifique que o dotnet-ef esteja instalado globalmente
     ```bash
     dotnet tool install --global dotnet-ef

5. **Execute a aplica��o localmente:**
   ```bash
   dotnet run

---

## Acessando os Servi�os

- **Swagger HTTPS:** [http://localhost:7201](https://localhost:7201/swagger/index.html)
- **RabbitMQ:** [http://localhost:15672](http://localhost:15672)
  - **Usu�rio padr�o:** `user123`
  - **Senha padr�o:** `user@123`
- **minIO:** [http://localhost:9001](http://localhost:9001)
  - **Usu�rio padr�o:** `admin`
  - **Senha padr�o:** `admin123`

---

## Casos de uso para teste

- Eu como usu�rio admin quero cadastrar uma nova moto.
  - Os dados obrigat�rios da moto s�o Identificador, Ano, Modelo e Placa
  - A placa � um dado �nico e n�o pode se repetir.
  - Quando a moto for cadastrada a aplica��o dever� gerar um evento de moto cadastrada
    - A notifica��o dever� ser publicada por mensageria.
    - Criar um consumidor para notificar quando o ano da moto for "2024"
    - Assim que a mensagem for recebida, dever� ser armazenada no banco de dados para consulta futura.
- Eu como usu�rio admin quero consultar as motos existentes na plataforma e conseguir filtrar pela placa.
- Eu como usu�rio admin quero modificar uma moto alterando apenas sua placa que foi cadastrado indevidamente
- Eu como usu�rio admin quero remover uma moto que foi cadastrado incorretamente, desde que n�o tenha registro de loca��es.
- Eu como usu�rio entregador quero me cadastrar na plataforma para alugar motos.
    - Os dados do entregador s�o( identificador, nome, cnpj, data de nascimento, n�mero da CNHh, tipo da CNH, imagemCNH)
    - Os tipos de cnh v�lidos s�o A, B ou ambas A+B.
    - O cnpj � �nico e n�o pode se repetir.
    - O n�mero da CNH � �nico e n�o pode se repetir.
- Eu como entregador quero enviar a foto de minha cnh para atualizar meu cadastro.
    - O formato do arquivo deve ser png ou bmp.
    - A foto n�o poder� ser armazenada no banco de dados, voc� pode utilizar um servi�o de storage( disco local, amazon s3, minIO ou outros).
- Eu como entregador quero alugar uma moto por um per�odo.
    - Os planos dispon�veis para loca��o s�o:
        - 7 dias com um custo de R$30,00 por dia
        - 15 dias com um custo de R$28,00 por dia
        - 30 dias com um custo de R$22,00 por dia
        - 45 dias com um custo de R$20,00 por dia
        - 50 dias com um custo de R$18,00 por dia
    - A loca��o obrigat�riamente tem que ter uma data de inicio e uma data de t�rmino e outra data de previs�o de t�rmino.
    - O inicio da loca��o obrigat�riamente � o primeiro dia ap�s a data de cria��o.
    - Somente entregadores habilitados na categoria A podem efetuar uma loca��o
- Eu como entregador quero informar a data que irei devolver a moto e consultar o valor total da loca��o.
    - Quando a data informada for inferior a data prevista do t�rmino, ser� cobrado o valor das di�rias e uma multa adicional
        - Para plano de 7 dias o valor da multa � de 20% sobre o valor das di�rias n�o efetivadas.
        - Para plano de 15 dias o valor da multa � de 40% sobre o valor das di�rias n�o efetivadas.
    - Quando a data informada for superior a data prevista do t�rmino, ser� cobrado um valor adicional de R$50,00 por di�ria adicional.