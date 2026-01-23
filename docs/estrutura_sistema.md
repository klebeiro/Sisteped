# Estrutura e Fluxo do Sistema Sisteped

## 📋 Visão Geral

O **Sisteped** é um sistema de gestão pedagógica que permite gerenciar turmas, alunos, frequência e notas de atividades escolares.

---

## 🏗️ Arquitetura em Camadas

```
┌─────────────────────────────────────┐
│   CONTROLLERS (API Endpoints)       │  ← Recebe requisições HTTP
│   - Validação de entrada            │
│   - Autorização por role            │
│   - Retorno de respostas            │
├─────────────────────────────────────┤
│   SERVICES (Lógica de Negócio)      │  ← Regras de negócio
│   - Validações complexas            │
│   - Cálculos e agregações          │
│   - Orquestração de operações      │
├─────────────────────────────────────┤
│   REPOSITORIES (Acesso a Dados)     │  ← Queries e persistência
│   - Entity Framework Core          │
│   - Include/ThenInclude            │
│   - Filtros dinâmicos              │
├─────────────────────────────────────┤
│   MODELS (Entidades do Domínio)     │  ← Estrutura de dados
│   - Propriedades                    │
│   - Relacionamentos                │
│   - Navigation Properties           │
└─────────────────────────────────────┘
```

---

## 🗂️ Hierarquia de Organização

### Estrutura Principal

```
GRID (Grade Curricular)
  └── Estrutura curricular que agrupa turmas por ano
      │
      └── GRADES (Turmas)
          ├── 1º Ano A - Manhã
          ├── 1º Ano B - Tarde
          ├── 2º Ano A - Manhã
          └── ...
          │
          ├── GRADE_CLASSES (Matérias da turma)
          │   └── CLASS (Matéria)
          │       ├── Code, Name, WorkloadHours
          │       │
          │       ├── ACTIVITIES (Atividades/Avaliações)
          │       │   ├── Title, Description
          │       │   ├── ApplicationDate
          │       │   ├── MaxScore
          │       │   │
          │       │   └── STUDENT_ACTIVITIES (Notas)
          │       │       ├── Score
          │       │       └── Remarks
          │       │
          │       └── CLASS_TEACHERS (Professores)
          │           └── USER (Teacher)
          │
          ├── STUDENT_GRADES (Alunos da turma)
          │   └── STUDENT
          │       ├── Enrollment, Name, BirthDate
          │       ├── GuardianId (USER como Guardian)
          │       │
          │       ├── STUDENT_ACTIVITIES (Notas)
          │       └── ATTENDANCES (Frequência)
          │           ├── Date
          │           └── Present (bool)
          │
          └── ATTENDANCES (Registros de frequência)
```

---

## 📊 Modelagem de Dados

### Entidades Principais

| Entidade | Descrição | Atributos Principais |
|----------|-----------|---------------------|
| **Grid** | Grade Curricular | Year, Name, Status |
| **Grade** | Turma | Name, Level, Shift, GridId |
| **Class** | Matéria/Disciplina | Code, Name, WorkloadHours |
| **Student** | Aluno | Enrollment, Name, BirthDate, GuardianId |
| **Activity** | Atividade/Avaliação | Title, Description, ClassId, ApplicationDate, MaxScore |
| **Attendance** | Frequência | StudentId, GradeId, Date, Present |
| **User** | Usuário | Email, Name, Role (Coordinator/Teacher/Guardian) |

### Tabelas de Relacionamento (N:N)

| Tabela | Relaciona | Propósito |
|--------|-----------|-----------|
| **GradeClass** | Grade ↔ Class | Matérias de cada turma |
| **ClassTeacher** | Class ↔ User | Professores de cada matéria |
| **StudentGrade** | Student ↔ Grade | Matrícula do aluno na turma |
| **StudentActivity** | Student ↔ Activity | Nota do aluno na atividade |

---

## 🔄 Fluxo de Operações

### 1. Configuração Inicial (Coordenador)

```
1. Criar Grid (Grade Curricular)
   POST /api/Grid
   → Ex: "Grade Curricular 2025"

2. Criar Grades (Turmas) e vincular ao Grid
   POST /api/Grade (com GridId)
   → Ex: "1º Ano A - Manhã" → Grid 2025

3. Criar Classes (Matérias)
   POST /api/Class
   → Ex: "Matemática", "Português"

4. Vincular Matérias às Turmas
   POST /api/GradeClass
   → Grade 1 (1º Ano A) + Class 1 (Matemática)

5. Atribuir Professores às Matérias
   POST /api/ClassTeacher
   → Class 1 (Matemática) + User 2 (Professor)

6. Cadastrar Alunos
   POST /api/Student
   → Vincula a um Guardian (User)

7. Matricular Alunos nas Turmas
   POST /api/StudentGrade
   → Student 1 + Grade 1 (1º Ano A)
```

### 2. Operações Diárias (Professor/Coordenador)

#### Registrar Frequência

```
POST /api/Attendance/bulk
{
  "gradeId": 1,
  "date": "2025-01-19",
  "students": [
    { "studentId": 1, "present": true },
    { "studentId": 2, "present": false }
  ]
}
```

#### Criar Atividade

```
POST /api/Activity
{
  "title": "Prova de Matemática - 1º Bimestre",
  "description": "Avaliação sobre números",
  "classId": 1,  // Matemática
  "applicationDate": "2025-01-20",
  "maxScore": 10
}
```

#### Lançar Notas

```
POST /api/StudentActivity/bulk
{
  "activityId": 1,
  "scores": [
    { "studentId": 1, "score": 9.5 },
    { "studentId": 2, "score": 7.0 }
  ]
}
```

### 3. Consultas e Relatórios

#### Relatório de Frequência

```
POST /api/Report/attendance
{
  "gradeId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```

#### Relatório de Notas

```
POST /api/Report/grades
{
  "classId": 1,  // Matemática
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```

---

## 🔐 Sistema de Autorização

### Roles e Permissões

| Role | Acesso | Operações Permitidas |
|------|--------|---------------------|
| **Coordinator** | Total | CRUD completo em todas as entidades |
| **Teacher** | Intermediário | Criar/ler/atualizar: Frequência, Atividades, Notas |
| **Guardian** | Limitado | Apenas visualização dos próprios dependentes |

### Políticas de Autorização

- `CoordinatorOnly`: Apenas Coordenador
- `CoordinatorOrTeacher`: Coordenador ou Professor
- `AllAuthenticated`: Qualquer usuário autenticado

---

## 📝 Relacionamentos Detalhados

### Grid → Grade (1:N)
- Uma Grade Curricular contém várias Turmas
- Grade tem `GridId` (FK para Grid)

### Grade → Class (N:N via GradeClass)
- Uma Turma tem várias Matérias
- Uma Matéria pode estar em várias Turmas
- Tabela intermediária: `GradeClass`

### Class → Activity (1:N)
- Uma Matéria tem várias Atividades
- Activity tem `ClassId` (FK para Class)

### Student → Grade (N:N via StudentGrade)
- Um Aluno pode estar em várias Turmas
- Uma Turma tem vários Alunos
- Tabela intermediária: `StudentGrade`

### Student → Activity (N:N via StudentActivity)
- Um Aluno tem notas em várias Atividades
- Uma Atividade tem notas de vários Alunos
- Tabela intermediária: `StudentActivity` (com Score e Remarks)

### Class → User (N:N via ClassTeacher)
- Uma Matéria tem vários Professores
- Um Professor leciona várias Matérias
- Tabela intermediária: `ClassTeacher`

### Student → User (N:1)
- Um Aluno tem um Responsável (Guardian)
- Student tem `GuardianId` (FK para User)

---

## 🎯 Exemplo Prático Completo

### Cenário: Professor lança nota de prova

1. **Professor faz login**
   ```
   POST /api/User/login
   → Recebe JWT token
   ```

2. **Busca atividades da matéria**
   ```
   GET /api/Activity/by-class/1
   → Lista atividades de Matemática
   ```

3. **Seleciona atividade e lança notas**
   ```
   POST /api/StudentActivity/bulk
   {
     "activityId": 1,
     "scores": [
       { "studentId": 1, "score": 9.5, "remarks": "Excelente!" },
       { "studentId": 2, "score": 7.0 }
     ]
   }
   ```

4. **Sistema valida e persiste**
   - ✅ Alunos existem?
   - ✅ Nota ≤ MaxScore da atividade?
   - ✅ Não existe nota duplicada?
   - → Salva em `StudentActivities`

5. **Responsável consulta notas**
   ```
   POST /api/Report/grades/my-students
   → Vê apenas notas dos seus dependentes
   ```

---

## 📁 Estrutura de Pastas

```
sisteped-api/
├── Controllers/          # Endpoints HTTP (13 controllers)
├── Services/             # Lógica de negócio
│   └── Interfaces/       # Contratos
├── Repositories/         # Acesso a dados
│   └── Interfaces/       # Contratos
├── Models/               # Entidades do domínio
│   └── Enums/            # UserRole
├── DTOs/                 # Objetos de transferência
│   ├── Request/          # Dados de entrada
│   └── Response/          # Dados de saída
├── Validators/           # Validações FluentValidation
├── Infra/Data/           # Entity Framework
│   ├── EntityConfigurations/
│   └── SistepedDbContext.cs
├── Configuration/        # AutoMapper
├── IoC/                  # Dependency Injection
└── Scripts/              # SQL de seed
```

---

## 🔄 Fluxo de Dados Típico

```
Cliente HTTP
    ↓
Controller (valida entrada, autoriza)
    ↓
Service (aplica regras de negócio)
    ↓
Repository (executa query no banco)
    ↓
Entity Framework Core
    ↓
SQLite/SQL Server
    ↓
Repository (retorna entidades)
    ↓
Service (mapeia para DTOs, calcula agregações)
    ↓
Controller (retorna JSON)
    ↓
Cliente HTTP
```

---

## ✅ Checklist de Operações

### Para Coordenador:
- [ ] Criar Grade Curricular (Grid)
- [ ] Criar Turmas (Grades) e vincular ao Grid
- [ ] Criar Matérias (Classes)
- [ ] Vincular Matérias às Turmas (GradeClass)
- [ ] Atribuir Professores às Matérias (ClassTeacher)
- [ ] Cadastrar Alunos
- [ ] Matricular Alunos nas Turmas (StudentGrade)

### Para Professor:
- [ ] Registrar Frequência (Attendance)
- [ ] Criar Atividades (Activity)
- [ ] Lançar Notas (StudentActivity)
- [ ] Consultar Relatórios

### Para Responsável:
- [ ] Visualizar Frequência dos dependentes
- [ ] Visualizar Notas dos dependentes

---

## 🎓 Conceitos Importantes

- **Grid**: Estrutura curricular (ex: "Grade 2025")
- **Grade**: Turma (ex: "1º Ano A - Manhã")
- **Class**: Matéria (ex: "Matemática")
- **Activity**: Atividade/Avaliação (ex: "Prova de Matemática")
- **Attendance**: Registro de frequência (presente/ausente)
- **StudentActivity**: Nota do aluno em uma atividade

---

## 📌 Pontos de Atenção

1. **Activities são vinculadas a Classes**, não diretamente a Grades
2. **Frequência é por Grade (turma)**, não por Class
3. **Grid agrupa Grades**, não Classes diretamente
4. **Classes são associadas às Grades** através de GradeClass
5. **Um aluno pode estar em múltiplas turmas** (através de StudentGrade)

---

Esta estrutura permite flexibilidade para:
- Múltiplas grades curriculares por ano
- Múltiplas turmas por grade
- Múltiplas matérias por turma
- Múltiplos professores por matéria
- Múltiplas atividades por matéria
- Controle completo de frequência e notas
