using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class GuestbookundCommentVote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuestbookEntry_Users_UserId",
                table: "GuestbookEntry");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuestbookEntry",
                table: "GuestbookEntry");

            migrationBuilder.RenameTable(
                name: "GuestbookEntry",
                newName: "GuestbookEntries");

            migrationBuilder.RenameIndex(
                name: "IX_GuestbookEntry_UserId",
                table: "GuestbookEntries",
                newName: "IX_GuestbookEntries_UserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "VotedAt",
                table: "CommentVotes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuestbookEntries",
                table: "GuestbookEntries",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestbookEntries_Users_UserId",
                table: "GuestbookEntries",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuestbookEntries_Users_UserId",
                table: "GuestbookEntries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GuestbookEntries",
                table: "GuestbookEntries");

            migrationBuilder.DropColumn(
                name: "VotedAt",
                table: "CommentVotes");

            migrationBuilder.RenameTable(
                name: "GuestbookEntries",
                newName: "GuestbookEntry");

            migrationBuilder.RenameIndex(
                name: "IX_GuestbookEntries_UserId",
                table: "GuestbookEntry",
                newName: "IX_GuestbookEntry_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GuestbookEntry",
                table: "GuestbookEntry",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestbookEntry_Users_UserId",
                table: "GuestbookEntry",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
