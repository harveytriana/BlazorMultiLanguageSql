using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorMultilanguage.Server.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextResources",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EN = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ES = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PT = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RU = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    NO = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IT = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextResources", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextResources");
        }
    }
}
