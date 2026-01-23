# 📋 Endpoints Completos da API Sisteped

## ✅ Checklist de Funcionalidades

### 🏗️ Configuração e Organização

#### Grid (Grade Curricular)
- ✅ `GET /api/Grid` - Listar todas as grades curriculares
- ✅ `GET /api/Grid/{id}` - Obter grade curricular por ID
- ✅ `POST /api/Grid` - Criar grade curricular
- ✅ `PUT /api/Grid/{id}` - Atualizar grade curricular
- ✅ `DELETE /api/Grid/{id}` - Excluir grade curricular
- ✅ `POST /api/Grid/add-grade` - Adicionar turma à grade curricular
- ✅ `POST /api/Grid/remove-grade/{gradeId}` - Remover turma da grade curricular

#### Grade (Turma)
- ✅ `GET /api/Grade` - Listar todas as turmas
- ✅ `GET /api/Grade/{id}` - Obter turma por ID
- ✅ `POST /api/Grade` - Criar turma
- ✅ `PUT /api/Grade/{id}` - Atualizar turma
- ✅ `DELETE /api/Grade/{id}` - Excluir turma

#### Class (Matéria)
- ✅ `GET /api/Class` - Listar todas as matérias
- ✅ `GET /api/Class/{id}` - Obter matéria por ID
- ✅ `POST /api/Class` - Criar matéria
- ✅ `PUT /api/Class/{id}` - Atualizar matéria
- ✅ `DELETE /api/Class/{id}` - Excluir matéria

#### GradeClass (Turma x Matéria)
- ✅ `GET /api/GradeClass` - Listar todos os relacionamentos
- ✅ `GET /api/GradeClass/{id}` - Obter relacionamento por ID
- ✅ `GET /api/GradeClass/by-grade/{gradeId}` - Matérias de uma turma
- ✅ `GET /api/GradeClass/by-class/{classId}` - Turmas de uma matéria
- ✅ `POST /api/GradeClass` - Vincular matéria à turma
- ✅ `DELETE /api/GradeClass/{id}` - Desvincular matéria da turma

#### ClassTeacher (Matéria x Professor)
- ✅ `GET /api/ClassTeacher` - Listar todas as atribuições
- ✅ `GET /api/ClassTeacher/{id}` - Obter atribuição por ID
- ✅ `GET /api/ClassTeacher/by-class/{classId}` - Professores de uma matéria
- ✅ `GET /api/ClassTeacher/by-teacher/{teacherId}` - Matérias de um professor
- ✅ `GET /api/ClassTeacher/my-classes` - Minhas matérias (Teacher)
- ✅ `POST /api/ClassTeacher` - Atribuir professor à matéria
- ✅ `DELETE /api/ClassTeacher/{id}` - Remover atribuição

---

### 👥 Gestão de Alunos

#### Student (Aluno)
- ✅ `GET /api/Student` - Listar todos os alunos
- ✅ `GET /api/Student/{id}` - Obter aluno por ID
- ✅ `GET /api/Student/by-guardian/{guardianId}` - Alunos por responsável
- ✅ `GET /api/Student/my-students` - Meus dependentes (Guardian)
- ✅ `POST /api/Student` - Criar aluno
- ✅ `PUT /api/Student/{id}` - Atualizar aluno
- ✅ `DELETE /api/Student/{id}` - Excluir aluno

#### StudentGrade (Matrícula)
- ✅ `GET /api/StudentGrade` - Listar todas as matrículas
- ✅ `GET /api/StudentGrade/{id}` - Obter matrícula por ID
- ✅ `GET /api/StudentGrade/by-student/{studentId}` - Turmas do aluno
- ✅ `GET /api/StudentGrade/by-grade/{gradeId}` - Alunos da turma
- ✅ `POST /api/StudentGrade` - Matricular aluno na turma
- ✅ `DELETE /api/StudentGrade/{id}` - Cancelar matrícula

---

### 📚 Atividades e Avaliações

#### Activity (Atividade)
- ✅ `GET /api/Activity` - Listar todas as atividades
- ✅ `GET /api/Activity/{id}` - Obter atividade por ID
- ✅ `GET /api/Activity/by-class/{classId}` - Atividades por matéria
- ✅ `GET /api/Activity/by-date-range?startDate={date}&endDate={date}` - Atividades por período
- ✅ `POST /api/Activity` - Criar atividade
- ✅ `PUT /api/Activity/{id}` - Atualizar atividade
- ✅ `DELETE /api/Activity/{id}` - Excluir atividade

#### StudentActivity (Notas)
- ✅ `GET /api/StudentActivity` - Listar todas as notas
- ✅ `GET /api/StudentActivity/{id}` - Obter nota por ID
- ✅ `GET /api/StudentActivity/by-student/{studentId}` - Notas por aluno
- ✅ `GET /api/StudentActivity/by-activity/{activityId}` - Notas por atividade
- ✅ `GET /api/StudentActivity/my-students` - Notas dos dependentes (Guardian)
- ✅ `POST /api/StudentActivity` - Lançar nota individual
- ✅ `POST /api/StudentActivity/bulk` - Lançar notas em lote
- ✅ `PUT /api/StudentActivity/{id}` - Atualizar nota
- ✅ `DELETE /api/StudentActivity/{id}` - Excluir nota

---

### 📊 Frequência

#### Attendance (Frequência)
- ✅ `GET /api/Attendance` - Listar todas as frequências
- ✅ `GET /api/Attendance/{id}` - Obter frequência por ID
- ✅ `GET /api/Attendance/by-student/{studentId}` - Frequências por aluno
- ✅ `GET /api/Attendance/by-grade/{gradeId}` - Frequências por turma
- ✅ `GET /api/Attendance/by-date/{date}` - Frequências por data
- ✅ `GET /api/Attendance/by-grade-and-date/{gradeId}/{date}` - Por turma e data
- ✅ `GET /api/Attendance/by-student-and-grade/{studentId}/{gradeId}` - Por aluno e turma
- ✅ `POST /api/Attendance` - Registrar frequência individual
- ✅ `POST /api/Attendance/bulk` - Registrar frequência em lote
- ✅ `DELETE /api/Attendance/{id}` - Excluir frequência

---

### 📈 Relatórios

#### Relatórios de Frequência
- ✅ `POST /api/Report/attendance` - Relatório detalhado de frequência (com filtros)
- ✅ `POST /api/Report/attendance/by-student` - Resumo de frequência por aluno
- ✅ `POST /api/Report/attendance/by-grade` - Resumo de frequência por turma
- ✅ `POST /api/Report/attendance/my-students` - Frequência dos dependentes (Guardian)

#### Relatórios de Notas
- ✅ `POST /api/Report/grades` - Relatório detalhado de notas (com filtros)
- ✅ `POST /api/Report/grades/by-student` - Resumo de notas por aluno
- ✅ `POST /api/Report/grades/by-activity` - Resumo de notas por atividade
- ✅ `POST /api/Report/grades/by-grade` - Resumo de notas por turma
- ✅ `POST /api/Report/grades/my-students` - Notas dos dependentes (Guardian)

---

### 🔐 Autenticação e Usuários

#### User
- ✅ `POST /api/User/login` - Autenticação (público)
- ✅ `POST /api/User/create` - Criar usuário
- ✅ `GET /api/User/{id}/details` - Obter usuário por ID
- ✅ `GET /api/User/get-all` - Listar todos usuários (CoordinatorOnly)
- ✅ `GET /api/User/teachers` - Listar professores (CoordinatorOnly)
- ✅ `GET /api/User/guardians` - Listar responsáveis (CoordinatorOrTeacher)

---

### 🧪 Testes e Seed

#### Seed
- ✅ `POST /api/Seed` - Popular banco com dados de teste
- ✅ `DELETE /api/Seed` - Limpar todos os dados

---

## 🔍 Filtros Disponíveis nos Relatórios

### AttendanceReportFilterDTO
- `StudentId` - Filtrar por aluno
- `GradeId` - Filtrar por turma
- `GridId` - Filtrar por grade curricular
- `TeacherId` - Filtrar por professor
- `Shift` - Filtrar por turno
- `ClassId` - Filtrar por matéria
- `StartDate` - Data inicial
- `EndDate` - Data final

### GradeReportFilterDTO
- `StudentId` - Filtrar por aluno
- `ActivityId` - Filtrar por atividade
- `ClassId` - Filtrar por matéria
- `GradeId` - Filtrar por turma
- `GridId` - Filtrar por grade curricular
- `TeacherId` - Filtrar por professor
- `StartDate` - Data inicial
- `EndDate` - Data final

---

## ⚠️ Endpoints que PODERIAM ser úteis (mas não essenciais)

### Sugestões de Melhorias Futuras:

1. **GET /api/Grid/{id}/grades** - Listar todas as turmas de uma grade curricular
   - **Status**: Não implementado, mas pode ser obtido via `GET /api/Grid/{id}` que já retorna as turmas

2. **GET /api/Grade/{id}/students** - Listar todos os alunos de uma turma
   - **Status**: Não implementado, mas pode ser obtido via `GET /api/StudentGrade/by-grade/{gradeId}`

3. **GET /api/Grade/{id}/activities** - Listar todas as atividades de uma turma
   - **Status**: Não implementado, mas pode ser obtido via:
     - `GET /api/GradeClass/by-grade/{gradeId}` para obter as matérias
     - `GET /api/Activity/by-class/{classId}` para cada matéria

4. **POST /api/Report/school-summary** - Relatório consolidado geral da escola
   - **Status**: Não implementado
   - **Sugestão**: Criar endpoint que retorne:
     - Total de alunos
     - Total de turmas
     - Total de professores
     - Média geral de frequência
     - Média geral de notas
     - Estatísticas por grade curricular

5. **GET /api/Report/export/{format}** - Exportar relatórios (PDF, Excel, CSV)
   - **Status**: Não implementado
   - **Sugestão**: Adicionar funcionalidade de exportação

---

## ✅ Conclusão

**Você TEM todos os endpoints essenciais para:**
- ✅ Criar e organizar grades curriculares (Grids)
- ✅ Criar e organizar turmas (Grades)
- ✅ Criar e organizar matérias (Classes)
- ✅ Vincular matérias às turmas (GradeClass)
- ✅ Atribuir professores às matérias (ClassTeacher)
- ✅ Cadastrar e gerenciar alunos (Student)
- ✅ Matricular alunos nas turmas (StudentGrade)
- ✅ Criar atividades (Activity)
- ✅ Lançar notas (StudentActivity)
- ✅ Registrar frequência (Attendance)
- ✅ Gerar relatórios de frequência e notas com filtros avançados

**Endpoints opcionais que poderiam ser adicionados:**
- Relatório consolidado geral da escola
- Exportação de relatórios (PDF/Excel)
- Endpoints de conveniência para listagens específicas (mas já podem ser obtidos via filtros)

---

## 📝 Nota

Todos os endpoints estão funcionais e cobrem todas as operações CRUD necessárias, além de relatórios detalhados com múltiplos filtros. O sistema está completo para uso em produção!
