CREATE DATABASE SistepedDB;
GO

USE SistepedDB;
GO

CREATE TABLE Escola (
    idEscola INT PRIMARY KEY IDENTITY(1,1),
    instituicaoEscolar NVARCHAR(255) NOT NULL,
    entidadeMantenedora NVARCHAR(255),
    numeroReconhecimento NVARCHAR(100)
);
GO

CREATE TABLE Professor (
    idProfessor INT PRIMARY KEY IDENTITY(1,1),
    nome NVARCHAR(255) NOT NULL,
    email NVARCHAR(255),
    senha NVARCHAR(255) NOT NULL
);
GO

CREATE TABLE Disciplina (
    idDisciplina INT PRIMARY KEY IDENTITY(1,1),
    nome NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE Turma (
    idTurma INT PRIMARY KEY IDENTITY(1,1),
    nome NVARCHAR(100) NOT NULL,
    anoLetivo NVARCHAR(20),
    idEscola INT NOT NULL,
    CONSTRAINT FK_Turma_Escola FOREIGN KEY (idEscola) REFERENCES Escola(idEscola)
);
GO

CREATE TABLE Aluno (
    idAluno INT PRIMARY KEY IDENTITY(1,1),
    nomeCompleto NVARCHAR(255) NOT NULL,
    dataNascimento DATE,
    filiacao NVARCHAR(255),
    naturalidade NVARCHAR(100),
    nacionalidade NVARCHAR(100),
    identidade NVARCHAR(50),
    cpf NVARCHAR(14) UNIQUE,
    idTurma INT NOT NULL,
    CONSTRAINT FK_Aluno_Turma FOREIGN KEY (idTurma) REFERENCES Turma(idTurma)
);
GO

CREATE TABLE Endereco (
    idEndereco INT PRIMARY KEY IDENTITY(1,1),
    rua NVARCHAR(255),
    numero NVARCHAR(20),
    cidade NVARCHAR(100),
    bairro NVARCHAR(100),
    idAluno INT NOT NULL,
    CONSTRAINT FK_Endereco_Aluno FOREIGN KEY (idAluno) REFERENCES Aluno(idAluno)
);
GO

CREATE TABLE Contato (
    idContato INT PRIMARY KEY IDENTITY(1,1),
    telefone NVARCHAR(20),
    email NVARCHAR(255),
    idAluno INT NOT NULL,
    CONSTRAINT FK_Contato_Aluno FOREIGN KEY (idAluno) REFERENCES Aluno(idAluno)
);
GO

CREATE TABLE Avaliacao (
    idAvaliacao INT PRIMARY KEY IDENTITY(1,1),
    conteudo NVARCHAR(255),
    nota DECIMAL(5, 2),
    data DATE,
    tipo NVARCHAR(50),
    idAluno INT NOT NULL,
    CONSTRAINT FK_Avaliacao_Aluno FOREIGN KEY (idAluno) REFERENCES Aluno(idAluno)
);
GO

CREATE TABLE Comportamento (
    idComportamento INT PRIMARY KEY IDENTITY(1,1),
    tag NVARCHAR(50),
    data DATE,
    observacao NVARCHAR(MAX),
    idAluno INT NOT NULL,
    CONSTRAINT FK_Comportamento_Aluno FOREIGN KEY (idAluno) REFERENCES Aluno(idAluno)
);
GO

CREATE TABLE ProfessorTurma (
    idProfessor INT NOT NULL,
    idTurma INT NOT NULL,
    PRIMARY KEY (idProfessor, idTurma),
    CONSTRAINT FK_ProfessorTurma_Professor FOREIGN KEY (idProfessor) REFERENCES Professor(idProfessor),
    CONSTRAINT FK_ProfessorTurma_Turma FOREIGN KEY (idTurma) REFERENCES Turma(idTurma)
);
GO

CREATE TABLE ProfessorDisciplina (
    idProfessor INT NOT NULL,
    idDisciplina INT NOT NULL,
    PRIMARY KEY (idProfessor, idDisciplina),
    CONSTRAINT FK_ProfessorDisciplina_Professor FOREIGN KEY (idProfessor) REFERENCES Professor(idProfessor),
    CONSTRAINT FK_ProfessorDisciplina_Disciplina FOREIGN KEY (idDisciplina) REFERENCES Disciplina(idDisciplina)
);
GO