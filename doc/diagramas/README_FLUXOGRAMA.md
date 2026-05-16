# 🔐 Fluxograma: Login até Dashboard

## 📍 Localização
```
📁 doc/diagramas/fluxo_login_dashboard.drawio
```

---

## 🎯 O que é este diagrama?

Fluxograma completo da arquitetura de **autenticação e autorização** da aplicação OrdemServico, mostrando o caminho dos dados desde o momento que o usuário preenche as credenciais até chegar no dashboard.

---

## 🏗️ Arquitetura em Camadas

O diagrama é organizado em **4 camadas principais**:

### 1️⃣ **WEB Layer** (Blazor Client-side)
- **Cor:** Azul claro
- **Componentes:**
  - `LoginPage.razor` - Página de login com formulário
  - `LoginViewModel` - Lógica de apresentação
  - `TokenStorage` - Armazenamento do JWT
  - `AuthStateProvider` - Gerenciamento de estado de autenticação

### 2️⃣ **API Layer** (.NET 9 Minimal APIs)
- **Cor:** Roxo
- **Componentes:**
  - `AuthEndpoints` - Endpoint `/api/auth/login`
  - `ValidationFilter<T>` - Validação de DTO
  - `AuthService` - Orquestração da autenticação
  - `JwtTokenService` - Geração de tokens

### 3️⃣ **Infrastructure Layer** (Identity & Services)
- **Cor:** Laranja
- **Componentes:**
  - `IdentityService` - Validação de credenciais
  - `UserManager<AppIdentityUser>` - Gerenciamento de usuários ASP.NET
  - Integração com `IdentityDb`

### 4️⃣ **Database Layer** (MySQL)
- **Cor:** Verde
- **Tabelas:**
  - `AspNetUsers` - Usuários (com PasswordHash)
  - `refresh_tokens` - Tokens de refresh persistidos
  - `usuarios` - Dados de domínio do usuário

---

## 📊 Fluxo Passo a Passo

### **Fase 1: Entrada do Usuário (WEB)**
```
1. LoginPage.razor
   ↓ Renderiza formulário de login
2. Usuário Preenche Email + Senha
   ↓ Dados no formulário (LoginFormModel)
3. LoginViewModel.HandleLogin()
   ↓ Prepara chamada HTTP
```

### **Fase 2: Transmissão (WEB → API)**
```
4. POST /api/auth/login
   ↓ HTTP Request com DTO {email, senha}
```

### **Fase 3: Processamento (API)**
```
5. ValidationFilter<LoginRequest>
   ↓ Valida DTO via FluentValidation
6. AuthService.LoginAsync()
   ↓ Orquestra o processo de login
7. IdentityService.ValidarCredenciaisAsync()
   ↓ Prepara validação de senha
```

### **Fase 4: Busca de Dados (Infrastructure → DB)**
```
8. UserManager.FindByEmailAsync(email)
   ↓ Query no AspNetUsers
   Database: SELECT * FROM AspNetUsers WHERE Email = @email
   
9. UserManager.CheckPasswordAsync(user, senha)
   ↓ Verifica hash da senha (ASP.NET Identity)
   Compara: BCrypt.Verify(senhaPlana, passwordHash)
```

### **Fase 5: Geração de Tokens (API)**
```
10. JwtTokenService.GerarTokensAsync()
    ↓ Cria:
    - accessToken (JWT) - Curta duração
    - refreshToken - Armazenado no BD
    
11. Database: INSERT INTO refresh_tokens
    ↓ Salva refresh token para uso posterior
```

### **Fase 6: Resposta para Cliente (API → WEB)**
```
12. Return LoginResponse
    {
      "accessToken": "eyJhbGci...",
      "refreshToken": "...",
      "expiraEm": "2026-05-16T22:43:18Z",
      "usuario": {...}
    }
```

### **Fase 7: Armazenamento Local (WEB)**
```
13. TokenStorage.SetTokens()
    ↓ Salva no localStorage/sessionStorage:
    - AccessToken
    - RefreshToken
    - Usuário Info
    
14. AuthStateProvider.MarkUserAsAuthenticated()
    ↓ Atualiza estado de autenticação
    ↓ Notifica componentes Blazor
```

### **Fase 8: Redirecionamento (WEB)**
```
15. NavigationManager.NavigateTo("/")
    ↓ Redireciona para Home/Dashboard
    
    Home.razor carrega com:
    ✓ Usuário autenticado
    ✓ Token disponível em requisições futuras
    ✓ Menu lateral + Conteúdo autorizado
```

---

## 🔑 Pontos-Chave da Implementação

### **Segurança**
- ✅ Senhas nunca transitam em texto plano
- ✅ Hash com ASP.NET Identity (PBKDF2 + salt)
- ✅ JWT tokens com assinatura HS256
- ✅ Refresh token persistido no BD

### **Fluxo de Dados**
```
Email + Senha (Client)
    ↓ HTTP POST
    ↓ Validação DTO
    ↓ Hash comparação (BD)
    ↓ JWT geração
    ↓ HTTP Response
    ↓ Token storage (Client)
    ↓ AuthState atualizado
    ↓ Dashboard carregado
```

### **Persistência de Tokens**
```
Para cada login bem-sucedido:
1. AccessToken: JWT (expiração curta, ~30 min)
2. RefreshToken: Armazenado em BD + localStorage
   → Permite renovar AccessToken sem re-login
```

### **Estado de Autenticação**
```
AuthStateProvider herda de AuthenticationStateProvider
↓ Implementa GetAuthenticationStateAsync()
↓ Verifica token no localStorage
↓ Comunica para Authorize/AuthorizeView components
↓ Renderização condicional baseada em roles
```

---

## 🚀 Como Usar Este Diagrama

### **Abrir no VS Code com Extensão Draw.io**

1. **Instale a extensão:**
   - Abra VS Code → Extensions
   - Busque por "Draw.io Integration"
   - Instale (publicado por hediet)

2. **Abra o arquivo:**
   ```bash
   # Abra o arquivo no VS Code
   code doc/diagramas/fluxo_login_dashboard.drawio
   ```

3. **Visualize o diagrama:**
   - O VS Code abrirá com visualização interativa
   - Clique em elementos para editá-los
   - Use zoom (Ctrl + Scroll) para ampliar

### **Abrir Online**

1. Acesse https://app.diagrams.net
2. File → Open → Selecione `fluxo_login_dashboard.drawio`
3. Edite conforme necessário
4. File → Save (sobrescreve o arquivo)

### **Exportar**

```bash
# Para PNG/SVG no Draw.io Desktop:
# File → Export As → Escolha formato
```

---

## 📝 Componentes Mencionados no Diagrama

### **Web (Blazor)**
- `LoginPage.razor` - [src/Web/Components/Pages/Auth/LoginPage.razor](../../src/Web/Components/Pages/Auth/LoginPage.razor)
- `LoginViewModel` - [src/Web/ViewModels/Auth/LoginViewModel.cs](../../src/Web/ViewModels/Auth/LoginViewModel.cs)
- `TokenStorage` - [src/Web/Services/Auth/TokenStorage.cs](../../src/Web/Services/Auth/TokenStorage.cs)
- `AuthStateProvider` - [src/Web/Services/Auth/AuthStateProvider.cs](../../src/Web/Services/Auth/AuthStateProvider.cs)
- `Home.razor` - [src/Web/Components/Pages/Home.razor](../../src/Web/Components/Pages/Home.razor)

### **API**
- `AuthEndpoints` - [src/Api/Endpoints/AuthEndpoints.cs](../../src/Api/Endpoints/AuthEndpoints.cs)
- `AuthService` - [src/Application/Services/AuthService.cs](../../src/Application/Services/AuthService.cs)
- `ValidationFilter` - [src/Api/Filters/ValidationFilter.cs](../../src/Api/Filters/ValidationFilter.cs)

### **Infrastructure**
- `IdentityService` - [src/Infrastructure/Identity/IdentityService.cs](../../src/Infrastructure/Identity/IdentityService.cs)
- `JwtTokenService` - [src/Infrastructure/Identity/JwtTokenService.cs](../../src/Infrastructure/Identity/JwtTokenService.cs)
- `AppIdentityUser` - [src/Infrastructure/Identity/AppIdentityUser.cs](../../src/Infrastructure/Identity/AppIdentityUser.cs)

---

## 🔄 Fluxo de Refresh Token (Não mostrado no diagrama principal)

Quando o AccessToken expira:

```
1. Client: Detecta erro 401 Unauthorized
2. Client: POST /api/auth/refresh com refreshToken
3. API: Valida refreshToken no BD
4. API: Se válido, gera novo accessToken
5. Client: Armazena novo token
6. Client: Retenta requisição original
```

---

## ✅ Testes do Fluxo

Para validar este fluxo no seu ambiente:

```bash
# 1. Login
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@ordemservico.com","senha":"Admin@123456"}'

# Response:
# {
#   "accessToken": "eyJhbGciOiJIUzI1NiIs...",
#   "refreshToken": "...",
#   "expiraEm": "2026-05-16T22:43:18Z"
# }

# 2. Acessar endpoint protegido
curl -X GET http://localhost:8080/api/clientes \
  -H "Authorization: Bearer YOUR_ACCESS_TOKEN"
```

---

## 📚 Relacionado

- 📖 [Ordem de Serviço - Regras de Negócio](./ordem_servico_regras_negocio.md)
- 🏗️ [Arquitetura da API](./arquitetura_api.md)
- 🎯 [Blazor MVVM - Fluxo de Cadastro](./blazor_mvvm_fluxo_cadastro_envio.md)

---

**Última Atualização:** 16/05/2026  
**Arquivo:** `doc/diagramas/fluxo_login_dashboard.drawio`
