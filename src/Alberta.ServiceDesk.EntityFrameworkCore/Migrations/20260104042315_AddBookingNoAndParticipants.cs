using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alberta.ServiceDesk.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingNoAndParticipants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppBookings_FacilityId",
                table: "AppBookings");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "AppBookings",
                type: "character varying(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "BookingNo",
                table: "AppBookings",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfParticipants",
                table: "AppBookings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppBookings_BookingNo",
                table: "AppBookings",
                column: "BookingNo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppBookings_FacilityId_StartTime",
                table: "AppBookings",
                columns: new[] { "FacilityId", "StartTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppBookings_BookingNo",
                table: "AppBookings");

            migrationBuilder.DropIndex(
                name: "IX_AppBookings_FacilityId_StartTime",
                table: "AppBookings");

            migrationBuilder.DropColumn(
                name: "BookingNo",
                table: "AppBookings");

            migrationBuilder.DropColumn(
                name: "NumberOfParticipants",
                table: "AppBookings");

            migrationBuilder.AlterColumn<string>(
                name: "Purpose",
                table: "AppBookings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(256)",
                oldMaxLength: 256);

            migrationBuilder.CreateIndex(
                name: "IX_AppBookings_FacilityId",
                table: "AppBookings",
                column: "FacilityId");
        }
    }
}
