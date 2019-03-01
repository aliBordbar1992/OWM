using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OWM.Data.Migrations
{
    public partial class _10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamOccupations",
                table: "TeamOccupations");

            migrationBuilder.DropIndex(
                name: "IX_TeamOccupations_TeamId",
                table: "TeamOccupations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "TeamOccupations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamOccupations",
                table: "TeamOccupations",
                columns: new[] { "TeamId", "OccupationId" });

            migrationBuilder.CreateTable(
                name: "TeamMembers",
                columns: table => new
                {
                    TeamId = table.Column<int>(nullable: false),
                    ProfileId = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    IsCreator = table.Column<bool>(nullable: false),
                    KickedOut = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamMembers", x => new { x.TeamId, x.ProfileId });
                    table.ForeignKey(
                        name: "FK_TeamMembers_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamMembers_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamMembers_ProfileId",
                table: "TeamMembers",
                column: "ProfileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TeamOccupations",
                table: "TeamOccupations");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "TeamOccupations",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TeamOccupations",
                table: "TeamOccupations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_TeamOccupations_TeamId",
                table: "TeamOccupations",
                column: "TeamId");
        }
    }
}
