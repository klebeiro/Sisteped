# Sisteped - Sistema de Gestão Pedagógica

Uma ferramenta prática para professores: este projeto oferece um aplicativo que centraliza o gerenciamento de turmas e alunos, permitindo acompanhar notas, desempenho por conteúdo e comportamentos individuais. Com ele, o professor consegue identificar padrões, acompanhar a evolução de cada aluno e tomar decisões pedagógicas mais eficazes.

---

## Índice

- [Tecnologias](#tecnologias)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Como Executar](#como-executar)
- [Banco de Dados](#banco-de-dados)
- [Autenticação e Autorização](#autenticação-e-autorização)
- [Controllers e Rotas](#controllers-e-rotas)
- [Modelagem de Dados](#modelagem-de-dados)
- [Requisitos Funcionais](#requisitos-funcionais)
- [Requisitos Não Funcionais](#requisitos-não-funcionais)
- [Design (Protótipo)](#design-protótipo)
- [Casos de Teste](#casos-de-teste)

---

## Tecnologias

### Backend (API)
- **.NET 8.0** - Framework principal
- **Entity Framework Core 9.0** - ORM para acesso a dados
- **SQLite** (desenvolvimento) / **SQL Server** (produção)
- **JWT Bearer** - Autenticação via tokens
- **FluentValidation** - Validação de DTOs
- **AutoMapper** - Mapeamento entre entidades e DTOs
- **Swagger/OpenAPI** - Documentação da API

### Frontend (Web)
- **React 18** com TypeScript
- **Vite** - Build tool
- **Redux Toolkit** - Gerenciamento de estado
- **Axios** - Cliente HTTP

---

## Estrutura do Projeto

```
Sisteped/
├── docs/                          # Documentação
│   ├── atas/                      # Atas de reunião
│   ├── banco de dados/            # Scripts SQL
│   ├── casos de uso/              # Documentação de casos de uso
│   ├── design/                    # Links e recursos de design
│   └── imagens/                   # Imagens do projeto
│
├── sistepad-api/                  # Backend (.NET)
│   └── sisteped-api/
│       ├── Configuration/         # Configurações (AutoMapper)
│       ├── Controllers/           # Controllers da API
│       ├── DTOs/                  # Data Transfer Objects
│       │   ├── Request/           # DTOs de entrada
│       │   └── Response/          # DTOs de saída
│       ├── Helpers/               # Classes auxiliares
│       ├── Infra/Data/            # Contexto e configurações do EF
│       ├── IoC/                   # Injeção de dependência
│       ├── Migrations/            # Migrations do EF Core
│       ├── Models/                # Entidades do domínio
│       ├── Repositories/          # Camada de acesso a dados
│       ├── Resources/             # Mensagens de erro
│       ├── Scripts/               # Scripts SQL de seed
│       ├── Services/              # Camada de serviços/negócio
│       └── Validators/            # Validadores FluentValidation
│
└── sistepad-web/                  # Frontend (React)
    └── src/
        ├── components/            # Componentes reutilizáveis
        ├── data/                  # DTOs e models
        ├── pages/                 # Páginas da aplicação
        ├── services/              # Serviços de API
        ├── store/                 # Redux store
        ├── styles/                # Estilos globais
        └── utils/                 # Utilitários
```

---

## Como Executar

### Pré-requisitos
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/) (para o frontend)
- [SQLite](https://www.sqlite.org/) ou ferramenta como DB Browser for SQLite

### Backend (API)

```bash
# Navegue até a pasta da API
cd sistepad-api/sisteped-api

# Restaure as dependências
dotnet restore

# Execute as migrations (cria o banco SQLite)
dotnet ef database update

# Execute a aplicação
dotnet run
```

A API estará disponível em:
- **HTTP:** http://localhost:5000
- **HTTPS:** https://localhost:5001
- **Swagger:** https://localhost:5001/swagger

### Frontend (Web)

```bash
# Navegue até a pasta do frontend
cd sistepad-web

# Instale as dependências
npm install

# Crie o arquivo .env baseado no exemplo
cp .env.example .env

# Execute em modo de desenvolvimento
npm run dev
```

O frontend estará disponível em: http://localhost:5173

---

## Banco de Dados

### Configuração

O projeto usa **SQLite** para desenvolvimento local. O arquivo do banco é criado automaticamente em:
```
sistepad-api/sisteped-api/sisteped.db
```

Para usar **SQL Server** em produção, altere a connection string em `appsettings.json` e descomente a configuração correspondente em `DependencyInjection.cs`.

### Scripts de Seed

Os scripts SQL para popular o banco estão em `sistepad-api/sisteped-api/Scripts/`:

| Script | Descrição |
|--------|-----------|
| `seed_database.sql` | Popula o banco com dados de teste |
| `clear_database.sql` | Limpa todos os dados do banco |

#### Como executar os scripts:

**Via linha de comando:**
```bash
cd sistepad-api/sisteped-api
sqlite3 sisteped.db < Scripts/seed_database.sql
```

**Via DB Browser for SQLite:**
1. Abra o arquivo `sisteped.db`
2. Vá em "Execute SQL"
3. Cole o conteúdo do script e execute

**Via API (endpoint de seed):**
```bash
# Popular o banco
POST /api/Seed

# Limpar o banco
DELETE /api/Seed
```

### Dados de Teste (após seed)

| Tipo | Email | Senha |
|------|-------|-------|
| Coordenador | coordenador@escola.com | Senha@123 |
| Professor | professor1@escola.com | Senha@123 |
| Professor | professor2@escola.com | Senha@123 |
| Responsável | responsavel1@email.com | Senha@123 |
| Responsável | responsavel2@email.com | Senha@123 |
| Responsável | responsavel3@email.com | Senha@123 |

---

## Autenticação e Autorização

### Tipos de Usuário (Roles)

| Role | Valor | Descrição |
|------|-------|-----------|
| Coordinator | 1 | Acesso total ao sistema |
| Teacher | 2 | Acesso intermediário (leitura + operações de aula) |
| Guardian | 3 | Acesso limitado (apenas seus dependentes) |

### Políticas de Autorização

| Política | Roles Permitidas |
|----------|------------------|
| `CoordinatorOnly` | Coordinator |
| `CoordinatorOrTeacher` | Coordinator, Teacher |
| `AllAuthenticated` | Qualquer usuário autenticado |

### Fluxo de Autenticação

1. Faça login via `POST /api/User/login`
2. Receba o token JWT na resposta
3. Inclua o token no header de todas as requisições:
   ```
   Authorization: Bearer {seu_token}
   ```

---

## Controllers e Rotas

### UserController (`/api/User`)

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| POST | `/login` | Autenticação | Público |
| POST | `/create` | Criar usuário | Público (Guardian) / CoordinatorOnly (outros) |
| GET | `/{id}/details` | Obter usuário por ID | Autenticado (próprio) / Coordinator |
| GET | `/get-all` | Listar todos usuários | CoordinatorOnly |
| GET | `/teachers` | Listar professores | CoordinatorOnly |
| GET | `/guardians` | Listar responsáveis | CoordinatorOrTeacher |

### GradeController (`/api/Grade`) - Turmas

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todas as turmas | CoordinatorOrTeacher |
| GET | `/{id}` | Obter turma por ID | CoordinatorOrTeacher |
| POST | `/` | Criar turma | CoordinatorOnly |
| PUT | `/{id}` | Atualizar turma | CoordinatorOnly |
| DELETE | `/{id}` | Excluir turma | CoordinatorOnly |

### ClassController (`/api/Class`) - Matérias

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todas as matérias | CoordinatorOrTeacher |
| GET | `/{id}` | Obter matéria por ID | CoordinatorOrTeacher |
| POST | `/` | Criar matéria | CoordinatorOnly |
| PUT | `/{id}` | Atualizar matéria | CoordinatorOnly |
| DELETE | `/{id}` | Excluir matéria | CoordinatorOnly |

### StudentController (`/api/Student`) - Alunos

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todos os alunos | CoordinatorOrTeacher |
| GET | `/{id}` | Obter aluno por ID | Autenticado (com validação) |
| GET | `/by-guardian/{guardianId}` | Alunos por responsável | Autenticado (com validação) |
| GET | `/my-students` | Meus dependentes | Guardian |
| POST | `/` | Criar aluno | CoordinatorOnly |
| PUT | `/{id}` | Atualizar aluno | CoordinatorOrTeacher |
| DELETE | `/{id}` | Excluir aluno | CoordinatorOnly |

### AttendanceController (`/api/Attendance`) - Frequência

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todas as frequências | CoordinatorOrTeacher |
| GET | `/{id}` | Obter frequência por ID | CoordinatorOrTeacher |
| GET | `/by-student/{studentId}` | Frequências por aluno | Autenticado (com validação) |
| GET | `/by-class/{classId}` | Frequências por matéria | CoordinatorOrTeacher |
| GET | `/by-date/{date}` | Frequências por data | CoordinatorOrTeacher |
| GET | `/by-class-and-date/{classId}/{date}` | Por matéria e data | CoordinatorOrTeacher |
| GET | `/by-student-and-class/{studentId}/{classId}` | Por aluno e matéria | Autenticado (com validação) |
| POST | `/` | Registrar frequência | CoordinatorOrTeacher |
| POST | `/bulk` | Registrar em lote | CoordinatorOrTeacher |
| DELETE | `/{id}` | Excluir frequência | CoordinatorOnly |

### GridController (`/api/Grid`) - Grades Curriculares

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todas as grades curriculares | CoordinatorOrTeacher |
| GET | `/{id}` | Obter grade curricular por ID | CoordinatorOrTeacher |
| POST | `/` | Criar grade curricular | CoordinatorOnly |
| PUT | `/{id}` | Atualizar grade curricular | CoordinatorOnly |
| DELETE | `/{id}` | Excluir grade curricular | CoordinatorOnly |
| POST | `/add-grade` | Adicionar turma à grade curricular | CoordinatorOnly |
| POST | `/remove-grade` | Remover turma da grade curricular | CoordinatorOnly |

### GridGradeController (`/api/GridGrade`) - Grade Curricular x Turma

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todos | CoordinatorOrTeacher |
| GET | `/{id}` | Obter por ID | CoordinatorOrTeacher |
| GET | `/by-grid/{gridId}` | Turmas de uma grade curricular | CoordinatorOrTeacher |
| GET | `/by-grade/{gradeId}` | Grades curriculares de uma turma | CoordinatorOrTeacher |
| POST | `/` | Vincular turma à grade curricular | CoordinatorOnly |
| DELETE | `/{id}` | Desvincular turma da grade curricular | CoordinatorOnly |

### GridClassController (`/api/GridClass`) - Grade Curricular x Matéria

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todos | CoordinatorOrTeacher |
| GET | `/{id}` | Obter por ID | CoordinatorOrTeacher |
| GET | `/by-grid/{gridId}` | Matérias de uma grade curricular | CoordinatorOrTeacher |
| GET | `/by-class/{classId}` | Grades curriculares de uma matéria | CoordinatorOrTeacher |
| POST | `/` | Vincular matéria à grade curricular | CoordinatorOnly |
| DELETE | `/{id}` | Desvincular matéria da grade curricular | CoordinatorOnly |

### ActivityController (`/api/Activity`) - Atividades

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todas as atividades | AllAuthenticated |
| GET | `/{id}` | Obter atividade por ID | AllAuthenticated |
| GET | `/by-class/{classId}` | Atividades por matéria | AllAuthenticated |
| GET | `/by-date-range` | Atividades por período | AllAuthenticated |
| POST | `/` | Criar atividade | CoordinatorOrTeacher |
| PUT | `/{id}` | Atualizar atividade | CoordinatorOrTeacher |
| DELETE | `/{id}` | Excluir atividade | CoordinatorOnly |

### StudentActivityController (`/api/StudentActivity`) - Notas

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todas as notas | CoordinatorOrTeacher |
| GET | `/{id}` | Obter nota por ID | AllAuthenticated (com validação) |
| GET | `/by-student/{studentId}` | Notas por aluno | AllAuthenticated (com validação) |
| GET | `/by-activity/{activityId}` | Notas por atividade | CoordinatorOrTeacher |
| GET | `/my-students` | Notas dos dependentes | Guardian |
| POST | `/` | Lançar nota | CoordinatorOrTeacher |
| POST | `/bulk` | Lançar notas em lote | CoordinatorOrTeacher |
| PUT | `/{id}` | Atualizar nota | CoordinatorOrTeacher |
| DELETE | `/{id}` | Excluir nota | CoordinatorOnly |

### ClassTeacherController (`/api/ClassTeacher`) - Matéria x Professor

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todos | CoordinatorOrTeacher |
| GET | `/{id}` | Obter por ID | CoordinatorOrTeacher |
| GET | `/by-class/{classId}` | Por matéria | CoordinatorOrTeacher |
| GET | `/by-teacher/{teacherId}` | Por professor | CoordinatorOrTeacher (com validação) |
| GET | `/my-classes` | Minhas matérias | Teacher |
| POST | `/` | Criar atribuição | CoordinatorOnly |
| DELETE | `/{id}` | Excluir atribuição | CoordinatorOnly |

### StudentGridController (`/api/StudentGrid`) - Aluno x Grade Curricular

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| GET | `/` | Listar todos | CoordinatorOrTeacher |
| GET | `/{id}` | Obter por ID | CoordinatorOrTeacher |
| GET | `/by-student/{studentId}` | Grades curriculares do aluno | Autenticado (com validação) |
| GET | `/by-grid/{gridId}` | Alunos da grade curricular | CoordinatorOrTeacher |
| POST | `/` | Vincular aluno à grade curricular | CoordinatorOnly |
| DELETE | `/{id}` | Desvincular aluno da grade curricular | CoordinatorOnly |

### ReportController (`/api/Report`) - Relatórios

#### Relatórios de Frequência

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| POST | `/attendance` | Relatório de frequência | Autenticado (com filtros) |
| POST | `/attendance/by-student` | Resumo por aluno | CoordinatorOrTeacher |
| POST | `/attendance/by-grade` | Resumo por turma | CoordinatorOrTeacher |
| POST | `/attendance/my-students` | Frequência dos dependentes | Guardian |

#### Relatórios de Notas

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| POST | `/grades` | Relatório de notas | Autenticado (com filtros) |
| POST | `/grades/by-student` | Resumo por aluno | CoordinatorOrTeacher |
| POST | `/grades/by-activity` | Resumo por atividade | CoordinatorOrTeacher |
| POST | `/grades/by-grade` | Resumo por turma | CoordinatorOrTeacher |
| POST | `/grades/my-students` | Notas dos dependentes | Guardian |

### SeedController (`/api/Seed`) - Dados de Teste

| Método | Rota | Descrição | Autorização |
|--------|------|-----------|-------------|
| POST | `/` | Popular banco com dados de teste | Público |
| DELETE | `/` | Limpar todos os dados | Público |

---

## Modelagem de Dados

> 📖 **Documentação Completa**: Veja [Estrutura e Fluxo do Sistema](docs/estrutura_sistema.md) para entender detalhadamente como o sistema funciona.

### Diagrama de Relacionamento

![Diagrama de Classes](docs/imagens/Diagram%20de%20Classes%20-%20Sisteped.png)

### Entidades

| Entidade | Descrição |
|----------|-----------|
| **User** | Usuários do sistema (Coordenador, Professor, Responsável) |
| **UserCredential** | Credenciais de autenticação |
| **Grid** | Grade curricular (estrutura curricular que agrupa turmas) |
| **Grade** | Turma (1º Ano A, 2º Ano B, etc.) - pertence a um Grid |
| **Class** | Matéria/Disciplina (Matemática, Português, etc.) |
| **Student** | Alunos |
| **Activity** | Atividade/Avaliação (vinculada a uma Class) |
| **Attendance** | Registros de frequência |
| **GridClass** | Relacionamento N:N entre Grade Curricular e Matéria |
| **ClassTeacher** | Relacionamento N:N entre Matéria e Professor |
| **StudentGrid** | Relacionamento N:N entre Aluno e Grade Curricular |
| **StudentActivity** | Relacionamento N:N entre Aluno e Atividade (com nota) |

### Relacionamentos

```
User (1) ────── (1) UserCredential
User (1) ────── (N) Student (como Guardian)
User (1) ────── (N) ClassTeacher (como Teacher)

Grid (1) ────── (N) GridGrade (Grade Curricular tem Turmas - N:N)
Grid (1) ────── (N) GridClass (Grade Curricular tem Matérias)
Grid (1) ────── (N) StudentGrid (Alunos vinculados à Grade Curricular)

Grade (1) ────── (N) GridGrade (Turma pode estar em várias Grades Curriculares - N:N)

Class (1) ────── (N) GridClass (Matéria em várias Grades Curriculares)
Class (1) ────── (N) ClassTeacher (Matéria tem Professores)
Class (1) ────── (N) Activity (Matéria tem Atividades)
Class (1) ────── (N) Attendance (Frequência é por Matéria)

Activity (1) ────── (N) StudentActivity (Atividade tem Notas)

Student (1) ────── (N) StudentGrid (Aluno em várias Grades Curriculares)
Student (1) ────── (N) Attendance (Frequência do aluno por matéria)
Student (1) ────── (N) StudentActivity (Notas do aluno)
```

### Hierarquia de Organização

```
GRID (Grade Curricular)
  ├── GRID_GRADES (Turmas vinculadas - N:N)
  │   └── GRADE (Turma)
  ├── GRID_CLASSES (Matérias da Grade Curricular)
  │   └── CLASS (Matéria)
  │       ├── CLASS_TEACHERS (Professores)
  │       ├── ACTIVITIES (Atividades)
  │       └── ATTENDANCES (Frequências)
  └── STUDENT_GRIDS (Alunos vinculados ao Grid)
      └── STUDENT
          ├── STUDENT_ACTIVITIES (Notas)
          └── ATTENDANCES (Frequências por matéria)
```

---

## Requisitos Funcionais

### Módulo ALU – Alunos
- **ALU-01:** Cadastro de alunos (adicionar, editar, remover)
- **ALU-02:** Histórico do aluno (notas e comportamentos)
- **ALU-03:** Busca e filtros de alunos

### Módulo TUR – Turmas
- **TUR-01:** Gerenciamento de turmas
- **TUR-02:** Visão geral da turma

### Módulo NOT – Notas
- **NOT-01:** Registro de notas
- **NOT-02:** Média e estatísticas
- **NOT-03:** Notas por conteúdo

### Módulo COM – Comportamentos
- **COM-01:** Registro de comportamentos
- **COM-02:** Métricas de frequência

### Módulo MON – Monitoramento
- **MON-01:** Análises acadêmicas
- **MON-02:** Análises comportamentais
- **MON-03:** Comparação temporal
- **MON-04:** Painel geral do professor

### Módulo REL – Relatórios
- **REL-01:** Exportação e relatórios
- **REL-02:** Relatório consolidado da turma

### Módulo SYS – Sistema
- **SYS-01:** Armazenamento local
- **SYS-02:** Backup local manual
- **SYS-03:** Tema claro/escuro

---

## Requisitos Não Funcionais

| Requisito | Métrica |
|-----------|---------|
| **Usabilidade** | Ações comuns ≤ 5 segundos, qualquer função em ≤ 3 cliques |
| **Performance** | Operações CRUD < 200ms, consultas com filtros < 1s |
| **Segurança** | Dados criptografados (AES-256), backup íntegro 100% |
| **Portabilidade** | Windows, Linux, macOS; telas de 768px a 4K |
| **Escalabilidade** | Novos módulos com impacto < 10%, suporte a 10k registros |

---

## Design (Protótipo)

- **Protótipo Figma:** [Sisteped no Figma](https://www.figma.com/design/WE4tHmzitXWEictT3dCRfe/Sisteped?node-id=0-1&p=f&t=riyhdJc2rPYz6J4X-0)

---

## Casos de Teste

### Cadastro de Usuário
1. ✅ Deve criar usuário com sucesso (201)
2. ✅ Deve retornar BadRequest quando dados inválidos (400)

### Login
1. ✅ Deve retornar token JWT no login bem-sucedido (200)
2. ✅ Deve retornar BadRequest quando dados inválidos (400)
3. ✅ Deve retornar Unauthorized quando credenciais incorretas (401)

### Autorização
1. ✅ Coordenador tem acesso total
2. ✅ Professor tem acesso intermediário
3. ✅ Responsável só vê seus dependentes

---

## Casos de Uso

Documentação completa em: `/docs/casos de uso/`

![Casos de Uso](docs/imagens/use%20cases.png)

---

## Licença

Este projeto está sob a licença especificada no arquivo [LICENSE](LICENSE).
