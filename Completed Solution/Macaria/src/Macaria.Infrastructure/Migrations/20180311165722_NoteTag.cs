using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Macaria.Infrastructure.Migrations
{
    public partial class NoteTag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "NoteTag",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "NoteTag",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "NoteTag",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedOn",
                table: "NoteTag",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "NoteTag",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "NoteTag",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_TenantId",
                table: "NoteTag",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTag_Tenants_TenantId",
                table: "NoteTag",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteTag_Tenants_TenantId",
                table: "NoteTag");

            migrationBuilder.DropIndex(
                name: "IX_NoteTag_TenantId",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "LastModifiedOn",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "NoteTag");
        }
    }
}
