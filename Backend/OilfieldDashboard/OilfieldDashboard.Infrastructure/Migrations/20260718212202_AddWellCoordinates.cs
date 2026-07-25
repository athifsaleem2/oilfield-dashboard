using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OilfieldDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWellCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Wells",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Wells",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Wells");
        }
    }
}
