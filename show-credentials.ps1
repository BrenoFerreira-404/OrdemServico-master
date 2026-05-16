# =============================================
# Script para exibir Credenciais de Acesso
# OrdemServico - Ambiente Docker
# =============================================

Write-Host "`n" -ForegroundColor White
Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║         🔐 CREDENCIAIS - ORDEM SERVIÇO             ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host "`n"

Write-Host "✅ SUPER ADMIN - Acesso 100%" -ForegroundColor Green
Write-Host "━━━━━━━━━━━━━━━━━━━━━━━━━━━━━" -ForegroundColor Green
Write-Host "📧 Email:  " -ForegroundColor White -NoNewline; Write-Host "superadmin@sistema.com" -ForegroundColor Yellow
Write-Host "🔑 Senha:  " -ForegroundColor White -NoNewline; Write-Host "Admin@123456" -ForegroundColor Yellow
Write-Host "👤 Role:   " -ForegroundColor White -NoNewline; Write-Host "SuperAdmin" -ForegroundColor Yellow
Write-Host "`n"

Write-Host "🌐 URLS DE ACESSO" -ForegroundColor Cyan
Write-Host "━━━━━━━━━━━━━━━━━" -ForegroundColor Cyan
Write-Host "🖥️  Web (Blazor):  " -ForegroundColor White -NoNewline; Write-Host "http://localhost:8082" -ForegroundColor Blue
Write-Host "⚙️  API:            " -ForegroundColor White -NoNewline; Write-Host "http://localhost:8080" -ForegroundColor Blue
Write-Host "📊 phpMyAdmin:    " -ForegroundColor White -NoNewline; Write-Host "http://localhost:8081" -ForegroundColor Blue
Write-Host "`n"

Write-Host "🗄️  BANCO DE DADOS" -ForegroundColor Magenta
Write-Host "━━━━━━━━━━━━━━━━━" -ForegroundColor Magenta
Write-Host "🖥️  Host:     " -ForegroundColor White -NoNewline; Write-Host "localhost:3306" -ForegroundColor Yellow
Write-Host "📁 Banco:    " -ForegroundColor White -NoNewline; Write-Host "os_db" -ForegroundColor Yellow
Write-Host "👤 Usuário:  " -ForegroundColor White -NoNewline; Write-Host "root" -ForegroundColor Yellow
Write-Host "🔑 Senha:    " -ForegroundColor White -NoNewline; Write-Host "root" -ForegroundColor Yellow
Write-Host "`n"

Write-Host "📝 DOCUMENTAÇÃO" -ForegroundColor White
Write-Host "━━━━━━━━━━━━━━" -ForegroundColor White
Write-Host "📄 Detalhes completos em: " -ForegroundColor White -NoNewline; Write-Host "ADMIN_CREDENTIALS.md" -ForegroundColor Cyan
Write-Host "📋 JSON com credenciais:  " -ForegroundColor White -NoNewline; Write-Host "credentials.json" -ForegroundColor Cyan
Write-Host "`n"

Write-Host "╔════════════════════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "║ ✨ Pronto para acessar! Cole as credenciais acima  ║" -ForegroundColor Cyan
Write-Host "╚════════════════════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host "`n"

# Opção para copiar para clipboard
$choice = Read-Host "Deseja copiar o email para o clipboard? (S/N)"
if ($choice -eq 'S' -or $choice -eq 's') {
    "superadmin@sistema.com" | Set-Clipboard
    Write-Host "✅ Email copiado!" -ForegroundColor Green
}

$choice = Read-Host "Deseja copiar a senha para o clipboard? (S/N)"
if ($choice -eq 'S' -or $choice -eq 's') {
    "Admin@123456" | Set-Clipboard
    Write-Host "✅ Senha copiada!" -ForegroundColor Green
}
