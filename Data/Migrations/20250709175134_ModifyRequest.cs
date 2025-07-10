using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_BookLoan_BookLoanId",
                table: "Request");

            migrationBuilder.RenameColumn(
                name: "BookLoanId",
                table: "Request",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Request_BookLoanId",
                table: "Request",
                newName: "IX_Request_BookId");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Request",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_Books_BookId",
                table: "Request",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Request_Books_BookId",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Request");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Request",
                newName: "BookLoanId");

            migrationBuilder.RenameIndex(
                name: "IX_Request_BookId",
                table: "Request",
                newName: "IX_Request_BookLoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Request_BookLoan_BookLoanId",
                table: "Request",
                column: "BookLoanId",
                principalTable: "BookLoan",
                principalColumn: "Id");
        }
    }
}
