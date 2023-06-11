using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _6_5ApiHomework.Data.Migrations
{
    public partial class Third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dislikes",
                table: "Jokes");

            migrationBuilder.RenameColumn(
                name: "Likes",
                table: "Jokes",
                newName: "OriginalId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OriginalId",
                table: "Jokes",
                newName: "Likes");

            migrationBuilder.AddColumn<int>(
                name: "Dislikes",
                table: "Jokes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
