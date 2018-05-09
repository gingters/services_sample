using Microsoft.EntityFrameworkCore.Migrations;

namespace Article.Services.Migrations
{
    public partial class InitialDbCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Artikel",
                columns: table => new
                {
                    ArtikelNummer = table.Column<int>(nullable: false),
                    Bezeichnung = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Artikel", x => x.ArtikelNummer);
                });

            migrationBuilder.CreateTable(
                name: "ArtikelKategorien",
                columns: table => new
                {
                    ArtikelNummer = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArtikelKategorien", x => new { x.ArtikelNummer, x.Name });
                    table.ForeignKey(
                        name: "FK_Kategorie_Artikel",
                        column: x => x.ArtikelNummer,
                        principalTable: "Artikel",
                        principalColumn: "ArtikelNummer",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Artikel",
                columns: new[] { "ArtikelNummer", "Bezeichnung" },
                values: new object[] { 1701, "Plüscheinhorn" });

            migrationBuilder.InsertData(
                table: "ArtikelKategorien",
                columns: new[] { "ArtikelNummer", "Name" },
                values: new object[] { 1701, "groß" });

            migrationBuilder.InsertData(
                table: "ArtikelKategorien",
                columns: new[] { "ArtikelNummer", "Name" },
                values: new object[] { 1701, "mittel" });

            migrationBuilder.InsertData(
                table: "ArtikelKategorien",
                columns: new[] { "ArtikelNummer", "Name" },
                values: new object[] { 1701, "klein" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ArtikelKategorien");

            migrationBuilder.DropTable(
                name: "Artikel");
        }
    }
}
