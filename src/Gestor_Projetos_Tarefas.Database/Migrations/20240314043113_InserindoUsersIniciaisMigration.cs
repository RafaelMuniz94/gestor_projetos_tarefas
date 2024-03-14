using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestor_Projetos_Tarefas.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class InserindoUsersIniciaisMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            {
                migrationBuilder.InsertData(table: "users",
                columns: new[] { "ID", "Name", "Role", "Email" },
                values: new object[,]
                {

                    {new Guid("7c1bc0ef-123f-4e97-b1e6-8dd36be07643"),"Analista",1, "analista@empresa.com" },
                    {new Guid("aea0c594-7e38-4826-b25c-42f4bb4bdc35"),"Gerente",0, "gerente@empresa.com" },

                });
            }
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "ID",
                keyValues: null);
        }
    }
}
