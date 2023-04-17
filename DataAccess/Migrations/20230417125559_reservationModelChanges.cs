using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace cochesApi.Migrations
{
    /// <inheritdoc />
    public partial class reservationModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "hasGPS",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isInternational",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "numberOfDrivers",
                table: "Reservations",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "hasGPS",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "isInternational",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "numberOfDrivers",
                table: "Reservations");
        }
    }
}
