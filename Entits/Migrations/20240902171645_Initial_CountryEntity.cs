using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entits.Migrations
{
    public partial class Initial_CountryEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "countries",
                columns: table => new
                {
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_countries", x => x.CountryID);
                });

            migrationBuilder.InsertData(
                table: "countries",
                columns: new[] { "CountryID", "CountryName" },
                values: new object[,]
                {
                    { new Guid("12e15727-d369-49a9-8b13-bc22e9362179"), "China" },
                    { new Guid("14629847-905a-4a0e-9abe-80b61655c5cb"), "Philippines" },
                    { new Guid("501c6d33-1bbe-45f1-8fbd-2275913c6218"), "China" },
                    { new Guid("56bf46a4-02b8-4693-a0f5-0a95e2218bdc"), "Thailand" },
                    { new Guid("8f30bedc-47dd-4286-8950-73d8a68e5d41"), "Palestinian Territory" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "countries");
        }
    }
}
