# 💰 Controle-Financeiro (Front + Back)

O projeto visa fornecer uma ferramenta para gerenciar finanças pessoais ou de pequenas empresas, permitindo o controle de receitas e despesas. Atualmente, o sistema possui a estrutura básica para gerenciamento de entidades como Cidades, Estados, Pessoas, Contas e Usuários, com foco na arquitetura e segurança da aplicação.

---

## 📁 Estrutura do Projeto

| Pasta           | Descrição                                                                 |
|-----------------|---------------------------------------------------------------------------|
| `FrontConFin`   | Aplicação cliente em Windows Forms com autenticação e interface gráfica  |
| `WFConFin`      | Back-end em ASP.NET Core com Entity Framework e autenticação JWT         |

---

## 🚀 Status Atual

- ✅ Autenticação com JWT
- ✅ CRUD de usuários
- ✅ Integração front-end ↔ back-end
- ⚠️ Funcionalidades pendentes:
  - Cadastro de pessoas
  - Lançamento e gerenciamento de contas
  - Configurações do usuário

---

## 🧪 Tecnologias Utilizadas

### 🔧 Back-end
- ASP.NET Core (Web API)
- Entity Framework Core + PostgreSQL
- Autenticação via JWT
- Hash de senha com MD5

### 🎨 Front-end
- Windows Forms (.NET)
- Comunicação via `HttpClient`
- Serialização JSON com `Newtonsoft.Json`

---

## 🔜 Funcionalidades Futuras (v2)

- 🧑 Cadastro/edição de **Pessoas**
- 💸 Tela de **lançamento de contas**
- ⚙️ Módulo de **Configurações do usuário**
- 🔒 Melhorias de segurança (ex: hash com salt)
- 📦 Geração de instalador + CI/CD
