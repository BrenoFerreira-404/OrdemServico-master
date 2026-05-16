# 🔐 Credenciais de Acesso - Super Admin

## ⚠️ CONFIDENCIAL - Uso Apenas em Desenvolvimento

**Data de Criação:** 16/05/2026  
**Ambiente:** Docker (Desenvolvimento)  
**Status:** ✅ Login Funcionando

---

## 🔑 Super Admin - Acesso 100%

| Campo | Valor |
|-------|-------|
| **Email** | `admin@ordemservico.com` |
| **Senha** | `Admin@123456` |
| **Role** | SuperAdmin |
| **Status** | Ativo |
| **Email Confirmado** | Sim |

---

## 🌐 URLs de Acesso

| Serviço | URL | Propósito |
|---------|-----|----------|
| Web (Blazor) | http://localhost:8082 | Interface principal do sistema |
| API | http://localhost:8080 | API REST .NET 9 |
| phpMyAdmin | http://localhost:8081 | Gerenciamento do banco de dados |

---

## 📊 Banco de Dados

| Item | Valor |
|------|-------|
| **Host** | localhost:3306 |
| **Banco** | os_db |
| **Usuário** | root |
| **Senha** | root |

---

## 🗄️ Dados Técnicos (BD)

**Tabela:** `AspNetUsers`

```sql
SELECT * FROM AspNetUsers WHERE Email = 'admin@ordemservico.com';
```

**Tabela Domain:** `usuarios`
**Cargo:** SuperAdmin (0 em enum)

---

## 🔄 Como Acessar

### Via Web (Recomendado)
1. Acesse: http://localhost:8082
2. Faça login com:
   - Email: `admin@ordemservico.com`
   - Senha: `Admin@123456`
3. Você terá acesso completo ao sistema com todas as funcionalidades

### Via API (cURL / Postman)
```json
POST http://localhost:8080/api/auth/login

{
  "email": "admin@ordemservico.com",
  "senha": "Admin@123456"
}
```

**Resposta:**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "...",
  "expiraEm": "2026-05-16T22:43:18Z",
  "usuario": {...}
}
```

### Via phpMyAdmin (Gerenciamento de BD)
1. Acesse: http://localhost:8081
2. Faça login com:
   - Usuário: `root`
   - Senha: `root`
3. Selecione o banco `os_db`
4. Navegue até a tabela `AspNetUsers` para visualizar todos os usuários

---

## 📝 Notas Importantes

- ✅ As credenciais foram criadas via **ASP.NET Identity UserManager**
- ✅ A senha está corretamente **hashada** no banco de dados
- ✅ O usuário tem a role **SuperAdmin** com todas as permissões
- ✅ Todos os dados de teste foram seedados automaticamente
- ⚠️ Nunca compartilhe estas credenciais em produção
- ⚠️ Em produção, use um gerenciador de secrets (Azure Key Vault, AWS Secrets Manager, etc)

---

## 🛠️ Dados Incluídos no Seed

Quando o Docker inicia, o banco é populado com:

- **Clientes:** 6 clientes de teste
- **Equipamentos:** 10 equipamentos variados  
- **Ordens de Serviço:** Múltiplas OS em diferentes estados
- **Produtos:** Itens para OS
- **Serviços:** Serviços para OS
- **Roles:** SuperAdmin, Admin, Gerente, Técnico, Atendente

---

**Última Atualização:** 16/05/2026 ✅ Login Confirmado
