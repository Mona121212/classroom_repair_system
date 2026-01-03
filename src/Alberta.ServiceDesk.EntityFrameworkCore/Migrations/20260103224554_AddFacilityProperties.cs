using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alberta.ServiceDesk.Migrations
{
    /// <inheritdoc />
    public partial class AddFacilityProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "AppFacilities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "AppFacilities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AppFacilities",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OwnerUnit",
                table: "AppFacilities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequiresApproval",
                table: "AppFacilities",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "AppFacilities",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "AppFacilities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "AppFacilities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AppFacilities");

            migrationBuilder.DropColumn(
                name: "OwnerUnit",
                table: "AppFacilities");

            migrationBuilder.DropColumn(
                name: "RequiresApproval",
                table: "AppFacilities");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "AppFacilities");
        }
    }
}
