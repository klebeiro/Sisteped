# 📊 Rotas de Exportação CSV - ReportController

## ✅ Novas Rotas Implementadas

### 📋 Relatórios de Frequência

#### 1. Exportar Relatório Detalhado de Frequência
```http
POST /api/Report/attendance/export-csv
Content-Type: application/json

{
  "studentId": 1,
  "classId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```
**Resposta:** Arquivo CSV com nome `relatorio_frequencia_YYYYMMDD_HHMMSS.csv`

**Conteúdo:**
- Estatísticas gerais (Total de Registros, Presentes, Ausentes, Percentual)
- Dados detalhados: ID, Data, Presente, Aluno, Matéria, Grade Curricular, Responsável

---

#### 2. Exportar Resumo de Frequência por Aluno
```http
POST /api/Report/attendance/by-student/export-csv
Content-Type: application/json

{
  "gridId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```
**Resposta:** Arquivo CSV com nome `resumo_frequencia_alunos_YYYYMMDD_HHMMSS.csv`

**Conteúdo:**
- ID Aluno, Nome, Matrícula
- Total Registros, Total Presente, Total Ausente, Percentual

---

#### 3. Exportar Resumo de Frequência por Turma
```http
POST /api/Report/attendance/by-grade/export-csv
Content-Type: application/json

{
  "gridId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```
**Resposta:** Arquivo CSV com nome `resumo_frequencia_turmas_YYYYMMDD_HHMMSS.csv`

**Conteúdo:**
- ID Turma, Nome, Nível, Turno
- Total Registros, Total Presente, Total Ausente, Percentual

---

### 📊 Relatórios de Notas

#### 4. Exportar Relatório Detalhado de Notas
```http
POST /api/Report/grades/export-csv
Content-Type: application/json

{
  "studentId": 1,
  "classId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```
**Resposta:** Arquivo CSV com nome `relatorio_notas_YYYYMMDD_HHMMSS.csv`

**Conteúdo:**
- Estatísticas gerais (Total de Registros, Avaliados, Pendentes, Média, Maior/Menor Nota)
- Dados detalhados: Nota, Observações, Aluno, Atividade, Matéria, Turma, Grade Curricular, Responsável

---

#### 5. Exportar Resumo de Notas por Aluno
```http
POST /api/Report/grades/by-student/export-csv
Content-Type: application/json

{
  "gridId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```
**Resposta:** Arquivo CSV com nome `resumo_notas_alunos_YYYYMMDD_HHMMSS.csv`

**Conteúdo:**
- ID Aluno, Nome, Matrícula
- Total Atividades, Avaliadas, Pendentes, Média, Maior/Menor Nota

---

#### 6. Exportar Resumo de Notas por Atividade
```http
POST /api/Report/grades/by-activity/export-csv
Content-Type: application/json

{
  "classId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```
**Resposta:** Arquivo CSV com nome `resumo_notas_atividades_YYYYMMDD_HHMMSS.csv`

**Conteúdo:**
- ID Atividade, Título, Data Aplicação, Nota Máxima
- Matéria, Total Alunos, Avaliados, Pendentes, Média, Maior/Menor Nota

---

#### 7. Exportar Resumo de Notas por Turma
```http
POST /api/Report/grades/by-grade/export-csv
Content-Type: application/json

{
  "gridId": 1,
  "startDate": "2025-01-01",
  "endDate": "2025-01-31"
}
```
**Resposta:** Arquivo CSV com nome `resumo_notas_turmas_YYYYMMDD_HHMMSS.csv`

**Conteúdo:**
- ID Turma, Nome, Turno
- Total Atividades, Total Alunos, Média, Maior/Menor Nota

---

## 🔐 Permissões

### Coordenadores e Professores
- ✅ Acesso completo a todas as rotas de exportação
- ✅ Podem exportar dados de qualquer aluno/turma/matéria

### Responsáveis
- ✅ Podem exportar apenas relatórios dos próprios dependentes
- ✅ Rotas disponíveis:
  - `POST /api/Report/attendance/export-csv` (filtrado automaticamente)
  - `POST /api/Report/grades/export-csv` (filtrado automaticamente)

---

## 📝 Formato do CSV

### Características
- ✅ Encoding: UTF-8 (suporta acentos e caracteres especiais)
- ✅ Separador: Vírgula (`,`)
- ✅ Campos com vírgulas/aspas são escapados automaticamente
- ✅ Nome do arquivo inclui timestamp para evitar conflitos
- ✅ Headers descritivos em português

### Exemplo de Uso

```javascript
// JavaScript/TypeScript
const response = await fetch('/api/Report/attendance/export-csv', {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
    'Authorization': 'Bearer ' + token
  },
  body: JSON.stringify({
    gridId: 1,
    startDate: '2025-01-01',
    endDate: '2025-01-31'
  })
});

const blob = await response.blob();
const url = window.URL.createObjectURL(blob);
const a = document.createElement('a');
a.href = url;
a.download = 'relatorio_frequencia.csv';
a.click();
```

---

## ✅ Checklist de Implementação

- [x] Helper `CsvHelper` criado
- [x] Métodos de conversão para todos os tipos de relatório
- [x] Rotas de exportação adicionadas ao ReportController
- [x] Validação de permissões (Coordenador/Professor/Responsável)
- [x] Nomes de arquivo com timestamp
- [x] Encoding UTF-8 configurado
- [x] Escape de campos CSV implementado
- [x] Headers descritivos em português

---

## 🎯 Resumo das Rotas

| Rota | Método | Descrição | Permissão |
|------|--------|-----------|-----------|
| `/api/Report/attendance/export-csv` | POST | Exporta relatório detalhado de frequência | Autenticado (com validação) |
| `/api/Report/attendance/by-student/export-csv` | POST | Exporta resumo por aluno | CoordinatorOrTeacher |
| `/api/Report/attendance/by-grade/export-csv` | POST | Exporta resumo por turma | CoordinatorOrTeacher |
| `/api/Report/grades/export-csv` | POST | Exporta relatório detalhado de notas | Autenticado (com validação) |
| `/api/Report/grades/by-student/export-csv` | POST | Exporta resumo por aluno | CoordinatorOrTeacher |
| `/api/Report/grades/by-activity/export-csv` | POST | Exporta resumo por atividade | CoordinatorOrTeacher |
| `/api/Report/grades/by-grade/export-csv` | POST | Exporta resumo por turma | CoordinatorOrTeacher |

---

## 🚀 Pronto para Uso!

Todas as rotas estão implementadas e prontas para exportar os relatórios como CSV! 🎉
