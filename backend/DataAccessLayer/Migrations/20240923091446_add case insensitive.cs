using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistenceLayer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addcaseinsensitive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .OldAnnotation("Npgsql:CollationDefinition:public.case_insensitive", "de-x-icu-level2,de-x-icu-level2,icu,")
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tickets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Room",
                table: "Tickets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Object",
                table: "Tickets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Tickets",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "States",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "States",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RoleAssignments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Priorities",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Comments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Buildings",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Attachments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "Attachments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldCollation: "case_insensitive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:public.case_insensitive", "de-x-icu-level2,de-x-icu-level2,icu,")
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,")
                .OldAnnotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Tickets",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Room",
                table: "Tickets",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Object",
                table: "Tickets",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tickets",
                type: "text",
                nullable: true,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Tickets",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "States",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "States",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "RoleAssignments",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Priorities",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comments",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Comments",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Buildings",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Attachments",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FileType",
                table: "Attachments",
                type: "text",
                nullable: false,
                collation: "case_insensitive",
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
