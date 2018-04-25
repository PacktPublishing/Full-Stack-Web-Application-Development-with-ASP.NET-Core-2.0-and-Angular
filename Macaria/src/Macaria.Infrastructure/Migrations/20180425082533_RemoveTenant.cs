using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Macaria.Infrastructure.Migrations
{
    public partial class RemoveTenant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Tenants_TenantId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteTag_Tenants_TenantId",
                table: "NoteTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Tenants_TenantId",
                table: "Tags");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Users_TenantId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Tags_TenantId",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_NoteTag_TenantId",
                table: "NoteTag");

            migrationBuilder.DropIndex(
                name: "IX_Notes_TenantId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "NoteTag");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Notes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Tags",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Tags",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "NoteTag",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "NoteTag",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Notes",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Notes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(nullable: false, defaultValueSql: "newsequentialid()"),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedOn = table.Column<DateTime>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.TenantId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_TenantId",
                table: "Users",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_TenantId",
                table: "Tags",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteTag_TenantId",
                table: "NoteTag",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TenantId",
                table: "Notes",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Tenants_TenantId",
                table: "Notes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteTag_Tenants_TenantId",
                table: "NoteTag",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Tenants_TenantId",
                table: "Tags",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Tenants_TenantId",
                table: "Users",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "TenantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
