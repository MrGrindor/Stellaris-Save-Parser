using Microsoft.EntityFrameworkCore.Migrations;

namespace StellarisSaveParser.Migrations
{
    public partial class New3Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PopGameId",
                table: "Pops",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<float>(
                name: "Stability",
                table: "Planets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Owner",
                table: "Planets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Migration",
                table: "Planets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "Crime",
                table: "Planets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Controller",
                table: "Planets",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanetGameId",
                table: "Planets",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GalacticObjectGameId",
                table: "GalacticObjects",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BuildingGameId",
                table: "Buildings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Hyperlane",
                columns: table => new
                {
                    hyperlaneId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    targetId = table.Column<int>(nullable: false),
                    distance = table.Column<int>(nullable: false),
                    GalacticObjectID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hyperlane", x => x.hyperlaneId);
                    table.ForeignKey(
                        name: "FK_Hyperlane_GalacticObjects_GalacticObjectID",
                        column: x => x.GalacticObjectID,
                        principalTable: "GalacticObjects",
                        principalColumn: "GalacticObjectID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Hyperlane_GalacticObjectID",
                table: "Hyperlane",
                column: "GalacticObjectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hyperlane");

            migrationBuilder.DropColumn(
                name: "PopGameId",
                table: "Pops");

            migrationBuilder.DropColumn(
                name: "PlanetGameId",
                table: "Planets");

            migrationBuilder.DropColumn(
                name: "GalacticObjectGameId",
                table: "GalacticObjects");

            migrationBuilder.DropColumn(
                name: "BuildingGameId",
                table: "Buildings");

            migrationBuilder.AlterColumn<string>(
                name: "Stability",
                table: "Planets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<string>(
                name: "Owner",
                table: "Planets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<string>(
                name: "Migration",
                table: "Planets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<string>(
                name: "Crime",
                table: "Planets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<string>(
                name: "Controller",
                table: "Planets",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
