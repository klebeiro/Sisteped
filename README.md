# EduSystem - Sistema de Gestão Educacional

Sistema completo de gestão educacional desenvolvido com arquitetura moderna, separando frontend e backend, com suporte a múltiplos perfis de usuário (Coordenadores, Professores e Responsáveis).

## Como executar

docker compose pull
docker compose up -d

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Tecnologias](#tecnologias)
- [Arquitetura](#arquitetura)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Funcionalidades](#funcionalidades)
---

## 🎯 Sobre o Projeto

O **EduSystem** é uma plataforma web desenvolvida para gerenciar processos educacionais em instituições de ensino. O sistema oferece funcionalidades completas para:

- Gestão de alunos, professores e responsáveis
- Controle de atividades e avaliações
- Registro de notas e frequência
- Gerenciamento de grades curriculares e disciplinas
- Geração de relatórios acadêmicos
- Calendário escolar

O projeto foi desenvolvido seguindo boas práticas de desenvolvimento, com separação clara de responsabilidades, código limpo e arquitetura escalável.

---

## 🛠 Tecnologias

### Frontend

- **React 19** - Biblioteca JavaScript para construção de interfaces
- **TypeScript** - Superset do JavaScript com tipagem estática
- **Vite** - Build tool e dev server de alta performance
- **React Router DOM** - Roteamento para aplicações React
- **Zustand** - Gerenciamento de estado global leve e simples
- **TanStack Query (React Query)** - Gerenciamento de estado do servidor e cache
- **Axios** - Cliente HTTP para requisições à API
- **React Hook Form** - Biblioteca para formulários performáticos
- **TailwindCSS** - Framework CSS utility-first

### Backend

- **.NET 8** - Framework de desenvolvimento web da Microsoft
- **Entity Framework Core** - ORM para acesso a dados
- **SQLite** - Banco de dados relacional embutido
- **JWT (JSON Web Tokens)** - Autenticação e autorização
- **FluentValidation** - Validação de dados robusta
- **AutoMapper** - Mapeamento de objetos
- **BCrypt** - Hash de senhas seguro
- **Swagger/OpenAPI** - Documentação interativa da API

### DevOps

- **Docker** - Conteinerização de aplicações
- **Docker Compose** - Orquestração de containers
- **Nginx** - Servidor web e proxy reverso

---

## 🏗 Arquitetura

O projeto segue uma arquitetura em camadas, separando claramente as responsabilidades:

### Frontend

```
edusystem/
├── src/
│   ├── components/     # Componentes reutilizáveis
│   ├── pages/          # Páginas da aplicação
│   ├── layouts/        # Layouts de páginas
│   ├── services/       # Serviços de comunicação com API
│   ├── contracts/      # Contratos/Tipos TypeScript (DTOs)
│   ├── store/          # Estado global (Zustand)
│   ├── routes/         # Configuração de rotas
│   ├── utils/          # Utilitários e helpers
│   └── queries/        # Queries do React Query
```

### Backend

```
edusystem-api/sisteped-api/
├── Controllers/        # Endpoints da API (camada de apresentação)
├── Services/           # Lógica de negócio
├── Repositories/       # Acesso a dados
├── DTOs/              # Data Transfer Objects (Request/Response)
├── Models/            # Entidades do domínio
├── Validators/        # Validações com FluentValidation
├── Infra/             # Infraestrutura (DbContext, configurações)
├── Helpers/           # Classes auxiliares
└── IoC/               # Injeção de dependências
```

### Comunicação

- O frontend se comunica com o backend através de requisições HTTP REST
- O Nginx atua como proxy reverso, redirecionando requisições `/api/*` para o backend
- Autenticação baseada em JWT tokens armazenados no localStorage

---

## 📁 Estrutura do Projeto

```
wes-pi/
├── edusystem/                 # Frontend React
│   ├── src/
│   ├── public/
│   ├── Dockerfile
│   ├── nginx.conf
│   └── package.json
│
├── edusystem-api/             # Backend .NET
│   └── sisteped-api/
│       ├── Controllers/
│       ├── Services/
│       ├── Repositories/
│       ├── DTOs/
│       ├── Models/
│       ├── Validators/
│       ├── Infra/
│       ├── Dockerfile
│       └── sisteped-api.csproj
│
├── docker-compose.yml         # Orquestração dos serviços
├── DOCKER.md                  # Documentação Docker
└── README.md                  # Este arquivo
```

---

## ✨ Funcionalidades

### Autenticação e Autorização

- **Login de Professores**: Acesso ao sistema com credenciais de professor
- **Login de Responsáveis**: Acesso para visualizar informações dos dependentes
- **Registro de Usuários**: Cadastro de novos usuários no sistema
- **Autenticação JWT**: Tokens seguros para autenticação de requisições
- **Controle de Acesso**: Diferentes permissões por perfil (Coordenador, Professor, Responsável)

### Gestão de Usuários

- CRUD completo de usuários
- Diferentes perfis: Coordenador, Professor, Responsável
- Gerenciamento de credenciais e senhas

### Gestão de Alunos

- Cadastro, edição e listagem de alunos
- Vinculação de alunos a responsáveis
- Visualização de informações acadêmicas

### Gestão de Professores

- Cadastro e gerenciamento de professores
- Vinculação de professores a disciplinas e turmas

### Gestão de Disciplinas (Matérias)

- CRUD de disciplinas/matérias
- Organização por séries e grades curriculares

### Gestão de Atividades

- Criação e gerenciamento de atividades acadêmicas
- Vinculação de atividades a disciplinas
- Controle de prazos e datas

### Gestão de Notas

- Registro de notas por atividade
- Cálculo de médias
- Histórico de avaliações

### Gestão de Frequência

- Registro de presença/ausência dos alunos
- Relatórios de frequência

### Gestão de Grades Curriculares

- Criação e configuração de grades curriculares
- Vinculação de disciplinas a séries
- Organização do currículo escolar

### Relatórios

- Relatórios de notas por aluno
- Relatórios de frequência
- Exportação de dados (CSV)

**Desenvolvido com ❤️ para facilitar a gestão educacional**