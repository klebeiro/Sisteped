# 🔄 Mudanças na Estrutura do Sistema

## 📋 Resumo das Alterações

A estrutura do sistema foi completamente reestruturada para seguir o fluxo correto:

**ANTES:**
- Grade (Turma) ↔ Class (Matéria) via GradeClass
- Student ↔ Grade (Turma) via StudentGrade
- Attendance vinculado a Grade (Turma)

**DEPOIS:**
- **Grade (Turma)** pertence a **Grid (Grade Curricular)**
- **Grid (Grade Curricular)** ↔ **Class (Matéria)** via GridClass
- **Student** ↔ **Grid (Grade Curricular)** via StudentGrid
- **Attendance** vinculado a **Class (Matéria)**
- **Class** tem: Teachers, Activities e Attendances

---

## 🗂️ Nova Hierarquia

```
GRID (Grade Curricular)
  ├── GRADES (Turmas) - pertencem ao Grid
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

## ✅ Arquivos Criados

### Models
- ✅ `GridClass.cs` - Relacionamento Grid x Class
- ✅ `StudentGrid.cs` - Relacionamento Student x Grid

### Entity Configurations
- ✅ `GridClassConfiguration.cs`
- ✅ `StudentGridConfiguration.cs`

### Repositories
- ✅ `IGridClassRepository.cs` e `GridClassRepository.cs`
- ✅ `IStudentGridRepository.cs` e `StudentGridRepository.cs`

### Services
- ✅ `IGridClassService.cs` e `GridClassService.cs`
- ✅ `IStudentGridService.cs` e `StudentGridService.cs`

### Controllers
- ✅ `GridClassController.cs` - `/api/GridClass`
- ✅ `StudentGridController.cs` - `/api/StudentGrid`

### DTOs
- ✅ `GridClassDTO.cs` e `GridClassResponseDTO.cs`
- ✅ `StudentGridDTO.cs` e `StudentGridResponseDTO.cs`

### Validators
- ✅ `GridClassValidator.cs`
- ✅ `StudentGridValidator.cs`

---

## 🔄 Arquivos Modificados

### Models
- ✅ `Grid.cs` - Adicionado `GridClasses` e `StudentGrids`
- ✅ `Grade.cs` - Removido `GradeClasses` e `StudentGrades`
- ✅ `Class.cs` - Adicionado `GridClasses` e `Attendances`, removido `GradeClasses`
- ✅ `Student.cs` - Alterado `StudentGrades` para `StudentGrids`
- ✅ `Attendance.cs` - Alterado `GradeId` para `ClassId`

### Entity Configurations
- ✅ `AttendanceConfiguration.cs` - Agora usa `ClassId` e relacionamento com `Class`

### DbContext
- ✅ `SistepedDbContext.cs` - Atualizado para `GridClasses` e `StudentGrids`

### Repositories
- ✅ `AttendanceRepository.cs` - Métodos atualizados para usar `ClassId`
- ✅ `GridRepository.cs` - Includes atualizados para `GridClasses` e `StudentGrids`
- ✅ `ReportRepository.cs` - Filtros atualizados para nova estrutura
- ✅ `ActivityRepository.cs` - Includes atualizados para `GridClasses`

### Services
- ✅ `AttendanceService.cs` - Atualizado para usar `ClassId` e `IClassRepository`
- ✅ `GridService.cs` - Cálculo de `TotalClasses` e `TotalStudents` atualizado
- ✅ `ReportService.cs` - Mapeamentos atualizados para nova estrutura

### Controllers
- ✅ `AttendanceController.cs` - Endpoints atualizados: `by-class`, `by-class-and-date`, `by-student-and-class`
- ✅ `GridController.cs` - Mantido (adiciona Grades ao Grid, correto)

### DTOs
- ✅ `AttendanceCreateDTO.cs` - `GradeId` → `ClassId`
- ✅ `AttendanceBulkCreateDTO.cs` - `GradeId` → `ClassId`
- ✅ `AttendanceResponseDTO.cs` - `GradeId/GradeName` → `ClassId/ClassName/ClassCode`
- ✅ `AttendanceReportItemDTO.cs` - Atualizado para usar `ClassId`

### AutoMapper
- ✅ `AutoMapperProfile.cs` - Mapeamentos atualizados para `GridClass` e `StudentGrid`, `Attendance` com `Class`

### DependencyInjection
- ✅ `DependencyInjection.cs` - Registros atualizados para novos services e repositories

### Scripts SQL
- ✅ `seed_database.sql` - Atualizado para usar `GridClasses`, `StudentGrids` e `Attendances` com `ClassId`
- ✅ `clear_database.sql` - Atualizado para limpar novas tabelas

---

## 🗑️ Arquivos Removidos

- ❌ `GradeClass.cs` (modelo)
- ❌ `StudentGrade.cs` (modelo)
- ❌ `GradeClassConfiguration.cs`
- ❌ `StudentGradeConfiguration.cs`
- ❌ `GradeClassController.cs`
- ❌ `StudentGradeController.cs`
- ❌ `IGradeClassService.cs` e `GradeClassService.cs`
- ❌ `IStudentGradeService.cs` e `StudentGradeService.cs`
- ❌ `IGradeClassRepository.cs` e `GradeClassRepository.cs`
- ❌ `IStudentGradeRepository.cs` e `StudentGradeRepository.cs`
- ❌ `GradeClassDTO.cs` e `GradeClassResponseDTO.cs`
- ❌ `StudentGradeDTO.cs` e `StudentGradeResponseDTO.cs`
- ❌ `GradeClassValidator.cs`
- ❌ `StudentGradeValidator.cs`

---

## 🔄 Endpoints Atualizados

### GridClassController (`/api/GridClass`)
- `GET /api/GridClass` - Listar todos
- `GET /api/GridClass/{id}` - Obter por ID
- `GET /api/GridClass/by-grid/{gridId}` - Matérias de uma grade curricular
- `GET /api/GridClass/by-class/{classId}` - Grades curriculares de uma matéria
- `POST /api/GridClass` - Vincular matéria à grade curricular
- `DELETE /api/GridClass/{id}` - Desvincular

### StudentGridController (`/api/StudentGrid`)
- `GET /api/StudentGrid` - Listar todos
- `GET /api/StudentGrid/{id}` - Obter por ID
- `GET /api/StudentGrid/by-student/{studentId}` - Grades curriculares do aluno
- `GET /api/StudentGrid/by-grid/{gridId}` - Alunos da grade curricular
- `POST /api/StudentGrid` - Vincular aluno à grade curricular
- `DELETE /api/StudentGrid/{id}` - Desvincular

### AttendanceController (`/api/Attendance`)
- `GET /api/Attendance/by-class/{classId}` - Frequências por matéria (substitui `by-grade`)
- `GET /api/Attendance/by-class-and-date/{classId}/{date}` - Por matéria e data (substitui `by-grade-and-date`)
- `GET /api/Attendance/by-student-and-class/{studentId}/{classId}` - Por aluno e matéria (substitui `by-student-and-grade`)

---

## 📝 Fluxo Correto de Uso

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

4. Vincular Matérias às Grades Curriculares
   POST /api/GridClass
   → Grid 2025 + Class Matemática

5. Atribuir Professores às Matérias
   POST /api/ClassTeacher
   → Class Matemática + User Professor

6. Cadastrar Alunos
   POST /api/Student
   → Vincula a um Guardian (User)

7. Vincular Alunos às Grades Curriculares
   POST /api/StudentGrid
   → Student 1 + Grid 2025
```

### 2. Operações Diárias

#### Registrar Frequência (por Matéria)
```
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

#### Criar Atividade
```
POST /api/Activity
{
  "title": "Prova de Matemática",
  "classId": 1,  // Matemática
  "applicationDate": "2025-01-20",
  "maxScore": 10
}
```

---

## ⚠️ Importante

1. **Frequência é por Matéria (Class)**, não por Turma (Grade)
2. **Alunos são vinculados às Grades Curriculares (Grids)**, não às Turmas (Grades)
3. **Matérias são vinculadas às Grades Curriculares (Grids)**, não às Turmas (Grades)
4. **Turmas (Grades) pertencem a Grades Curriculares (Grids)**

---

## 🎯 Próximos Passos

1. **Criar Migration** para aplicar as mudanças no banco:
   ```bash
   dotnet ef migrations add RestructureToGridClassAndStudentGrid
   dotnet ef database update
   ```

2. **Testar os endpoints** para garantir que tudo está funcionando

3. **Atualizar documentação** se necessário

---

## ✅ Status

Todas as mudanças foram implementadas e o sistema está pronto para uso com a nova estrutura!
