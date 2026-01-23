# 📝 Comandos para Gerar e Aplicar Migrations

## 🔄 Mudanças que serão aplicadas na Migration

1. ✅ Removida entidade `StudentGrid`
2. ✅ Criada entidade `StudentGrade` (Aluno x Turma)
3. ✅ Criada entidade `GridGrade` (Grid x Turma - N:N)
4. ✅ Removida relação direta `Grid -> Grade` (1:N)
5. ✅ Atualizados relacionamentos em todas as entidades

---

## 📋 Comandos

### 1️⃣ Navegar até a pasta do projeto

```powershell
cd sistepad-api\sisteped-api
```

### 2️⃣ Criar a Migration

```powershell
dotnet ef migrations add RestructureToGridGradeAndStudentGrade
```

**O que este comando faz:**
- Analisa todas as mudanças nos modelos
- Compara com o estado atual do banco
- Gera arquivos de migration com as alterações necessárias
- Cria arquivos em `Migrations/` com timestamp

### 3️⃣ Aplicar a Migration ao Banco de Dados

```powershell
dotnet ef database update
```

**O que este comando faz:**
- Aplica todas as migrations pendentes ao banco SQLite
- Cria/remove tabelas conforme necessário
- Atualiza o schema do banco de dados

---

## ⚠️ Importante: Backup de Dados

**Se você já tem dados no banco**, faça backup antes:

```powershell
# Copiar o arquivo do banco
Copy-Item sisteped.db sisteped.db.backup
```

---

## 🔍 Verificar Status das Migrations

Para ver quais migrations foram aplicadas:

```powershell
dotnet ef migrations list
```

---

## 🗑️ Reverter Migration (se necessário)

Se precisar reverter a última migration:

```powershell
dotnet ef database update <NomeDaMigrationAnterior>
```

Ou reverter todas:

```powershell
dotnet ef database update 0
```

---

## 📦 Comandos Completos (Sequência)

```powershell
# 1. Navegar até o projeto
cd sistepad-api\sisteped-api

# 2. (Opcional) Fazer backup do banco
Copy-Item sisteped.db sisteped.db.backup

# 3. Criar a migration
dotnet ef migrations add RestructureToGridGradeAndStudentGrade

# 4. Aplicar ao banco
dotnet ef database update

# 5. Verificar se foi aplicada
dotnet ef migrations list
```

---

## 🎯 Resultado Esperado

Após executar os comandos, você terá:

1. ✅ Nova migration criada em `Migrations/`
2. ✅ Banco de dados atualizado com:
   - Tabela `GridGrades` criada
   - Tabela `StudentGrades` criada
   - Tabela `StudentGrids` removida (se existia)
   - Coluna `GridId` removida da tabela `Grades` (se existia)
   - Todos os relacionamentos atualizados

---

## 🐛 Resolução de Problemas

### Erro: "No DbContext was found"
**Solução:** Certifique-se de estar na pasta `sistepad-api\sisteped-api`

### Erro: "Unable to create an object of type 'SistepedDbContext'"
**Solução:** Verifique se o `DbContext` está configurado corretamente no `Program.cs`

### Erro: "The process cannot access the file because it is being used by another process"
**Solução:** Feche o Visual Studio/IDE e qualquer processo que esteja usando o banco

### Migration com conflitos
**Solução:** 
1. Remova a última migration: `dotnet ef migrations remove`
2. Corrija os problemas
3. Crie novamente: `dotnet ef migrations add NomeDaMigration`

---

## ✅ Verificação Pós-Migration

Após aplicar a migration, verifique:

1. ✅ Tabela `GridGrades` existe
2. ✅ Tabela `StudentGrades` existe
3. ✅ Tabela `StudentGrids` não existe (se foi removida)
4. ✅ Tabela `Grades` não tem coluna `GridId` (se foi removida)

```powershell
# Verificar estrutura do banco (SQLite)
sqlite3 sisteped.db ".tables"
sqlite3 sisteped.db ".schema GridGrades"
sqlite3 sisteped.db ".schema StudentGrades"
```
