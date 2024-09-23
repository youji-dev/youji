using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistenceLayer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updatecascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Tickets_TicketId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Buildings_BuildingId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Priorities_PriorityValue",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_States_StateId",
                table: "Tickets");

            migrationBuilder.AlterColumn<Guid>(
                name: "TicketId",
                table: "Comments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TicketId",
                table: "Attachments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Tickets_TicketId",
                table: "Attachments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Buildings_BuildingId",
                table: "Tickets",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Priorities_PriorityValue",
                table: "Tickets",
                column: "PriorityValue",
                principalTable: "Priorities",
                principalColumn: "Value",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_States_StateId",
                table: "Tickets",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Tickets_TicketId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Buildings_BuildingId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Priorities_PriorityValue",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_States_StateId",
                table: "Tickets");

            migrationBuilder.AlterColumn<Guid>(
                name: "TicketId",
                table: "Comments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<Guid>(
                name: "TicketId",
                table: "Attachments",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Tickets_TicketId",
                table: "Attachments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Tickets_TicketId",
                table: "Comments",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Buildings_BuildingId",
                table: "Tickets",
                column: "BuildingId",
                principalTable: "Buildings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Priorities_PriorityValue",
                table: "Tickets",
                column: "PriorityValue",
                principalTable: "Priorities",
                principalColumn: "Value");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_States_StateId",
                table: "Tickets",
                column: "StateId",
                principalTable: "States",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
