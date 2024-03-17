![Build Status](https://github.com/RafaelMuniz94/gestor_projetos_tarefas/actions/workflows/ScriptBuildTest.yml/badge.svg) [![Coverage Status](https://coveralls.io/repos/github/RafaelMuniz94/gestor_projetos_tarefas/badge.svg?branch=main)](https://coveralls.io/github/RafaelMuniz94/gestor_projetos_tarefas?branch=main) ![Versão do .NET](https://img.shields.io/badge/.NET-7.x-blueviolet) ![Última Atualização](https://img.shields.io/github/last-commit/RafaelMuniz94/gestor_projetos_tarefas/main)

---

### Etapa 1: Desenvolvimento da API

Para a primeira Sprint, foi estipulado o desenvolvimento de funcionalidades básicas para o gerenciamento de tarefas. Desenvolva uma API RESTful capaz de responder às requisições feitas pelo aplicativo para os seguintes itens:

1. **Listagem de Projetos** - listar todos os projetos do usuário
2. **Visualização de Tarefas** - visualizar todas as tarefas de um projeto específico
3. **Criação de Projetos** - criar um novo projeto
4. **Criação de Tarefas** - adicionar uma nova tarefa a um projeto
5. **Atualização de Tarefas** - atualizar o status ou detalhes de uma tarefa
6. **Remoção de Tarefas** - remover uma tarefa de um projeto

**Regras de negócio:**

1. **Prioridades de Tarefas:**
   - Cada tarefa deve ter uma prioridade atribuída (baixa, média, alta).
   - Não é permitido alterar a prioridade de uma tarefa depois que ela foi criada.
2. **Restrições de Remoção de Projetos:**
   - Um projeto não pode ser removido se ainda houver tarefas pendentes associadas a ele.
   - Caso o usuário tente remover um projeto com tarefas pendentes, a API deve retornar um erro e sugerir a conclusão ou remoção das tarefas primeiro.
3. **Histórico de Atualizações:**
   - Cada vez que uma tarefa for atualizada (status, detalhes, etc.), a API deve registrar um histórico de alterações para a tarefa.
   - O histórico de alterações deve incluir informações sobre o que foi modificado, a data da modificação e o usuário que fez a modificação.
4. **Limite de Tarefas por Projeto:**
   - Cada projeto tem um limite máximo de 20 tarefas. Tentar adicionar mais tarefas do que o limite deve resultar em um erro.
5. **Relatórios de Desempenho:**
   - A API deve fornecer endpoints para gerar relatórios de desempenho, como o número médio de tarefas concluídas por usuário nos últimos 30 dias.
   - Os relatórios devem ser acessíveis apenas por usuários com uma função específica de "gerente".
6. **Comentários nas Tarefas:**
   - Os usuários podem adicionar comentários a uma tarefa para fornecer informações adicionais.
   - Os comentários devem ser registrados no histórico de alterações da tarefa.

**Regras da API e avaliação:**

1. **Não é necessário** nenhum tipo de CRUD para usuários.
2. **Não é necessário** nenhum tipo de autenticação; este será um serviço externo.
3. Tenha pelo menos **80%** de cobertura de testes de unidade para validar suas regras de negócio.
4. **Utilize o git** como ferramenta de versionamento de código.
5. **Utilize um banco de dados** (o que preferir) para salvar os dados.
6. **Utilize o framework e libs** que julgue necessário para uma boa implementação.
7. **O projeto deve executar no docker e as informações de execução via terminal devem estar disponíveis no** [README.md](http://README.md) **do projeto**.

### Docker
Para solucionar parte do desafio, este projeto é executado no Docker.

Docker é uma plataforma de software que permite a criação, o teste e a implantação de aplicativos dentro de contêineres. Um contêiner é uma unidade de software que inclui tudo o que o aplicativo precisa para ser executado de forma independente, incluindo o código, as bibliotecas, as ferramentas e as configurações. O Docker simplifica o processo de desenvolvimento e implantação de aplicativos, fornecendo um ambiente consistente e isolado.

Para iniciar a execução, siga os seguintes passos:
1. Certifique-se de ter o .NET SDK 7 instalado em seu sistema. Você pode fazer o download e instalar a versão mais recente do .NET SDK a partir do site oficial da Microsoft: https://dotnet.microsoft.com/download/dotnet/7.0

2. Certifique-se de ter o Docker instalado em seu sistema. Você pode fazer o download e instalar o Docker a partir do site oficial: https://www.docker.com/get-started
 

3. Realize o clone do repositório do projeto: 
```
git clone https://github.com/RafaelMuniz94/gestor_projetos_tarefas.git
```

4. A partir da raiz do projeto navegue ate o caminho: 
```
cd <caminho_da_pasta_raiz>\gestor_projetos_tarefas\src\Gestor_Projetos_Tarefas.Api
```
6. Execute o docker

7. A partir deste diretório, execute o seguinte comando para construir a imagem Docker, incluindo a aplicação e suas dependências:
```docker
docker build -f ./Dockerfile  -t gestorprojetostarefasapi:dev  --build-arg "BUILD_CONFIGURATION=Debug" ..
```
- A flag ```-f``` especifica o arquivo que será usado para construir a imagem.
- A flag ```-t``` define o nome da imagem.
- A flag ```--build-arg``` permite adicionar parâmetros durante a construção da imagem.
- O parâmetro ```..``` representam o caminho para o repositório que contém o código-fonte da aplicação. 

8. Agora que a imagem foi gerada, será necessário criar e executar o container utilizando o seguinte comando:
```docker
 docker run -d -p 3000:80 -p 443:443 --name gestorprojetostarefasapi gestorprojetostarefasapi:dev
```
- A flag `-d` executa o container em modo detached, permitindo que o terminal permaneça utilizável.
- As flags `-p` mapeiam as portas entre o host e o container.
- A flag `--name` define o nome do container.
- O parâmetro `gestorprojetostarefasapi:dev` indica a imagem que será utilizada no container.

9. Execute chamadas por meio de um cliente HTTP. O padrão de URL a ser seguido será: `http://localhost:3000/api/<controller>/<endpoint>`
---
### Fase 2: Refinamento

Para a segunda fase, escreva no arquivo **README.md** em uma seção dedicada o que você perguntaria ao PO visando o refinamento para futuras implementações ou melhorias.

- Qual é o público-alvo que o sistema busca atender e como é medido seu uso?
- Quais são as possíveis novas funcionalidades do sistema além das especificadas na Fase 1?
- Existe a possibilidade de modificação nas regras de negócio que precisamos considerar?
- Quando poderemos priorizar tarefas visando otimizar o desempenho da API para lidar com um aumento no número de usuários e tarefas?
- Qual é a previsão de usuários totais e picos de utilização do sistema?
- Quais são as áreas que mais se beneficiariam dos dados fornecidos pelo sistema e como iremos priorizar estratégias de observabilidade para medir o desempenho e eficiência da solução?
- Quais são os times que poderiam consumir a API e como podemos disponibilizar uma documentação eficaz?
- É possível construir uma interface entre outras ferramentas que o usuário utiliza para integrar com a API?

---

### Fase 3: Final

Na terceira fase, escreva no arquivo **README.md** em uma sessão dedicada o que você melhoraria no projeto, identificando possíveis pontos de melhoria, implementação de padrões, visão do projeto sobre arquitetura/cloud, etc.


- Adotar uma arquitetura de microsserviços, eliminando a interdependência entre as funcionalidades da aplicação.
- Implementar ferramentas e práticas de monitoramento e observabilidade para acompanhar o desempenho do sistema.
- Aprimorar a geração de logs e estabelecer políticas para armazenamento e análise dos registros.
- Introduzir medidas de segurança e identificação dos consumidores da aplicação.
- Elaborar uma documentação completa para facilitar a manutenção e integração da aplicação por outros sistemas.
- Avaliar e implementar estratégias visando tornar a infraestrutura mais escalável. Considerando a simplicidade das rotas e funções, explorar a possibilidade de adotar a arquitetura serverless pode ser uma opção interessante.
- Selecionar um banco de dados robusto e independente do contêiner da aplicação para garantir uma operação mais confiável e eficiente. 
- Implementar operações CRUD para usuários.

---
## Dependências utilizadas:

- AutoMapper: Utilizada para mapeamento de DTOs
- Entity Framework Core: Gerenciamento e acesso ao banco de dados
- Entity Framework Core SQLite: Ferramentas utilitarias para conexão com SQLite
- Serilog: Utilitário para criação de logs
- Serilog.AspNetCore: Utilitários necessários para a configuração da bibiliteca Serilog em aplicações AspNet.
- Serilog.Sinks.Console: Conector para gerar log na console.
- Swagger: Utilizado para gerar live doc da aplicação e para auxiliar na execução de testes.
- MOQ: Utilitário para criação de objetos Mockados, facilitando criação de instância de alguns objetos e forçar comportamentos para testes mais específicos.

## SQLite

O banco de dados escolhido para essa solução foi o SQLite, devido a sua simplicidade, não necessitar de configurações para executar e ser multiplataforma, importante para execução dos testes de integração em **VMs** do **GitHub Actions**.