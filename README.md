# Employee Manager



### Repositório dedicado ao desenvolvimento do projeto referente a criação de um gerenciador de funcionários básico utilizando .Net, Angular e Docker.

<br/>

## Tecnologias e frameworks utilizados

- .Net 8
- Angular 19
- MySQL Community Server 8.0.35 (em container)
- Docker (necessário ter suporte a containers Linux para rodar)
- Entity Framework

<br/>

## Executando

* Usuário e senha de login pré-cadastrado: **"admin123"**
* Para executar o back-end:
    * Após clonar o repositório acesse a pasta "...\basic-employee-manager\EmployeeManager";
    * Execute o comando abaixo:
    * ```
      docker-compose up -d --build
      ```
    * Aguarde até um minuto para finalizar a criação dos containers (na primeira vez irá baixar a imagem do mysql caso não esteja disponível e gerar as imagens da API e do BD);
    * Para checar os containers em execução execute:
    * ```
      docker ps
      ```
    * O serviço estará disponível no endereço: http://localhost:55000/swagger/index.html
    * Para parar os containers execute:
    * ```
      docker-compose down
      ```
* Para executar o front-end:
    * Acesse a pasta "...\basic-employee-manager\EmployeeManager-Client"
    * Execute o comando abaixo:
    * ```
      ng s
      ```
      OU
      ```
      npm start
      ```
    * O serviço estará disponível no endereço: http://localhost:4200/

<br/>

