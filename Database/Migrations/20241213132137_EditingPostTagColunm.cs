using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class EditingPostTagColunm : Migration
    {
        /// <inheritdoc />
        
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostTags_Users_UserId",
                table: "PostTags");

            migrationBuilder.DropIndex(
                name: "IX_PostTags_UserId",
                table: "PostTags");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "PostTags");

            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "PostTags",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TagId",
                table: "PostTags",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "PostTags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PostTags_UserId",
                table: "PostTags",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostTags_Users_UserId",
                table: "PostTags",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
