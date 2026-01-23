# 📚 Estrutura Completa do Sistema Sisteped

## 🎯 Visão Geral

O **Sisteped** é um sistema de gestão escolar que permite gerenciar turmas, alunos, matérias, atividades, frequências e gerar relatórios completos. O sistema segue uma hierarquia bem definida onde **Grades Curriculares (Grids)** organizam **Turmas (Grades)**, que por sua vez têm **Matérias (Classes)** associadas.

---

## 🏗️ Hierarquia e Estrutura de Dados

### Fluxo Principal

```
GRID (Grade Curricular)
  │
  ├── GRID_GRADES (Turmas vinculadas - N:N)
  │   └── GRADE (Turma) - Ex: "1º Ano A - Manhã", "2º Ano B - Tarde"
  │
  ├── GRID_CLASSES (Matérias da Grade Curricular)
  │   └── CLASS (Matéria)
  │       ├── CLASS_TEACHERS (Professores)
  │       ├── ACTIVITIES (Atividades/Avaliações)
  │       └── ATTENDANCES (Frequências)
  │
  └── STUDENT_GRIDS (Alunos vinculados ao Grid)
      └── STUDENT
          ├── STUDENT_ACTIVITIES (Notas)
          └── ATTENDANCES (Frequências por matéria)
```

### Entidades Principais

| Entidade | Descrição | Relacionamentos |
|----------|-----------|-----------------|
| **Grid** | Grade Curricular (ex: "Grade 2025") | Tem GridGrades, GridClasses, StudentGrids |
| **Grade** | Turma (ex: "1º Ano A") | Tem GridGrades (pode estar em várias Grades Curriculares) |
| **GridGrade** | Relacionamento N:N entre Grade Curricular e Turma | - |
| **Class** | Matéria/Disciplina (ex: "Matemática") | Tem GridClasses, ClassTeachers, Activities, Attendances |
| **Student** | Aluno | Tem StudentGrids, Attendances, StudentActivities |
| **User** | Usuário (Coordenador/Professor/Responsável) | Pode ser Guardian, Teacher, ou Coordinator |
| **Activity** | Atividade/Avaliação | Pertence a uma Class, tem StudentActivities |
| **Attendance** | Frequência | Vinculado a Student e Class (não mais a Grade!) |

---

## 🔄 Fluxo de Uso do Sistema

### 1️⃣ Configuração Inicial (Coordenador)

#### Passo 1: Criar Grade Curricular (Grid)
```http
POST /api/Grid
{
  "year": 2025,
  "name": "Grade Curricular 2025",
  "status": true
}
```
**Resultado:** Grade curricular criada (ex: ID = 1)

#### Passo 2: Criar Turmas (Grades)
```http
POST /api/Grade
{
  "name": "1º Ano A",
  "level": 1,
  "shift": 1,  // 1=Manhã, 2=Tarde, 3=Noite
  "status": true
}
```
**Resultado:** Turma criada

#### Passo 2.1: Vincular Turma ao Grid
```http
POST /api/GridGrade
{
  "gridId": 1,    // Grade Curricular
  "gradeId": 1    // Turma
}
```
**Resultado:** Turma vinculada à Grade Curricular

#### Passo 3: Criar Matérias (Classes)
```http
POST /api/Class
{
  "code": "MAT001",
  "name": "Matemática",
  "workloadHours": 80,
  "status": true
}
```
**Resultado:** Matéria criada

#### Passo 4: Vincular Matérias às Grades Curriculares
```http
POST /api/GridClass
{
  "gridId": 1,    // Grade Curricular
  "classId": 1    // Matéria
}
```
**Resultado:** Matéria agora faz parte da Grade Curricular

#### Passo 5: Atribuir Professores às Matérias
```http
POST /api/ClassTeacher
{
  "classId": 1,     // Matemática
  "teacherId": 2    // ID do Professor
}
```
**Resultado:** Professor atribuído à matéria

#### Passo 6: Cadastrar Alunos
```http
POST /api/Student
{
  "enrollment": "2025001",
  "name": "Pedro Oliveira",
  "birthDate": "2015-03-15",
  "guardianId": 4,  // ID do Responsável
  "status": true
}
```
**Resultado:** Aluno cadastrado

#### Passo 7: Vincular Alunos às Grades Curriculares
```http
POST /api/StudentGrid
{
  "studentId": 1,
  "gridId": 1  // Grade Curricular
}
```
**Resultado:** Aluno vinculado à Grade Curricular (e consequentemente às matérias dela)

---

### 2️⃣ Operações Diárias

#### Registrar Frequência (por Matéria)
```http
POST /api/Attendance/bulk
{
  "classId": 1,  // Matemática
  "date": "2025-01-19",
  "students": [
    { "studentId": 1, "present": true },
    { "studentId": 2, "present": false }
  ]
}
```

#### Criar Atividade/Avaliação
```http
POST /api/Activity
{
  "title": "Prova de Matemática - 1º Bimestre",
  "description": "Avaliação sobre números e operações",
  "classId": 1,  // Matemática
  "applicationDate": "2025-01-20",
  "maxScore": 10,
  "status": true
}
```

#### Lançar Notas
```http
POST /api/StudentActivity/bulk
{
  "activityId": 1,
  "students": [
    { "studentId": 1, "score": 9.5, "remarks": "Excelente!" },
    { "studentId": 2, "score": 7.0, "remarks": "Pode melhorar" }
  ]
}
```

---

## 📋 Endpoints Completos por Funcionalidade

### 🔐 Autenticação (`/api/User`)
- `POST /api/User/register` - Registrar novo usuário
- `POST /api/User/login` - Fazer login (retorna JWT)
- `GET /api/User/{id}/details` - Obter detalhes do usuário

### 📚 Grades Curriculares (`/api/Grid`)
- `GET /api/Grid` - Listar todas
- `GET /api/Grid/{id}` - Obter por ID
- `POST /api/Grid` - Criar grade curricular
- `PUT /api/Grid/{id}` - Atualizar
- `DELETE /api/Grid/{id}` - Excluir
- `POST /api/Grid/add-grade` - Adicionar turma à grade curricular
- `POST /api/Grid/remove-grade/{gradeId}` - Remover turma

### 🏫 Turmas (`/api/Grade`)
- `GET /api/Grade` - Listar todas
- `GET /api/Grade/{id}` - Obter por ID
- `POST /api/Grade` - Criar turma
- `PUT /api/Grade/{id}` - Atualizar
- `DELETE /api/Grade/{id}` - Excluir

### 📖 Matérias (`/api/Class`)
- `GET /api/Class` - Listar todas
- `GET /api/Class/{id}` - Obter por ID
- `POST /api/Class` - Criar matéria
- `PUT /api/Class/{id}` - Atualizar
- `DELETE /api/Class/{id}` - Excluir

### 🔗 Grade Curricular x Turma (`/api/GridGrade`)
- `GET /api/GridGrade` - Listar todos os vínculos
- `GET /api/GridGrade/{id}` - Obter por ID
- `GET /api/GridGrade/by-grid/{gridId}` - Turmas de uma grade curricular
- `GET /api/GridGrade/by-grade/{gradeId}` - Grades curriculares de uma turma
- `POST /api/GridGrade` - Vincular turma à grade curricular
- `DELETE /api/GridGrade/{id}` - Desvincular

### 🔗 Grade Curricular x Matéria (`/api/GridClass`)
- `GET /api/GridClass` - Listar todos os vínculos
- `GET /api/GridClass/{id}` - Obter por ID
- `GET /api/GridClass/by-grid/{gridId}` - Matérias de uma grade curricular
- `GET /api/GridClass/by-class/{classId}` - Grades curriculares de uma matéria
- `POST /api/GridClass` - Vincular matéria à grade curricular
- `DELETE /api/GridClass/{id}` - Desvincular

### 👨‍🏫 Matéria x Professor (`/api/ClassTeacher`)
- `GET /api/ClassTeacher` - Listar todos
- `GET /api/ClassTeacher/{id}` - Obter por ID
- `GET /api/ClassTeacher/by-class/{classId}` - Professores de uma matéria
- `GET /api/ClassTeacher/by-teacher/{teacherId}` - Matérias de um professor
- `POST /api/ClassTeacher` - Atribuir professor à matéria
- `DELETE /api/ClassTeacher/{id}` - Remover atribuição

### 👨‍🎓 Alunos (`/api/Student`)
- `GET /api/Student` - Listar todos
- `GET /api/Student/{id}` - Obter por ID
- `GET /api/Student/by-guardian/{guardianId}` - Alunos de um responsável
- `POST /api/Student` - Cadastrar aluno
- `PUT /api/Student/{id}` - Atualizar
- `DELETE /api/Student/{id}` - Excluir

### 🔗 Aluno x Grade Curricular (`/api/StudentGrid`)
- `GET /api/StudentGrid` - Listar todos
- `GET /api/StudentGrid/{id}` - Obter por ID
- `GET /api/StudentGrid/by-student/{studentId}` - Grades curriculares do aluno
- `GET /api/StudentGrid/by-grid/{gridId}` - Alunos da grade curricular
- `POST /api/StudentGrid` - Vincular aluno à grade curricular
- `DELETE /api/StudentGrid/{id}` - Desvincular

### ✅ Frequência (`/api/Attendance`)
- `GET /api/Attendance` - Listar todas
- `GET /api/Attendance/{id}` - Obter por ID
- `GET /api/Attendance/by-student/{studentId}` - Frequências por aluno
- `GET /api/Attendance/by-class/{classId}` - Frequências por matéria
- `GET /api/Attendance/by-date/{date}` - Frequências por data
- `GET /api/Attendance/by-class-and-date/{classId}/{date}` - Por matéria e data
- `GET /api/Attendance/by-student-and-class/{studentId}/{classId}` - Por aluno e matéria
- `POST /api/Attendance` - Registrar frequência individual
- `POST /api/Attendance/bulk` - Registrar frequência em lote
- `DELETE /api/Attendance/{id}` - Excluir registro

### 📝 Atividades (`/api/Activity`)
- `GET /api/Activity` - Listar todas
- `GET /api/Activity/{id}` - Obter por ID
- `GET /api/Activity/by-class/{classId}` - Atividades por matéria
- `GET /api/Activity/by-date-range` - Atividades por período
- `POST /api/Activity` - Criar atividade
- `PUT /api/Activity/{id}` - Atualizar
- `DELETE /api/Activity/{id}` - Excluir

### 📊 Notas (`/api/StudentActivity`)
- `GET /api/StudentActivity` - Listar todas
- `GET /api/StudentActivity/{id}` - Obter por ID
- `GET /api/StudentActivity/by-student/{studentId}` - Notas por aluno
- `GET /api/StudentActivity/by-activity/{activityId}` - Notas por atividade
- `GET /api/StudentActivity/my-students` - Notas dos dependentes (Responsável)
- `POST /api/StudentActivity` - Lançar nota individual
- `POST /api/StudentActivity/bulk` - Lançar notas em lote
- `PUT /api/StudentActivity/{id}` - Atualizar nota
- `DELETE /api/StudentActivity/{id}` - Excluir nota

### 📈 Relatórios (`/api/Report`)

#### Relatórios de Frequência
- `POST /api/Report/attendance` - Relatório detalhado de frequência (com filtros)
- `POST /api/Report/attendance/by-student` - Resumo por aluno
- `POST /api/Report/attendance/by-grade` - Resumo por matéria

#### Relatórios de Notas
- `POST /api/Report/grades` - Relatório detalhado de notas (com filtros)
- `POST /api/Report/grades/by-student` - Resumo por aluno
- `POST /api/Report/grades/by-activity` - Resumo por atividade
- `POST /api/Report/grades/by-grade` - Resumo por turma

---

## 🎯 Filtros Disponíveis nos Relatórios

### Filtros de Frequência (`AttendanceReportFilterDTO`)
```json
{
  "studentId": 1,           // ID do aluno
  "enrollment": "2025001", // Matrícula
  "teacherId": 2,          // ID do professor
  "gradeId": 1,            // ID da turma
  "shift": 1,              // Turno (1=Manhã, 2=Tarde, 3=Noite)
  "gridId": 1,             // ID da grade curricular
  "classId": 1,            // ID da matéria
  "startDate": "2025-01-01",
  "endDate": "2025-01-31",
  "present": true          // true=presente, false=ausente
}
```

### Filtros de Notas (`GradeReportFilterDTO`)
```json
{
  "studentId": 1,
  "enrollment": "2025001",
  "gradeId": 1,            // ID da turma
  "activityId": 1,          // ID da atividade
  "teacherId": 2,
  "shift": 1,
  "gridId": 1,
  "classId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31",
  "minScore": 7.0,
  "maxScore": 10.0
}
```

---

## 🔐 Permissões por Perfil

### 👑 Coordenador (Coordinator)
- **Acesso Total:** Todos os endpoints
- **Pode:** Criar, editar, excluir qualquer entidade
- **Responsabilidades:** Configuração inicial, gestão completa

### 👨‍🏫 Professor (Teacher)
- **Pode:** 
  - Ver todas as informações
  - Criar e editar frequências
  - Criar e editar atividades
  - Lançar e atualizar notas
- **Não pode:** Excluir entidades principais, gerenciar usuários

### 👨‍👩‍👧 Responsável (Guardian)
- **Pode:**
  - Ver apenas seus dependentes (alunos vinculados)
  - Ver frequências dos dependentes
  - Ver notas dos dependentes
  - Ver relatórios dos dependentes
- **Não pode:** Criar, editar ou excluir dados

---

## 📊 Exemplos de Fluxos Completos

### Exemplo 1: Configurar Ano Letivo Completo

1. **Criar Grade Curricular 2025**
   ```http
   POST /api/Grid
   { "year": 2025, "name": "Grade Curricular 2025" }
   ```

2. **Criar Turmas**
   ```http
   POST /api/Grade
   { "name": "1º Ano A", "level": 1, "shift": 1 }
   POST /api/Grade
   { "name": "1º Ano B", "level": 1, "shift": 2 }
   ```

3. **Vincular Turmas ao Grid**
   ```http
   POST /api/GridGrade
   { "gridId": 1, "gradeId": 1 }  // 1º Ano A
   POST /api/GridGrade
   { "gridId": 1, "gradeId": 2 }  // 1º Ano B
   ```

4. **Criar Matérias**
   ```http
   POST /api/Class
   { "code": "MAT001", "name": "Matemática", "workloadHours": 80 }
   POST /api/Class
   { "code": "POR001", "name": "Português", "workloadHours": 80 }
   ```

5. **Vincular Matérias ao Grid**
   ```http
   POST /api/GridClass
   { "gridId": 1, "classId": 1 }  // Matemática
   POST /api/GridClass
   { "gridId": 1, "classId": 2 }  // Português
   ```

6. **Atribuir Professores**
   ```http
   POST /api/ClassTeacher
   { "classId": 1, "teacherId": 2 }  // Matemática → Prof. João
   POST /api/ClassTeacher
   { "classId": 2, "teacherId": 3 }  // Português → Prof. Ana
   ```

7. **Cadastrar e Vincular Alunos**
   ```http
   POST /api/Student
   { "enrollment": "2025001", "name": "Pedro", "guardianId": 4 }
   
   POST /api/StudentGrid
   { "studentId": 1, "gridId": 1 }  // Pedro → Grade 2025
   ```

### Exemplo 2: Registrar Frequência Diária

```http
POST /api/Attendance/bulk
{
  "classId": 1,  // Matemática
  "date": "2025-01-19",
  "students": [
    { "studentId": 1, "present": true },
    { "studentId": 2, "present": true },
    { "studentId": 3, "present": false }
  ]
}
```

### Exemplo 3: Criar Prova e Lançar Notas

1. **Criar Atividade**
   ```http
   POST /api/Activity
   {
     "title": "Prova de Matemática - 1º Bimestre",
     "classId": 1,
     "applicationDate": "2025-01-20",
     "maxScore": 10
   }
   ```

2. **Lançar Notas**
   ```http
   POST /api/StudentActivity/bulk
   {
     "activityId": 1,
     "students": [
       { "studentId": 1, "score": 9.5 },
       { "studentId": 2, "score": 7.0 },
       { "studentId": 3, "score": 8.5 }
     ]
   }
   ```

### Exemplo 4: Gerar Relatório de Frequência

```http
POST /api/Report/attendance
{
  "gridId": 1,
  "classId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```

**Resposta:**
```json
{
  "totalRecords": 150,
  "totalPresent": 135,
  "totalAbsent": 15,
  "attendancePercentage": 90.0,
  "items": [
    {
      "attendanceId": 1,
      "date": "2025-01-19",
      "present": true,
      "studentId": 1,
      "studentName": "Pedro Oliveira",
      "enrollment": "2025001",
      "classId": 1,
      "className": "Matemática",
      "gridId": 1,
      "gridName": "Grade Curricular 2025"
    }
    // ... mais itens
  ]
}
```

---

## ✅ Checklist de Endpoints

### ✅ Configuração
- [x] Criar/Listar/Atualizar/Excluir Grids
- [x] Criar/Listar/Atualizar/Excluir Grades
- [x] Criar/Listar/Atualizar/Excluir Classes
- [x] Vincular Turmas às Grades Curriculares (GridGrade) - N:N
- [x] Vincular Matérias às Grades Curriculares (GridClass)
- [x] Atribuir Professores às Matérias (ClassTeacher)

### ✅ Gestão de Alunos
- [x] Criar/Listar/Atualizar/Excluir Students
- [x] Vincular Alunos às Grades Curriculares (StudentGrid)
- [x] Buscar alunos por responsável

### ✅ Frequência
- [x] Registrar frequência individual
- [x] Registrar frequência em lote
- [x] Listar frequências por aluno
- [x] Listar frequências por matéria
- [x] Listar frequências por data
- [x] Listar frequências por aluno e matéria

### ✅ Atividades e Notas
- [x] Criar/Listar/Atualizar/Excluir Activities
- [x] Listar atividades por matéria
- [x] Listar atividades por período
- [x] Lançar nota individual
- [x] Lançar notas em lote
- [x] Atualizar notas
- [x] Listar notas por aluno
- [x] Listar notas por atividade

### ✅ Relatórios
- [x] Relatório detalhado de frequência (com filtros)
- [x] Resumo de frequência por aluno
- [x] Resumo de frequência por matéria
- [x] Relatório detalhado de notas (com filtros)
- [x] Resumo de notas por aluno
- [x] Resumo de notas por atividade
- [x] Resumo de notas por turma

---

## 🎯 Conclusão

O sistema possui **TODOS os endpoints necessários** para:

1. ✅ **Criar e organizar** Grades Curriculares, Turmas, Matérias
2. ✅ **Gerenciar** Alunos, Professores, Responsáveis
3. ✅ **Registrar** Frequências e Atividades
4. ✅ **Lançar** Notas
5. ✅ **Gerar** Relatórios completos de frequência e desempenho

A estrutura está completa e pronta para uso! 🚀
