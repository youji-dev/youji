using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace PersistenceLayer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Priorities_PriorityName",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities",
                column: "Value");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PriorityValue",
                table: "Tickets",
                column: "PriorityValue");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Priorities_PriorityValue",
                table: "Tickets",
                column: "PriorityValue",
                principalTable: "Priorities",
                principalColumn: "Value");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Priorities_PriorityValue",
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

            migrationBuilder.AddPrimaryKey(
                name: "PK_Priorities",
                table: "Priorities",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PriorityName",
                table: "Tickets",
                column: "PriorityName");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Priorities_PriorityName",
                table: "Tickets",
                column: "PriorityName",
                principalTable: "Priorities",
                principalColumn: "Name");
        }
    }
}
