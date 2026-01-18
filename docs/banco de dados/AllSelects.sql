-- Usuario e Credenciais
SELECT 
    U.Id,
    U.Name,
    U.Email,
    C.Role,
    C.PasswordHash,
    U.CreatedAt
FROM Users U
INNER JOIN UserCredentials C ON U.Id = C.UserId;

-- Escola
SELECT 
    idEscola,
    instituicaoEscolar,
    entidadeMantenedora,
    numeroReconhecimento
FROM Escola;

-- Professor
SELECT 
    idProfessor,
    nome,
    email,
    senha
FROM Professor;

-- Disciplina
SELECT 
    idDisciplina,
    nome
FROM Disciplina;

-- Turma
SELECT 
    idTurma,
    nome,
    anoLetivo,
    idEscola
FROM Turma;

-- Aluno
SELECT 
    idAluno,
    nomeCompleto,
    dataNascimento,
    filiacao,
    naturalidade,
    nacionalidade,
    identidade,
    cpf,
    idTurma
FROM Aluno;

-- Endere�o
SELECT 
    idEndereco,
    rua,
    numero,
    cidade,
    bairro,
    idAluno
FROM Endereco;

-- Contato
SELECT 
    idContato,
    telefone,
    email,
    idAluno
FROM Contato;

-- Avalia��o
SELECT 
    idAvaliacao,
    conteudo,
    nota,
    data,
    tipo,
    idAluno
FROM Avaliacao;

-- Comportamento
SELECT 
    idComportamento,
    tag,
    data,
    observacao,
    idAluno
FROM Comportamento;

-- ProfessorTurma
SELECT 
    idProfessor,
    idTurma
FROM ProfessorTurma;

-- ProfessorDisciplina
SELECT 
    idProfessor,
    idDisciplina
FROM ProfessorDisciplina;