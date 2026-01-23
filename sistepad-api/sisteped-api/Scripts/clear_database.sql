-- =====================================================
-- SCRIPT PARA LIMPAR O BANCO DE DADOS SISTEPED
-- Execute este script no SQLite para remover todos os dados
-- =====================================================

-- Desabilita verificação de FK temporariamente
PRAGMA foreign_keys = OFF;

-- Limpa todas as tabelas na ordem correta (por causa das FKs)
DELETE FROM StudentActivities;
DELETE FROM Activities;
DELETE FROM Attendances;
DELETE FROM StudentGrades;
DELETE FROM ClassTeachers;
DELETE FROM GridClasses;
DELETE FROM GridGrades;
DELETE FROM Students;
DELETE FROM Grades;
DELETE FROM Grids;
DELETE FROM Classes;
DELETE FROM UserCredentials;
DELETE FROM Users;

-- Reseta os auto-incrementos
DELETE FROM sqlite_sequence WHERE name IN ('Users', 'UserCredentials', 'Grids', 'Grades', 'Classes', 'GridGrades', 'GridClasses', 'ClassTeachers', 'Students', 'StudentGrades', 'Attendances', 'Activities', 'StudentActivities');

-- Reabilita verificação de FK
PRAGMA foreign_keys = ON;

SELECT 'Banco de dados limpo com sucesso!' AS Resultado;
