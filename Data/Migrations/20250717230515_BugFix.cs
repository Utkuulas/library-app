using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class BugFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoan_AspNetUsers_UserId",
                table: "BookLoan");

            migrationBuilder.DropForeignKey(
                name: "FK_BookLoan_Books_BookId",
                table: "BookLoan");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_AspNetUsers_UserId",
                table: "Request");

            migrationBuilder.DropForeignKey(
                name: "FK_Request_Books_BookId",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Request",
                table: "Request");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookLoan",
                table: "BookLoan");

            migrationBuilder.RenameTable(
                name: "Request",
                newName: "Requests");

            migrationBuilder.RenameTable(
                name: "BookLoan",
                newName: "BookLoans");

            migrationBuilder.RenameIndex(
                name: "IX_Request_UserId",
                table: "Requests",
                newName: "IX_Requests_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Request_BookId",
                table: "Requests",
                newName: "IX_Requests_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookLoan_UserId",
                table: "BookLoans",
                newName: "IX_BookLoans_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookLoan_BookId",
                table: "BookLoans",
                newName: "IX_BookLoans_BookId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requests",
                table: "Requests",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookLoans",
                table: "BookLoans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_AspNetUsers_UserId",
                table: "BookLoans",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoans_Books_BookId",
                table: "BookLoans",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_AspNetUsers_UserId",
                table: "Requests",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_Books_BookId",
                table: "Requests",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_AspNetUsers_UserId",
                table: "BookLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_BookLoans_Books_BookId",
                table: "BookLoans");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_AspNetUsers_UserId",
                table: "Requests");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_Books_BookId",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requests",
                table: "Requests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BookLoans",
                table: "BookLoans");

            migrationBuilder.RenameTable(
                name: "Requests",
                newName: "Request");

            migrationBuilder.RenameTable(
                name: "BookLoans",
                newName: "BookLoan");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_UserId",
                table: "Request",
                newName: "IX_Request_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Requests_BookId",
                table: "Request",
                newName: "IX_Request_BookId");

            migrationBuilder.RenameIndex(
                name: "IX_BookLoans_UserId",
                table: "BookLoan",
                newName: "IX_BookLoan_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_BookLoans_BookId",
                table: "BookLoan",
                newName: "IX_BookLoan_BookId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Books",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Request",
                table: "Request",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BookLoan",
                table: "BookLoan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoan_AspNetUsers_UserId",
                table: "BookLoan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BookLoan_Books_BookId",
                table: "BookLoan",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Request_AspNetUsers_UserId",
                table: "Request",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Books_BookId",
                table: "Request",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }
    }
}
