using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PersistenceLayer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Adddeletebehavior : Migration
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
                name: "FK_Tickets_Priorities_PriorityName",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_States_StateId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PriorityName",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "PriorityName",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "PriorityValue",
                table: "Tickets",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Priorities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PriorityValue",
                table: "Tickets",
                column: "PriorityValue");

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
                name: "FK_Tickets_Priorities_PriorityValue",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_States_StateId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_PriorityValue",
                table: "Tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities");

            migrationBuilder.DropColumn(
                name: "PriorityValue",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "PriorityName",
                table: "Tickets",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Value",
                table: "Priorities",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PriorityName",
                table: "Tickets",
                column: "PriorityName");

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
                name: "FK_Tickets_Priorities_PriorityName",
                table: "Tickets",
                column: "PriorityName",
                principalTable: "Priorities",
                principalColumn: "Name");

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
