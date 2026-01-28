# Sisteped - Sistema de Gestão Pedagógica


Uma ferramenta prática para professores: este projeto oferece um aplicativo que centraliza o gerenciamento de turmas e alunos, permitindo acompanhar notas, desempenho por conteúdo e comportamentos individuais. Com ele, o professor consegue identificar padrões, acompanhar a evolução de cada aluno e tomar decisões pedagógicas mais eficazes, tudo de forma intuitiva, rápida e personalizada. É um sistema que coloca o controle e a clareza nas mãos do professor, facilitando o ensino e fortalecendo o acompanhamento do aprendizado.


---
# Configuração Inicial


### SistepedWeb
- Abra o Visual Studio Code.
- Abra o Workspace pelo arquivo `/workspace/SistepedWeb.code-workspace`.
- Execute o `./setup.sh`.


### SistepedAPI
- Abra o Microsoft Visual Studio.
- Abra o terminal em `Exibir - Terminal` e execute `dotnet ef database update`.
- Abra a solução em `/sisteped-api/sisteped-api.sln`.
- Execute a solução.


---


## 1. Requisitos Funcionais (por Módulo)


### **Módulo ALU – Alunos**
**ALU-01 – Cadastro de Alunos**  
- Adicionar, editar e remover alunos.  
- Armazenar informações básicas: nome, idade, turma, contato opcional.


**ALU-02 – Histórico do Aluno**  
- Visualizar histórico consolidado de notas e comportamentos por aluno.


**ALU-03 – Busca e Filtros de Alunos**  
- Buscar alunos por nome, turma ou tags de comportamento/desempenho.


---


### **Módulo TUR – Turmas**
**TUR-01 – Gerenciamento de Turmas**  
- Criar turmas personalizadas.  
- Adicionar ou remover alunos de cada turma.


**TUR-02 – Visão Geral da Turma**  
- Exibir resumo de desempenho médio da turma (notas e comportamentos).


---


### **Módulo NOT – Notas**
**NOT-01 – Registro de Notas**  
- Lançar notas para cada aluno.  
- Adicionar tags às notas para identificar conteúdos ou competências.  
- Visualizar histórico de notas por aluno ou turma.


**NOT-02 – Média e Estatísticas**  
- Calcular média automática por conteúdo, aluno e turma.


**NOT-03 – Notas por Conteúdo**  
- Filtrar notas por matéria, conteúdo ou competência específica.


---


### **Módulo COM – Comportamentos**
**COM-01 – Registro de Comportamentos**  
- Criar cards de comportamento com descrição e tipo.  
- Associar comportamentos a alunos.  
- Registrar histórico por aluno e turma.


**COM-02 – Métricas de Frequência**  
- Analisar padrões de comportamentos positivos/negativos por período.


---


### **Módulo MON – Monitoramento e Análises**
**MON-01 – Análises Acadêmicas**  
- Gráficos e indicadores de desempenho por conteúdo.


**MON-02 – Análises Comportamentais**  
- Frequência por tipo de comportamento.


**MON-03 – Comparação Temporal**  
- Comparar evolução de alunos ao longo do tempo.


**MON-04 – Painel Geral do Professor**  
- Dashboard com visão agregada do semestre, turmas e alertas.


---


### **Módulo REL – Relatórios**
**REL-01 – Exportação e Relatórios**  
- Gerar relatórios para impressão ou compartilhamento.  
- Exportar dados para CSV ou PDF.


**REL-02 – Relatório Consolidado da Turma**  
- Documento com notas, médias, estatísticas e análises de comportamento.


---


### **Módulo SYS – Sistema / Infraestrutura**
**SYS-01 – Armazenamento Local**  
- Banco de dados local para salvar todas as informações.  
- Funcionamento offline.


**SYS-02 – Backup Local Manual**  
- Exportar e importar backups do banco local.


**SYS-03 – Tema Claro/Escuro**  
- Alternar entre dois temas para melhorar a usabilidade.


---


## 2. Requisitos Não Funcionais (Metrificados)


1. **Usabilidade**
   - Tempo médio para realizar ações comuns (ex.: lançar nota) ≤ **5 segundos**.
   - Usuário deve conseguir localizar qualquer função principal em até **3 cliques**.


2. **Performance**
   - Operações de cadastro, edição e remoção devem responder em **< 200 ms**.  
   - Consultas com filtros devem carregar em até **1 segundo**, mesmo com 300+ alunos.


3. **Segurança**
   - Dados locais devem estar armazenados com criptografia AES-256 ou equivalente.  
   - Processo de backup deve gerar arquivo íntegro em **100%** das tentativas.


4. **Portabilidade**
   - Compatível com Windows, Linux e macOS.  
   - Interface deve se adaptar corretamente a telas entre **768px e 4K**.


5. **Escalabilidade**
   - Arquitetura deve permitir adicionar novos módulos sem refatorações extensas, com impacto máximo de **< 10%** sobre módulos já existentes.  
   - Banco local deve suportar crescimento até **10 mil registros** sem perda de performance significativa.


---
## Diagrama de Relacionamento
![](docs/imagens/Diagram de Classes - Sisteped.png)


## Casos de Uso
Pasta: `/docs/casos de uso/`


![](docs/imagens/use cases.png)


**Diagramas**


![](docs/imagens/diagrama-relacionamento-sisteped.svg)


---


## 3. Design (Protótipo)


- **Protótipo Sisteped:** [Sisteped no Figma](https://www.figma.com/design/WE4tHmzitXWEictT3dCRfe/Sisteped?node-id=0-1&t=IRbu88pydoPa5qWc-1)
- **PI4 — Protótipo / Referência:** [PI-4 MARCO I](https://www.figma.com/design/LMMxZiu67DsR1nqvS2DIMq/PI-4-MARCO-I?node-id=0-1&p=f&t=JD5AybDtn5JbJOV0-0)


- **Como acessar:** Clique no link acima; caso seja solicitado, faça login no Figma. Os protótipos estão compartilhados por link — verifique as permissões se não conseguir visualizar.


---


## Como Executar (Desenvolvimento)


- **Pré-requisitos:** .NET SDK (versão compatível), Node.js (v16+ recomendado), `npm`.
- **Backend (API):**
   - Acesse a pasta: `sistepad-api/sisteped-api`
   - Restaurar e executar:
      - `dotnet restore`
      - Ajuste a string de conexão em `appsettings.Development.json` ou variáveis de ambiente
      - (opcional) `dotnet ef database update` para aplicar migrations
      - `dotnet run`
- **Frontend (Web):**
   - Acesse a pasta: `sistepad-web`
   - Instale dependências e execute:
      - `npm install`
      - `npm run dev`


## Fluxo da Aplicação (resumo)


- **Autenticação:** usuário faz login → token JWT é emitido pelo backend (`JwtService`).
- **Camada principal:** usuário autenticado gerencia turmas, disciplinas e alunos (CRUD).
- **Lançamento de dados:** professores lançam atividades, notas e registram frequência/comportamento.
- **Monitoramento:** módulos de análise agregam médias, frequências e geram visualizações e alertas.
- **Export/Relatórios:** relatórios podem ser exportados para CSV/PDF para análises externas e prestação de contas.


## Como o projeto resolve as dores dos PIs


- **Base comum:** autenticação, gerenciamento de turmas, disciplinas e cadastro de alunos (reutilizável para todos os PIs).


- **PI2 — Monitorar atividades e notas dos alunos:**
   - Registro de atividades e notas por aluno/turma (módulo `NOT`).
   - Filtros por disciplina, conteúdo e período; cálculo automático de médias e estatísticas (módulo `MON`).
   - Dashboard de acompanhamento por aluno e por turma; histórico detalhado para identificar quedas de desempenho.


- **PI4 — Monitorar frequência e gerar relatórios para auxílio governamental:**
   - Registro de frequência por aluno e por período (fluxo integrado no módulo de monitoramento).
   - Exportação de relatórios consolidados (módulo `REL`) em CSV/PDF para seleção de beneficiários do auxílio (pé de meia).
   - Filtros e regras para identificar alunos elegíveis (histórico de frequência, notas e indicadores socioeconômicos, quando disponíveis).


---


## 4. Casos de teste
### Cadastro
1. Deve criar usuário com sucesso (201)
2. Deve retornar BadRequest e o campo incorreto quando os dados estiverem num formato inválido (400)


### Login
1. Deve retornar Login bem-sucedido e o token (200)
2. Deve retornar BadRequest  e o campo incorreto quando os dados estiverem num formato inválido for inválido (400)
3. Deve retornar que as credenciais estão inválidas quando os dados forem passados corretamente mas o usuário não existir ou as credenciais estiverem erradas(401)
