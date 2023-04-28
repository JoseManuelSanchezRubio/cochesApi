using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cochesApi.Migrations
{
    /// <inheritdoc />
    public partial class PricesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "TypeCars",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "TypeCars");
        }
    }
}
