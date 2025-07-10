using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRequest2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Request",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Request_UserId",
                table: "Request",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_AspNetUsers_UserId",
                table: "Request",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_AspNetUsers_UserId",
                table: "Request");

            migrationBuilder.DropIndex(
                name: "IX_Request_UserId",
                table: "Request");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
