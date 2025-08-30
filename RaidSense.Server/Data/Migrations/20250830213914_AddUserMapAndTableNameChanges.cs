using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RaidSense.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddUserMapAndTableNameChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bases_Maps_MapId",
                table: "Bases");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_AspNetUsers_OwnerId",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Servers_ServerId",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_MapUsers_Maps_MapId",
                table: "MapUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_AspNetUsers_UserId",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_MapUsers_MapId",
                table: "MapUsers");

            migrationBuilder.DropIndex(
                name: "IX_Maps_OwnerId",
                table: "Maps");

            migrationBuilder.DropIndex(
                name: "IX_Maps_ServerId",
                table: "Maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Servers",
                table: "Servers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "Maps");

            migrationBuilder.RenameTable(
                name: "Servers",
                newName: "RustServers");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                newName: "RefreshTokens");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");

            migrationBuilder.AlterColumn<int>(
                name: "MapId",
                table: "MapUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "MapId",
                table: "Bases",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "MapId",
                table: "RustServers",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RustServers",
                table: "RustServers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "UserMaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MapId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserMaps_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMaps_Maps_MapId",
                        column: x => x.MapId,
                        principalTable: "Maps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MapUsers_MapId_UserId",
                table: "MapUsers",
                columns: new[] { "MapId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RustServers_MapId",
                table: "RustServers",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMaps_MapId",
                table: "UserMaps",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMaps_OwnerId",
                table: "UserMaps",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bases_UserMaps_MapId",
                table: "Bases",
                column: "MapId",
                principalTable: "UserMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapUsers_UserMaps_MapId",
                table: "MapUsers",
                column: "MapId",
                principalTable: "UserMaps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RustServers_Maps_MapId",
                table: "RustServers",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bases_UserMaps_MapId",
                table: "Bases");

            migrationBuilder.DropForeignKey(
                name: "FK_MapUsers_UserMaps_MapId",
                table: "MapUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_AspNetUsers_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RustServers_Maps_MapId",
                table: "RustServers");

            migrationBuilder.DropTable(
                name: "UserMaps");

            migrationBuilder.DropIndex(
                name: "IX_MapUsers_MapId_UserId",
                table: "MapUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RustServers",
                table: "RustServers");

            migrationBuilder.DropIndex(
                name: "IX_RustServers_MapId",
                table: "RustServers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "MapId",
                table: "RustServers");

            migrationBuilder.RenameTable(
                name: "RustServers",
                newName: "Servers");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "RefreshToken");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "MapId",
                table: "MapUsers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Maps",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ServerId",
                table: "Maps",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MapId",
                table: "Bases",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Servers",
                table: "Servers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_MapUsers_MapId",
                table: "MapUsers",
                column: "MapId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_OwnerId",
                table: "Maps",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_ServerId",
                table: "Maps",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bases_Maps_MapId",
                table: "Bases",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_AspNetUsers_OwnerId",
                table: "Maps",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Servers_ServerId",
                table: "Maps",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MapUsers_Maps_MapId",
                table: "MapUsers",
                column: "MapId",
                principalTable: "Maps",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_AspNetUsers_UserId",
                table: "RefreshToken",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
