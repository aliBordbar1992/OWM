using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OWM.Data.Migrations
{
    public partial class _14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "TeamInvitations");

            migrationBuilder.AddColumn<Guid>(
                name: "TeamGuid",
                table: "TeamInvitations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamGuid",
                table: "TeamInvitations");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "TeamInvitations",
                nullable: false,
                defaultValue: 0);
        }
    }
}
