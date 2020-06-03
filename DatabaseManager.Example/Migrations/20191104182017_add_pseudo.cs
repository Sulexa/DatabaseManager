using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace MigrationManager.Example.Migrations
{
    public partial class add_pseudo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pseudo",
                table: "Users",
                nullable: true);
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (environment == "Development")
            {
                migrationBuilder.InsertData(
                    table: "Users",
                    columns: new string[] { "Id", "Name", "Pseudo" },
                    values: new object[] { 1, "José", "Le conquérant" }
                    );
            }
            if (environment == "Production")
            {
                migrationBuilder.InsertData(
                    table: "Users",
                    columns: new string[] { "Id", "Name", "Pseudo" },
                    values: new object[] { 1, "José", "Le producteur" }
                    );
            }

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pseudo",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1
                );
        }
    }
}
