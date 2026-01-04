using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alberta.ServiceDesk.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "AppBookings",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "FacilityId",
                table: "AppBookings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Purpose",
                table: "AppBookings",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "AppBookings",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppBookings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppBookings_FacilityId",
                table: "AppBookings",
                column: "FacilityId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppBookings_AppFacilities_FacilityId",
                table: "AppBookings",
                column: "FacilityId",
                principalTable: "AppFacilities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppBookings_AppFacilities_FacilityId",
                table: "AppBookings");

            migrationBuilder.DropIndex(
                name: "IX_AppBookings_FacilityId",
                table: "AppBookings");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "AppBookings");

            migrationBuilder.DropColumn(
                name: "FacilityId",
                table: "AppBookings");

            migrationBuilder.DropColumn(
                name: "Purpose",
                table: "AppBookings");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "AppBookings");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppBookings");
        }
    }
}
