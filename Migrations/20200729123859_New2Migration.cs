using Microsoft.EntityFrameworkCore.Migrations;

namespace StellarisSaveParser.Migrations
{
    public partial class New2Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PopKey",
                table: "Pops",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
