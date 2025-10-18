# Sisteped - Sistema de Gestão Pedagógica em Kivy

Sistema de gestão pedagógica desenvolvido em Python usando Kivy, com banco de dados SQLite local. O projeto permite gerenciamento de usuários, alunos, disciplinas e notas.

## Estrutura do Projeto

```
sisteped/
│
├─[1] main.py
├─ database/
│ ├─[2] db_manager.py 
│ └─[3] sisteped.db
├─ kv/
│ └─[4] main.kv
└─[5] assets/ 
```

1. Arquivo principal do app
1. Criação e manipulação do banco SQLite
1. Banco de dados (gerado automaticamente)
1. Layout declarativo Kivy
1. Ícones, imagens, recursos gráficos

## Configuração do Ambiente (venv)

### 1. Criar o ambiente virtual
No terminal do VS Code, na pasta raiz do projeto, execute: `python -m venv venv`

### 2. Ativar o ambiente virtual
Execute:
- Linux: `source venv/bin/activate`
- Windows: `.\venv\Scripts\activate`

### 3. Dependencias de projeto
- Para instalar as dependencias: `pip install -r requirements.txt` 
- Para atualizar as dependencias: `pip freeze > requirements.txt`