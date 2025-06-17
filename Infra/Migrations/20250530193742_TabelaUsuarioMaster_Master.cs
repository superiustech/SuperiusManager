using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations.ApplicationDbContextMasterMigrations
{
    /// <inheritdoc />
    public partial class TabelaUsuarioMaster_Master : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "USUARIO_MASTER",
                columns: table => new
                {
                    sCdUsuario = table.Column<string>(type: "text", nullable: false),
                    sSenha = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIO_MASTER", x => x.sCdUsuario);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USUARIO_MASTER");
        }
    }
}
