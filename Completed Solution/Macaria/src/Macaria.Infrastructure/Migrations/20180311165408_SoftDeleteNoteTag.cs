using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Macaria.Infrastructure.Migrations
{
    public partial class SoftDeleteNoteTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NoteTag",
                table: "NoteTag");

            migrationBuilder.DropIndex(
                name: "IX_NoteTag_NoteId",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "NoteTagId",
                table: "NoteTag");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "NoteTag",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoteTag",
                table: "NoteTag",
                columns: new[] { "NoteId", "TagId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_NoteTag",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "NoteTag");

            migrationBuilder.AddColumn<int>(
                name: "NoteTagId",
                table: "NoteTag",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoteTag",
                table: "NoteTag",
                column: "NoteTagId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_NoteId",
                table: "NoteTag",
                column: "NoteId");
        }
    }
}
