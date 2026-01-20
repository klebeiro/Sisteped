-- =====================================================
-- SCRIPT DE SEED DO BANCO DE DADOS SISTEPED
-- Execute este script no SQLite para popular o banco
-- ATENÇÃO: Este script limpa todos os dados existentes!
-- =====================================================

-- Desabilita verificação de FK temporariamente
PRAGMA foreign_keys = OFF;

-- =====================================================
-- LIMPA TODAS AS TABELAS (ordem correta para FK)
-- =====================================================
DELETE FROM StudentActivities;
DELETE FROM Activities;
DELETE FROM Attendances;
DELETE FROM StudentGrades;
DELETE FROM ClassTeachers;
DELETE FROM GradeClasses;
DELETE FROM Students;
DELETE FROM Grades;
DELETE FROM Grids;
DELETE FROM Classes;
DELETE FROM UserCredentials;
DELETE FROM Users;

-- Reseta os auto-incrementos
DELETE FROM sqlite_sequence WHERE name IN ('Users', 'UserCredentials', 'Grids', 'Grades', 'Classes', 'GradeClasses', 'ClassTeachers', 'Students', 'StudentGrades', 'Attendances', 'Activities', 'StudentActivities');

-- Reabilita verificação de FK
PRAGMA foreign_keys = ON;

-- =====================================================
-- USUÁRIOS
-- Senha para todos: Senha@123
-- Hash gerado com PBKDF2 (formato: hash:salt em Base64)
-- =====================================================

-- Coordenador (Role = 1)
INSERT INTO Users (Id, Email, Name, Role, Status, CreatedAt) VALUES 
(1, 'coordenador@escola.com', 'Maria Silva - Coordenadora', 1, 1, datetime('now'));

INSERT INTO UserCredentials (UserId, PasswordHash, Role) VALUES 
(1, 'xK8Ht5Yw2Zp0Qr3Jm6Nv9Bc4Df7Gh1Kl=:aB2cD4eF6gH8iJ0k', 1);

-- Professor 1 (Role = 2)
INSERT INTO Users (Id, Email, Name, Role, Status, CreatedAt) VALUES 
(2, 'professor1@escola.com', 'João Santos - Professor de Matemática', 2, 1, datetime('now'));

INSERT INTO UserCredentials (UserId, PasswordHash, Role) VALUES 
(2, 'yL9Iu6Zx3Aq1Rs4Kn7Ow0Cd5Eg8Hj2Lm=:bC3dE5fG7hI9jK1l', 2);

-- Professor 2 (Role = 2)
INSERT INTO Users (Id, Email, Name, Role, Status, CreatedAt) VALUES 
(3, 'professor2@escola.com', 'Ana Costa - Professora de Português', 2, 1, datetime('now'));

INSERT INTO UserCredentials (UserId, PasswordHash, Role) VALUES 
(3, 'zM0Jv7Ay4Br2St5Lo8Px1De6Fi9Ik3Nn=:cD4eF6gH8iJ0kL2m', 2);

-- Responsável 1 (Role = 3)
INSERT INTO Users (Id, Email, Name, Role, Status, CreatedAt) VALUES 
(4, 'responsavel1@email.com', 'Carlos Oliveira', 3, 1, datetime('now'));

INSERT INTO UserCredentials (UserId, PasswordHash, Role) VALUES 
(4, 'aN1Kw8Bz5Cs3Tu6Mp9Qy2Ef7Gj0Hl4Oo=:dE5fG7hI9jK1lM3n', 3);

-- Responsável 2 (Role = 3)
INSERT INTO Users (Id, Email, Name, Role, Status, CreatedAt) VALUES 
(5, 'responsavel2@email.com', 'Fernanda Lima', 3, 1, datetime('now'));

INSERT INTO UserCredentials (UserId, PasswordHash, Role) VALUES 
(5, 'bO2Lx9Ca6Dt4Uv7Nq0Rz3Fg8Hk1Im5Pp=:eF6gH8iJ0kL2mN4o', 3);

-- Responsável 3 (Role = 3)
INSERT INTO Users (Id, Email, Name, Role, Status, CreatedAt) VALUES 
(6, 'responsavel3@email.com', 'Roberto Almeida', 3, 1, datetime('now'));

INSERT INTO UserCredentials (UserId, PasswordHash, Role) VALUES 
(6, 'cP3My0Db7Eu5Vw8Or1Sa4Gh9Il2Jn6Qq=:fG7hI9jK1lM3nO5p', 3);

-- =====================================================
-- GRIDS (Grades Curriculares)
-- =====================================================
INSERT INTO Grids (Id, Year, Name, Status, CreatedAt) VALUES 
(1, 2025, 'Grade Curricular 2025', 1, datetime('now')),
(2, 2026, 'Grade Curricular 2026', 1, datetime('now'));

-- =====================================================
-- GRADES (Séries/Turmas)
-- Shift: 1 = Manhã, 2 = Tarde, 3 = Noite
-- =====================================================
INSERT INTO Grades (Id, Name, Level, Shift, Status, GridId, CreatedAt) VALUES 
(1, '1º Ano A', 1, 1, 1, 1, datetime('now')),  -- Manhã, Grid 2025
(2, '1º Ano B', 1, 2, 1, 1, datetime('now')),  -- Tarde, Grid 2025
(3, '2º Ano A', 2, 1, 1, 1, datetime('now')),  -- Manhã, Grid 2025
(4, '3º Ano A', 3, 3, 1, 2, datetime('now')),  -- Noite, Grid 2026
(5, '2º Ano B', 2, 2, 1, 1, datetime('now'));  -- Tarde, Grid 2025

-- =====================================================
-- CLASSES (Matérias/Disciplinas)
-- =====================================================
INSERT INTO Classes (Id, Code, Name, WorkloadHours, Status, CreatedAt) VALUES 
(1, 'MAT001', 'Matemática', 80, 1, datetime('now')),
(2, 'POR001', 'Português', 80, 1, datetime('now')),
(3, 'CIE001', 'Ciências', 60, 1, datetime('now')),
(4, 'HIS001', 'História', 40, 1, datetime('now')),
(5, 'GEO001', 'Geografia', 40, 1, datetime('now')),
(6, 'ING001', 'Inglês', 40, 1, datetime('now')),
(7, 'ART001', 'Artes', 30, 1, datetime('now'));

-- =====================================================
-- GRADE_CLASSES (Relacionamento Série x Matéria)
-- =====================================================
INSERT INTO GradeClasses (Id, GradeId, ClassId, CreatedAt) VALUES 
-- 1º Ano A (Série 1)
(1, 1, 1, datetime('now')),   -- Matemática
(2, 1, 2, datetime('now')),   -- Português
(3, 1, 3, datetime('now')),   -- Ciências
(4, 1, 7, datetime('now')),   -- Artes

-- 1º Ano B (Série 2)
(5, 2, 1, datetime('now')),   -- Matemática
(6, 2, 2, datetime('now')),   -- Português
(7, 2, 3, datetime('now')),   -- Ciências

-- 2º Ano A (Série 3)
(8, 3, 1, datetime('now')),   -- Matemática
(9, 3, 2, datetime('now')),   -- Português
(10, 3, 4, datetime('now')),  -- História
(11, 3, 5, datetime('now')),  -- Geografia

-- 3º Ano A (Série 4)
(12, 4, 1, datetime('now')),  -- Matemática
(13, 4, 2, datetime('now')),  -- Português
(14, 4, 5, datetime('now')),  -- Geografia
(15, 4, 6, datetime('now')),  -- Inglês

-- 2º Ano B (Série 5)
(16, 5, 1, datetime('now')),  -- Matemática
(17, 5, 2, datetime('now')),  -- Português
(18, 5, 4, datetime('now'));  -- História

-- =====================================================
-- CLASS_TEACHERS (Relacionamento Matéria x Professor)
-- Professor 1 (Id=2): Matemática, Ciências
-- Professor 2 (Id=3): Português, História, Geografia, Inglês, Artes
-- =====================================================
INSERT INTO ClassTeachers (Id, ClassId, TeacherId, CreatedAt) VALUES 
(1, 1, 2, datetime('now')),  -- Matemática -> Professor 1
(2, 3, 2, datetime('now')),  -- Ciências -> Professor 1
(3, 2, 3, datetime('now')),  -- Português -> Professor 2
(4, 4, 3, datetime('now')),  -- História -> Professor 2
(5, 5, 3, datetime('now')),  -- Geografia -> Professor 2
(6, 6, 3, datetime('now')),  -- Inglês -> Professor 2
(7, 7, 3, datetime('now'));  -- Artes -> Professor 2

-- =====================================================
-- STUDENTS (Alunos)
-- Responsável 1 (Id=4): Pedro e Laura Oliveira
-- Responsável 2 (Id=5): Gabriel Lima e Beatriz Santos
-- Responsável 3 (Id=6): Sofia e Lucas Almeida
-- =====================================================
INSERT INTO Students (Id, Enrollment, Name, BirthDate, GuardianId, Status, CreatedAt) VALUES 
(1, '2025001', 'Pedro Oliveira', '2015-03-15', 4, 1, datetime('now')),
(2, '2025002', 'Laura Oliveira', '2016-07-22', 4, 1, datetime('now')),
(3, '2025003', 'Gabriel Lima', '2015-01-10', 5, 1, datetime('now')),
(4, '2025004', 'Sofia Almeida', '2014-11-05', 6, 1, datetime('now')),
(5, '2025005', 'Lucas Almeida', '2013-08-30', 6, 1, datetime('now')),
(6, '2025006', 'Beatriz Santos', '2015-04-18', 5, 1, datetime('now')),
(7, '2025007', 'Miguel Costa', '2014-09-12', 4, 1, datetime('now')),
(8, '2025008', 'Julia Ferreira', '2015-06-25', 5, 1, datetime('now'));

-- =====================================================
-- STUDENT_GRADES (Relacionamento Aluno x Série)
-- =====================================================
INSERT INTO StudentGrades (Id, StudentId, GradeId, CreatedAt) VALUES 
-- 1º Ano A (Série 1): Pedro, Laura, Beatriz
(1, 1, 1, datetime('now')),  -- Pedro -> 1º Ano A
(2, 2, 1, datetime('now')),  -- Laura -> 1º Ano A
(3, 6, 1, datetime('now')),  -- Beatriz -> 1º Ano A

-- 1º Ano B (Série 2): Gabriel
(4, 3, 2, datetime('now')),  -- Gabriel -> 1º Ano B

-- 2º Ano A (Série 3): Sofia, Miguel
(5, 4, 3, datetime('now')),  -- Sofia -> 2º Ano A
(6, 7, 3, datetime('now')),  -- Miguel -> 2º Ano A

-- 3º Ano A (Série 4): Lucas
(7, 5, 4, datetime('now')),  -- Lucas -> 3º Ano A

-- 2º Ano B (Série 5): Julia
(8, 8, 5, datetime('now'));  -- Julia -> 2º Ano B

-- =====================================================
-- ATTENDANCES (Frequência)
-- Gerando registros para os últimos 15 dias úteis
-- =====================================================

-- Pedro (Aluno 1) - 1º Ano A (Série 1) - Excelente frequência
INSERT INTO Attendances (StudentId, GradeId, Date, Present, CreatedAt) VALUES 
(1, 1, date('now', '-1 day'), 1, datetime('now')),
(1, 1, date('now', '-2 day'), 1, datetime('now')),
(1, 1, date('now', '-3 day'), 1, datetime('now')),
(1, 1, date('now', '-4 day'), 1, datetime('now')),
(1, 1, date('now', '-7 day'), 1, datetime('now')),
(1, 1, date('now', '-8 day'), 1, datetime('now')),
(1, 1, date('now', '-9 day'), 1, datetime('now')),
(1, 1, date('now', '-10 day'), 1, datetime('now')),
(1, 1, date('now', '-11 day'), 1, datetime('now')),
(1, 1, date('now', '-14 day'), 1, datetime('now'));

-- Laura (Aluno 2) - 1º Ano A (Série 1) - Boa frequência com algumas faltas
INSERT INTO Attendances (StudentId, GradeId, Date, Present, CreatedAt) VALUES 
(2, 1, date('now', '-1 day'), 1, datetime('now')),
(2, 1, date('now', '-2 day'), 0, datetime('now')),  -- Falta
(2, 1, date('now', '-3 day'), 1, datetime('now')),
(2, 1, date('now', '-4 day'), 1, datetime('now')),
(2, 1, date('now', '-7 day'), 1, datetime('now')),
(2, 1, date('now', '-8 day'), 0, datetime('now')),  -- Falta
(2, 1, date('now', '-9 day'), 1, datetime('now')),
(2, 1, date('now', '-10 day'), 1, datetime('now')),
(2, 1, date('now', '-11 day'), 1, datetime('now')),
(2, 1, date('now', '-14 day'), 0, datetime('now')); -- Falta

-- Gabriel (Aluno 3) - 1º Ano B (Série 2) - Frequência média
INSERT INTO Attendances (StudentId, GradeId, Date, Present, CreatedAt) VALUES 
(3, 2, date('now', '-1 day'), 1, datetime('now')),
(3, 2, date('now', '-2 day'), 1, datetime('now')),
(3, 2, date('now', '-3 day'), 0, datetime('now')),  -- Falta
(3, 2, date('now', '-4 day'), 1, datetime('now')),
(3, 2, date('now', '-7 day'), 0, datetime('now')),  -- Falta
(3, 2, date('now', '-8 day'), 1, datetime('now')),
(3, 2, date('now', '-9 day'), 1, datetime('now')),
(3, 2, date('now', '-10 day'), 0, datetime('now')), -- Falta
(3, 2, date('now', '-11 day'), 1, datetime('now')),
(3, 2, date('now', '-14 day'), 1, datetime('now'));

-- Sofia (Aluno 4) - 2º Ano A (Série 3) - Excelente frequência
INSERT INTO Attendances (StudentId, GradeId, Date, Present, CreatedAt) VALUES 
(4, 3, date('now', '-1 day'), 1, datetime('now')),
(4, 3, date('now', '-2 day'), 1, datetime('now')),
(4, 3, date('now', '-3 day'), 1, datetime('now')),
(4, 3, date('now', '-4 day'), 1, datetime('now')),
(4, 3, date('now', '-7 day'), 1, datetime('now')),
(4, 3, date('now', '-8 day'), 1, datetime('now')),
(4, 3, date('now', '-9 day'), 0, datetime('now')),  -- Falta
(4, 3, date('now', '-10 day'), 1, datetime('now')),
(4, 3, date('now', '-11 day'), 1, datetime('now')),
(4, 3, date('now', '-14 day'), 1, datetime('now'));

-- Lucas (Aluno 5) - 3º Ano A (Série 4) - Frequência ruim
INSERT INTO Attendances (StudentId, GradeId, Date, Present, CreatedAt) VALUES 
(5, 4, date('now', '-1 day'), 0, datetime('now')),  -- Falta
(5, 4, date('now', '-2 day'), 1, datetime('now')),
(5, 4, date('now', '-3 day'), 0, datetime('now')),  -- Falta
(5, 4, date('now', '-4 day'), 1, datetime('now')),
(5, 4, date('now', '-7 day'), 0, datetime('now')),  -- Falta
(5, 4, date('now', '-8 day'), 0, datetime('now')),  -- Falta
(5, 4, date('now', '-9 day'), 1, datetime('now')),
(5, 4, date('now', '-10 day'), 0, datetime('now')), -- Falta
(5, 4, date('now', '-11 day'), 1, datetime('now')),
(5, 4, date('now', '-14 day'), 0, datetime('now')); -- Falta

-- Beatriz (Aluno 6) - 1º Ano A (Série 1) - Boa frequência
INSERT INTO Attendances (StudentId, GradeId, Date, Present, CreatedAt) VALUES 
(6, 1, date('now', '-1 day'), 1, datetime('now')),
(6, 1, date('now', '-2 day'), 1, datetime('now')),
(6, 1, date('now', '-3 day'), 1, datetime('now')),
(6, 1, date('now', '-4 day'), 0, datetime('now')),  -- Falta
(6, 1, date('now', '-7 day'), 1, datetime('now')),
(6, 1, date('now', '-8 day'), 1, datetime('now')),
(6, 1, date('now', '-9 day'), 1, datetime('now')),
(6, 1, date('now', '-10 day'), 1, datetime('now')),
(6, 1, date('now', '-11 day'), 0, datetime('now')), -- Falta
(6, 1, date('now', '-14 day'), 1, datetime('now'));

-- Miguel (Aluno 7) - 2º Ano A (Série 3) - Frequência média
INSERT INTO Attendances (StudentId, GradeId, Date, Present, CreatedAt) VALUES 
(7, 3, date('now', '-1 day'), 1, datetime('now')),
(7, 3, date('now', '-2 day'), 0, datetime('now')),  -- Falta
(7, 3, date('now', '-3 day'), 1, datetime('now')),
(7, 3, date('now', '-4 day'), 1, datetime('now')),
(7, 3, date('now', '-7 day'), 0, datetime('now')),  -- Falta
(7, 3, date('now', '-8 day'), 1, datetime('now')),
(7, 3, date('now', '-9 day'), 0, datetime('now')),  -- Falta
(7, 3, date('now', '-10 day'), 1, datetime('now')),
(7, 3, date('now', '-11 day'), 1, datetime('now')),
(7, 3, date('now', '-14 day'), 1, datetime('now'));

-- Julia (Aluno 8) - 2º Ano B (Série 5) - Excelente frequência
INSERT INTO Attendances (StudentId, GradeId, Date, Present, CreatedAt) VALUES 
(8, 5, date('now', '-1 day'), 1, datetime('now')),
(8, 5, date('now', '-2 day'), 1, datetime('now')),
(8, 5, date('now', '-3 day'), 1, datetime('now')),
(8, 5, date('now', '-4 day'), 1, datetime('now')),
(8, 5, date('now', '-7 day'), 1, datetime('now')),
(8, 5, date('now', '-8 day'), 1, datetime('now')),
(8, 5, date('now', '-9 day'), 1, datetime('now')),
(8, 5, date('now', '-10 day'), 1, datetime('now')),
(8, 5, date('now', '-11 day'), 1, datetime('now')),
(8, 5, date('now', '-14 day'), 0, datetime('now')); -- Falta

-- =====================================================
-- ACTIVITIES (Atividades)
-- =====================================================
INSERT INTO Activities (Id, Title, Description, GradeId, ApplicationDate, MaxScore, Status, CreatedAt) VALUES 
-- Atividades para 1º Ano A (Série 1)
(1, 'Prova de Matemática - 1º Bimestre', 'Avaliação sobre números e operações básicas', 1, date('now', '-10 day'), 10, 1, datetime('now')),
(2, 'Trabalho de Português - Leitura', 'Interpretação de texto e vocabulário', 1, date('now', '-7 day'), 10, 1, datetime('now')),
(3, 'Exercícios de Ciências', 'Atividade sobre o corpo humano', 1, date('now', '-5 day'), 5, 1, datetime('now')),

-- Atividades para 1º Ano B (Série 2)
(4, 'Prova de Matemática - 1º Bimestre', 'Avaliação sobre números e operações básicas', 2, date('now', '-10 day'), 10, 1, datetime('now')),
(5, 'Redação - Minha Família', 'Produção de texto sobre a família', 2, date('now', '-6 day'), 10, 1, datetime('now')),

-- Atividades para 2º Ano A (Série 3)
(6, 'Prova de Matemática - 1º Bimestre', 'Avaliação sobre multiplicação e divisão', 3, date('now', '-12 day'), 10, 1, datetime('now')),
(7, 'Trabalho de História', 'Pesquisa sobre a cidade', 3, date('now', '-8 day'), 10, 1, datetime('now')),
(8, 'Prova de Geografia', 'Avaliação sobre relevo brasileiro', 3, date('now', '-4 day'), 10, 1, datetime('now')),

-- Atividades para 3º Ano A (Série 4)
(9, 'Prova de Matemática - 1º Bimestre', 'Avaliação sobre frações', 4, date('now', '-14 day'), 10, 1, datetime('now')),
(10, 'Trabalho de Inglês', 'Apresentação oral em inglês', 4, date('now', '-7 day'), 10, 1, datetime('now')),

-- Atividades para 2º Ano B (Série 5)
(11, 'Prova de Matemática - 1º Bimestre', 'Avaliação sobre multiplicação', 5, date('now', '-11 day'), 10, 1, datetime('now')),
(12, 'Prova de Português', 'Avaliação de gramática', 5, date('now', '-5 day'), 10, 1, datetime('now'));

-- =====================================================
-- STUDENT_ACTIVITIES (Notas dos Alunos)
-- =====================================================
INSERT INTO StudentActivities (Id, StudentId, ActivityId, Score, Remarks, CreatedAt) VALUES 
-- 1º Ano A (Série 1): Pedro (1), Laura (2), Beatriz (6)
-- Prova de Matemática (Atividade 1)
(1, 1, 1, 9.5, 'Excelente desempenho!', datetime('now')),
(2, 2, 1, 7.0, 'Pode melhorar nas operações', datetime('now')),
(3, 6, 1, 8.5, 'Bom trabalho', datetime('now')),
-- Trabalho de Português (Atividade 2)
(4, 1, 2, 8.0, NULL, datetime('now')),
(5, 2, 2, 9.0, 'Ótima interpretação', datetime('now')),
(6, 6, 2, 7.5, NULL, datetime('now')),
-- Exercícios de Ciências (Atividade 3)
(7, 1, 3, 5.0, 'Nota máxima!', datetime('now')),
(8, 2, 3, 4.0, NULL, datetime('now')),
(9, 6, 3, 4.5, NULL, datetime('now')),

-- 1º Ano B (Série 2): Gabriel (3)
-- Prova de Matemática (Atividade 4)
(10, 3, 4, 6.5, 'Precisa estudar mais', datetime('now')),
-- Redação (Atividade 5)
(11, 3, 5, 8.0, 'Criativo!', datetime('now')),

-- 2º Ano A (Série 3): Sofia (4), Miguel (7)
-- Prova de Matemática (Atividade 6)
(12, 4, 6, 10.0, 'Perfeito!', datetime('now')),
(13, 7, 6, 7.5, NULL, datetime('now')),
-- Trabalho de História (Atividade 7)
(14, 4, 7, 9.0, 'Pesquisa muito completa', datetime('now')),
(15, 7, 7, 6.0, 'Faltou profundidade', datetime('now')),
-- Prova de Geografia (Atividade 8)
(16, 4, 8, 8.5, NULL, datetime('now')),
(17, 7, 8, 7.0, NULL, datetime('now')),

-- 3º Ano A (Série 4): Lucas (5)
-- Prova de Matemática (Atividade 9)
(18, 5, 9, 5.0, 'Precisa de reforço em frações', datetime('now')),
-- Trabalho de Inglês (Atividade 10)
(19, 5, 10, 7.5, 'Boa pronúncia', datetime('now')),

-- 2º Ano B (Série 5): Julia (8)
-- Prova de Matemática (Atividade 11)
(20, 8, 11, 9.0, 'Muito bem!', datetime('now')),
-- Prova de Português (Atividade 12)
(21, 8, 12, 8.5, NULL, datetime('now'));

-- =====================================================
-- RESUMO DOS DADOS INSERIDOS
-- =====================================================
-- 
-- USUÁRIOS (6):
--   - 1 Coordenador: coordenador@escola.com
--   - 2 Professores: professor1@escola.com, professor2@escola.com
--   - 3 Responsáveis: responsavel1@email.com, responsavel2@email.com, responsavel3@email.com
--   - Senha para todos: Senha@123
--
-- GRIDS (2):
--   - Grade Curricular 2025
--   - Grade Curricular 2026
--
-- SÉRIES (5):
--   - 1º Ano A (Manhã) - Grid 2025
--   - 1º Ano B (Tarde) - Grid 2025
--   - 2º Ano A (Manhã) - Grid 2025
--   - 3º Ano A (Noite) - Grid 2026
--   - 2º Ano B (Tarde) - Grid 2025
--
-- MATÉRIAS (7):
--   - Matemática, Português, Ciências, História, Geografia, Inglês, Artes
--
-- ALUNOS (8):
--   - Pedro Oliveira (2025001) - 1º Ano A - Resp: Carlos
--   - Laura Oliveira (2025002) - 1º Ano A - Resp: Carlos
--   - Gabriel Lima (2025003) - 1º Ano B - Resp: Fernanda
--   - Sofia Almeida (2025004) - 2º Ano A - Resp: Roberto
--   - Lucas Almeida (2025005) - 3º Ano A - Resp: Roberto
--   - Beatriz Santos (2025006) - 1º Ano A - Resp: Fernanda
--   - Miguel Costa (2025007) - 2º Ano A - Resp: Carlos
--   - Julia Ferreira (2025008) - 2º Ano B - Resp: Fernanda
--
-- FREQUÊNCIA: 80 registros (10 por aluno, últimos 15 dias)
--
-- ATIVIDADES (12):
--   - Diversas provas e trabalhos por série
--
-- NOTAS (21):
--   - Notas variadas para cada aluno em suas atividades
-- =====================================================

SELECT 'Seed concluído com sucesso!' AS Resultado;
SELECT 'Usuários: ' || COUNT(*) FROM Users;
SELECT 'Grids: ' || COUNT(*) FROM Grids;
SELECT 'Séries: ' || COUNT(*) FROM Grades;
SELECT 'Matérias: ' || COUNT(*) FROM Classes;
SELECT 'Série x Matéria: ' || COUNT(*) FROM GradeClasses;
SELECT 'Matéria x Professor: ' || COUNT(*) FROM ClassTeachers;
SELECT 'Alunos: ' || COUNT(*) FROM Students;
SELECT 'Aluno x Série: ' || COUNT(*) FROM StudentGrades;
SELECT 'Frequências: ' || COUNT(*) FROM Attendances;
SELECT 'Atividades: ' || COUNT(*) FROM Activities;
SELECT 'Notas: ' || COUNT(*) FROM StudentActivities;
