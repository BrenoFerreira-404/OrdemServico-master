using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiTenancy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Criar tabela Tenants
            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "CHAR(36)", nullable: false, collation: "utf8mb4_general_ci"),
                    nome = table.Column<string>(type: "VARCHAR(150)", maxLength: 150, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    slug = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    documento = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "VARCHAR(256)", maxLength: 256, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    endereco = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cidade = table.Column<string>(type: "VARCHAR(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<string>(type: "VARCHAR(2)", maxLength: 2, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    logo_url = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    plano_atual = table.Column<int>(type: "INT", nullable: false),
                    ativo = table.Column<bool>(type: "TINYINT(1)", nullable: false),
                    motivo_desativacao = table.Column<string>(type: "VARCHAR(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_expiracao = table.Column<DateTime>(type: "DATETIME(6)", nullable: true),
                    created_at = table.Column<DateTime>(type: "DATETIME(6)", nullable: false),
                    updated_at = table.Column<DateTime>(type: "DATETIME(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenants", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Criar índice único para slug
            migrationBuilder.CreateIndex(
                name: "IX_tenants_slug",
                table: "tenants",
                column: "slug",
                unique: true);

            // Adicionar coluna tenant_id às tabelas existentes
            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "usuarios",
                type: "CHAR(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "utf8mb4_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "ordens_servico",
                type: "CHAR(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "utf8mb4_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "equipamentos",
                type: "CHAR(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "utf8mb4_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "tenant_id",
                table: "clientes",
                type: "CHAR(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "utf8mb4_general_ci");

            // Criar índices para tenant_id
            migrationBuilder.CreateIndex(
                name: "IX_clientes_tenant_id",
                table: "clientes",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_equipamentos_tenant_id",
                table: "equipamentos",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_ordens_servico_tenant_id",
                table: "ordens_servico",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_tenant_id",
                table: "usuarios",
                column: "tenant_id");

            // Adicionar chaves estrangeiras
            migrationBuilder.AddForeignKey(
                name: "FK_clientes_tenants_tenant_id",
                table: "clientes",
                column: "tenant_id",
                principalTable: "tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_equipamentos_tenants_tenant_id",
                table: "equipamentos",
                column: "tenant_id",
                principalTable: "tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ordens_servico_tenants_tenant_id",
                table: "ordens_servico",
                column: "tenant_id",
                principalTable: "tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_usuarios_tenants_tenant_id",
                table: "usuarios",
                column: "tenant_id",
                principalTable: "tenants",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover chaves estrangeiras
            migrationBuilder.DropForeignKey(
                name: "FK_clientes_tenants_tenant_id",
                table: "clientes");

            migrationBuilder.DropForeignKey(
                name: "FK_equipamentos_tenants_tenant_id",
                table: "equipamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_ordens_servico_tenants_tenant_id",
                table: "ordens_servico");

            migrationBuilder.DropForeignKey(
                name: "FK_usuarios_tenants_tenant_id",
                table: "usuarios");

            // Remover índices
            migrationBuilder.DropIndex(
                name: "IX_usuarios_tenant_id",
                table: "usuarios");

            migrationBuilder.DropIndex(
                name: "IX_ordens_servico_tenant_id",
                table: "ordens_servico");

            migrationBuilder.DropIndex(
                name: "IX_equipamentos_tenant_id",
                table: "equipamentos");

            migrationBuilder.DropIndex(
                name: "IX_clientes_tenant_id",
                table: "clientes");

            // Remover colunas
            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "usuarios");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "ordens_servico");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "equipamentos");

            migrationBuilder.DropColumn(
                name: "tenant_id",
                table: "clientes");

            // Remover tabela Tenants
            migrationBuilder.DropTable(
                name: "tenants");
        }
    }
}
