using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OilfieldDashboard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddThresholdsAlertsWorkOrderFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedTo",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "WorkOrders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "WorkOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
     name: "MaxPressure",
     table: "Wells",
     type: "float",
     nullable: false,
     defaultValue: 2450.0);

            migrationBuilder.AddColumn<double>(
                name: "MaxTemperature",
                table: "Wells",
                type: "float",
                nullable: false,
                defaultValue: 190.0);

            migrationBuilder.AddColumn<double>(
                name: "MinFlowRate",
                table: "Wells",
                type: "float",
                nullable: false,
                defaultValue: 150.0);

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WellId = table.Column<int>(type: "int", nullable: false),
                    Metric = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<double>(type: "float", nullable: false),
                    Threshold = table.Column<double>(type: "float", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsResolved = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alerts_Wells_WellId",
                        column: x => x.WellId,
                        principalTable: "Wells",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_WellId",
                table: "Alerts",
                column: "WellId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropColumn(
                name: "AssignedTo",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "WorkOrders");

            migrationBuilder.DropColumn(
                name: "MaxPressure",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "MaxTemperature",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "MinFlowRate",
                table: "Wells");
        }
    }
}
