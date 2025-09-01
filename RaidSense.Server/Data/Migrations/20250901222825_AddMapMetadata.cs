using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaidSense.Server.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddMapMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isStaging",
                table: "Maps",
                newName: "IsStaging");

            migrationBuilder.RenameColumn(
                name: "isCustomMap",
                table: "Maps",
                newName: "IsCustomMap");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Maps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RawImageUrl",
                table: "Maps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Maps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Maps",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "RawImageUrl",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Maps");

            migrationBuilder.RenameColumn(
                name: "IsStaging",
                table: "Maps",
                newName: "isStaging");

            migrationBuilder.RenameColumn(
                name: "IsCustomMap",
                table: "Maps",
                newName: "isCustomMap");
        }
    }
}
