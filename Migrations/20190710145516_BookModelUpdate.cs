using Microsoft.EntityFrameworkCore.Migrations;

namespace book_app_server.Migrations
{
    public partial class BookModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Bookshelf",
                newName: "BookShelfName");

            migrationBuilder.AddColumn<string>(
                name: "Bookshelf",
                table: "Book",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bookshelf",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "BookShelfName",
                table: "Bookshelf",
                newName: "Name");
        }
    }
}
