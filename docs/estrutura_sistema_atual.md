# 📚 Estrutura Completa do Sistema Sisteped - Análise Atual

## 🎯 Visão Geral

O **Sisteped** é um sistema de gestão escolar que permite gerenciar turmas, alunos, matérias, atividades, frequências e gerar relatórios completos.

---

## 🏗️ Hierarquia e Estrutura de Dados

### Fluxo Principal (CORRETO)

```
GRID (Grade Curricular)
  │
  ├── GRID_GRADES (N:N) ──→ GRADE (Turma)
  │                           │
  │                           └── STUDENT_GRADES (N:N) ──→ STUDENT (Aluno)
  │
  └── GRID_CLASSES (N:N) ──→ CLASS (Matéria)
                                │
                                ├── CLASS_TEACHERS (N:N) ──→ USER (Professor)
                                ├── ACTIVITIES
                                │   └── STUDENT_ACTIVITIES (N:N) ──→ STUDENT
                                └── ATTENDANCES ──→ STUDENT
```

### Entidades Principais

| Entidade | Descrição | Relacionamentos |
|----------|-----------|-----------------|
| **Grid** | Grade Curricular (ex: "Grade 2025") | Tem GridGrades, GridClasses |
| **Grade** | Turma (ex: "1º Ano A") | Tem GridGrades, StudentGrades |
| **GridGrade** | Relacionamento N:N entre Grid e Grade | - |
| **StudentGrade** | Relacionamento N:N entre Student e Grade | - |
| **Class** | Matéria/Disciplina (ex: "Matemática") | Tem GridClasses, ClassTeachers, Activities, Attendances |
| **Student** | Aluno | Tem StudentGrades, Attendances, StudentActivities |
| **User** | Usuário (Coordenador/Professor/Responsável) | Pode ser Guardian, Teacher, ou Coordinator |
| **Activity** | Atividade/Avaliação | Pertence a uma Class, tem StudentActivities |
| **Attendance** | Frequência | Vinculado a Student e Class |
| **StudentActivity** | Nota do aluno em uma atividade | Vincula Student e Activity |

---

## 🔄 Fluxo de Uso do Sistema (CORRETO)

### 1️⃣ Configuração Inicial (Coordenador)

#### Passo 1: Criar Turma (Grade)
```http
POST /api/Grade
{
  "name": "1º Ano A",
  "level": 1,
  "shift": 1,  // 1=Manhã, 2=Tarde, 3=Noite
  "status": true
}
```
**Resultado:** Turma criada (ex: ID = 1)

#### Passo 2: Criar Grade Curricular (Grid)
```http
POST /api/Grid
{
  "year": 2025,
  "name": "Grade Curricular 2025",
  "status": true
}
```
**Resultado:** Grade curricular criada (ex: ID = 1)

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

#### Passo 4: Vincular Matérias ao Grid
```http
POST /api/GridClass
{
  "gridId": 1,    // Grade Curricular
  "classId": 1    // Matéria
}
```
**Resultado:** Matéria agora faz parte da Grade Curricular

#### Passo 5: Vincular Grid à Turma
```http
POST /api/GridGrade
{
  "gridId": 1,    // Grade Curricular
  "gradeId": 1    // Turma
}
```
**Resultado:** Turma agora tem acesso às matérias do Grid

#### Passo 6: Atribuir Professores às Matérias
```http
POST /api/ClassTeacher
{
  "classId": 1,     // Matemática
  "teacherId": 2    // ID do Professor
}
```

#### Passo 7: Cadastrar Alunos
```http
POST /api/Student
{
  "enrollment": "2025001",
  "name": "Pedro Oliveira",
  "birthDate": "2015-03-15",
  "guardianId": 4,
  "status": true
}
```

#### Passo 8: Relacionar Alunos à Turma
```http
POST /api/StudentGrade
{
  "studentId": 1,
  "gradeId": 1  // Turma
}
```
**Resultado:** Aluno vinculado à turma (e consequentemente às matérias do Grid da turma)

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

## 📋 Controllers Disponíveis

### ✅ Controllers Corretos

1. **UserController** (`/api/User`)
   - Registro e autenticação
   - ✅ CORRETO

2. **GradeController** (`/api/Grade`)
   - CRUD de Turmas
   - ✅ CORRETO

3. **GridController** (`/api/Grid`)
   - CRUD de Grades Curriculares
   - ✅ CORRETO

4. **ClassController** (`/api/Class`)
   - CRUD de Matérias
   - ✅ CORRETO

5. **GridGradeController** (`/api/GridGrade`)
   - Vincular Grid à Turma (N:N)
   - ✅ CORRETO

6. **GridClassController** (`/api/GridClass`)
   - Vincular Matérias ao Grid (N:N)
   - ✅ CORRETO

7. **ClassTeacherController** (`/api/ClassTeacher`)
   - Atribuir Professores às Matérias
   - ✅ CORRETO

8. **StudentController** (`/api/Student`)
   - CRUD de Alunos
   - ✅ CORRETO

9. **StudentGradeController** (`/api/StudentGrade`)
   - Vincular Alunos às Turmas (N:N)
   - ✅ CORRETO

10. **AttendanceController** (`/api/Attendance`)
    - Registrar Frequências
    - ✅ CORRETO

11. **ActivityController** (`/api/Activity`)
    - CRUD de Atividades
    - ✅ CORRETO

12. **StudentActivityController** (`/api/StudentActivity`)
    - Lançar Notas
    - ✅ CORRETO

13. **ReportController** (`/api/Report`)
    - Relatórios de Frequência e Notas
    - ✅ CORRETO

14. **SeedController** (`/api/Seed`)
    - Popular e limpar banco de dados
    - ✅ CORRETO

### ⚠️ Controller Obsoleto (DEVE SER REMOVIDO)

15. **StudentGridController** (`/api/StudentGrid`)
    - ❌ OBSOLETO - Substituído por StudentGradeController
    - Alunos agora são vinculados às Turmas, não aos Grids diretamente
    - **AÇÃO NECESSÁRIA:** Remover este controller e todos os arquivos relacionados

---

## 🔍 Análise de Consistência

### ✅ Pontos Corretos

1. **Modelos de Dados:**
   - ✅ Grid não tem mais StudentGrids (correto)
   - ✅ Grade tem StudentGrades (correto)
   - ✅ Student tem StudentGrades (correto)
   - ✅ Attendance vinculado a Class (correto)

2. **DbContext:**
   - ✅ Tem StudentGrade (correto)
   - ✅ Não tem StudentGrid (correto)

3. **Repositories:**
   - ✅ ReportRepository usa StudentGrade (correto)
   - ✅ GridRepository não usa StudentGrids (correto)

4. **Services:**
   - ✅ GridService calcula TotalStudents via GridGrades → Grade → StudentGrades (correto)
   - ✅ ReportService usa StudentGrade (correto)

5. **DependencyInjection:**
   - ✅ Registra IStudentGradeService (correto)
   - ⚠️ Ainda registra IStudentGridService (PRECISA REMOVER)

### ❌ Problemas Encontrados

1. **Arquivos Obsoletos que Precisam ser Removidos:**
   - `StudentGridController.cs`
   - `StudentGridService.cs`
   - `IStudentGridService.cs`
   - `StudentGridRepository.cs`
   - `IStudentGridRepository.cs`
   - `StudentGridConfiguration.cs`
   - `StudentGrid.cs` (Model)
   - `StudentGridDTO.cs`
   - `StudentGridResponseDTO.cs`
   - `StudentGridValidator.cs`

2. **DependencyInjection:**
   - Ainda registra `IStudentGridService` e `IStudentGridRepository` (precisa remover)

---

## 🎯 Estrutura Final Esperada

### Relacionamentos Corretos

```
User (1) ────── (1) UserCredential
User (1) ────── (N) Student (como Guardian)
User (1) ────── (N) ClassTeacher (como Teacher)

Grid (1) ────── (N) GridGrade (N:N) ────── (1) Grade
Grid (1) ────── (N) GridClass (N:N) ────── (1) Class

Grade (1) ────── (N) StudentGrade (N:N) ────── (1) Student

Class (1) ────── (N) ClassTeacher (N:N) ────── (1) User (Teacher)
Class (1) ────── (N) Activity
Class (1) ────── (N) Attendance

Activity (1) ────── (N) StudentActivity (N:N) ────── (1) Student
Attendance (N) ────── (1) Student
Attendance (N) ────── (1) Class
```

---

## 📊 Fluxo de Dados para Relatórios

### Como o Sistema Acessa os Dados

1. **Para obter alunos de um Grid:**
   ```
   Grid → GridGrades → Grade → StudentGrades → Student
   ```

2. **Para obter matérias de um Grid:**
   ```
   Grid → GridClasses → Class
   ```

3. **Para obter frequências de alunos de uma turma:**
   ```
   Grade → StudentGrades → Student → Attendances → Class
   ```

4. **Para obter notas de alunos de uma turma:**
   ```
   Grade → StudentGrades → Student → StudentActivities → Activity → Class
   ```

---

## ✅ Checklist de Correções Necessárias

- [ ] Remover `StudentGridController.cs`
- [ ] Remover `StudentGridService.cs` e `IStudentGridService.cs`
- [ ] Remover `StudentGridRepository.cs` e `IStudentGridRepository.cs`
- [ ] Remover `StudentGridConfiguration.cs`
- [ ] Remover `StudentGrid.cs` (Model)
- [ ] Remover `StudentGridDTO.cs` e `StudentGridResponseDTO.cs`
- [ ] Remover `StudentGridValidator.cs`
- [ ] Remover registros de `IStudentGridService` e `IStudentGridRepository` do `DependencyInjection.cs`
- [ ] Verificar se há outras referências a `StudentGrid` no código

---

## 🎯 Conclusão

A estrutura principal está **CORRETA** e segue o fluxo desejado:
1. ✅ Criar Turma
2. ✅ Criar Grid
3. ✅ Criar Classes e vincular ao Grid
4. ✅ Vincular Grid à Turma
5. ✅ Relacionar Alunos à Turma

**AÇÃO NECESSÁRIA:** Remover todos os arquivos relacionados a `StudentGrid`, pois foram substituídos por `StudentGrade`.
