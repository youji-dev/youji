using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersistenceLayer.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Rename_PrefferedLcid_To_PrefferedEmailLcid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreferredLcid",
                table: "Users",
                newName: "PreferredEmailLcid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PreferredEmailLcid",
                table: "Users",
                newName: "PreferredLcid");
        }
    }
}
