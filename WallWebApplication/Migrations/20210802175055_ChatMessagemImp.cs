using Microsoft.EntityFrameworkCore.Migrations;

namespace WallWebApplication.Migrations
{
    public partial class ChatMessagemImp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PrivateMessageId",
                table: "Message",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "MessageBody",
                table: "Message",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RecipientUserID",
                table: "Chat",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SenderUserID",
                table: "Chat",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecipientUserID",
                table: "Chat");

            migrationBuilder.DropColumn(
                name: "SenderUserID",
                table: "Chat");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Message",
                newName: "PrivateMessageId");

            migrationBuilder.AlterColumn<string>(
                name: "MessageBody",
                table: "Message",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
