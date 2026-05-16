# 🎉 OrdemServico - Setup Completo

## ✅ Status: Tudo Funcionando!

**Data:** 16/05/2026  
**Ambiente:** Docker  
**Login:** ✅ Testado e Verificado

---

## 🔐 CREDENCIAIS DE ACESSO

```
📧 Email:    admin@ordemservico.com
🔑 Senha:    Admin@123456
👤 Perfil:   SuperAdmin (100% de acesso)
```

---

## 🌐 ACESSOS RÁPIDOS

| Serviço | URL | Tipo |
|---------|-----|------|
| **Sistema Web** | http://localhost:8082 | Blazor MVVM |
| **API REST** | http://localhost:8080 | .NET 9 Minimal APIs |
| **Banco de Dados** | http://localhost:8081 | phpMyAdmin |

---

## 🗄️ BANCO DE DADOS

| Componente | Valor |
|------------|-------|
| Host | localhost:3306 |
| Database | os_db |
| User | root |
| Password | root |

---

## 📦 O QUE ESTÁ INCLUÍDO

### Banco de Dados
- ✅ Tabelas do domínio sincronizadas
- ✅ Roles: SuperAdmin, Admin, Gerente, Técnico, Atendente
- ✅ Super Admin pré-criado com senha hashada
- ✅ Dados de teste: 6 clientes + 10 equipamentos + múltiplas ordens de serviço

### Serviços Docker
- ✅ **API** (.NET 9) - port 8080
- ✅ **Web** (Blazor) - port 8082
- ✅ **MySQL 8.0** - port 3306
- ✅ **Redis 7** - port 6379 (cache)
- ✅ **phpMyAdmin** - port 8081 (gestor BD)

---

## 🚀 COMO USAR

### 1️⃣ Iniciar o Projeto
```bash
docker-compose up -d
```

### 2️⃣ Acessar o Sistema
1. Abra http://localhost:8082
2. Faça login com:
   - Email: `admin@ordemservico.com`
   - Senha: `Admin@123456`
3. Pronto! Você tem acesso total ao sistema

### 3️⃣ Gerenciar o Banco de Dados
1. Abra http://localhost:8081
2. Login com `root` / `root`
3. Selecione banco `os_db`

### 4️⃣ Usar a API
```bash
# Login
curl -X POST http://localhost:8080/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@ordemservico.com",
    "senha": "Admin@123456"
  }'

# Swagger UI
http://localhost:8080/swagger/index.html
```

---

## 📝 ARQUIVOS DE REFERÊNCIA

No projeto você encontra:
- 📄 **ADMIN_CREDENTIALS.md** - Detalhes completos de acesso
- 📄 **credentials.json** - Credenciais em formato JSON
- 📄 **docker-compose.yml** - Configuração dos containers
- 📄 **.env.local** - Variáveis de ambiente
- 📄 **show-credentials.ps1** - Script para exibir credenciais

---

## 🔍 VALIDAÇÃO - Testes Realizados

✅ API respondendo em http://localhost:8080  
✅ Web acessível em http://localhost:8082  
✅ MySQL banco criado e sincronizado  
✅ Super Admin criado com senha hashada corretamente  
✅ Login testado e funcionando - Token JWT retornado  
✅ Seed de dados carregado (6 clientes + 10 equipamentos)  
✅ phpMyAdmin acessível em http://localhost:8081  
✅ Redis rodando para cache  

---

## ⚠️ NOTAS IMPORTANTES

- 🔒 Nunca compartilhe as credenciais em produção
- 🔐 A senha está hashada com ASP.NET Identity
- 📦 Todos os dados são de teste/desenvolvimento
- 🗑️ Remover containers com `docker-compose down` limpa tudo
- 💾 Dados persistem em volumes Docker
- 🔄 Se precisar resetar, execute `docker-compose down -v`

---

## 🛠️ SOLUÇÃO DE PROBLEMAS

### Login não funciona
- ✅ **Resolvido** - Seed automático agora cria a senha corretamente

### Porta já em uso
```bash
# Parar containers anteriores
docker-compose down

# Limpar tudo
docker system prune -a --volumes
```

### Banco vazio
```bash
# Forçar seed (via API)
curl -X POST http://localhost:8080/api/seed
```

---

## 📞 PRÓXIMOS PASSOS

1. ✅ Explore o sistema Web em http://localhost:8082
2. ✅ Teste os endpoints da API em http://localhost:8080/swagger
3. ✅ Verifique os dados no phpMyAdmin em http://localhost:8081
4. ✅ Customize conforme necessário

---

**Setup finalizado com sucesso!** 🎊

Para mais informações, consulte [ADMIN_CREDENTIALS.md](ADMIN_CREDENTIALS.md)
