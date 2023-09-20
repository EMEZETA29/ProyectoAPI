using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProyectoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRegistros : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vehiculo",
                columns: new[] { "Id", "CarroceriaId", "Color", "MarcaId", "ModeloId", "Patente" },
                values: new object[,]
                {
                    { 1, 1, "Celeste", 1, 1, "RPRP42" },
                    { 2, 1, "Celeste", 1, 1, "RPRP50" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vehiculo",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehiculo",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
