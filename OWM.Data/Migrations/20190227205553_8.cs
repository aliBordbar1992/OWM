using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OWM.Data.Migrations
{
    public partial class _8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "TeamOccupations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    TeamId = table.Column<int>(nullable: false),
                    OccupationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamOccupations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeamOccupations_Occupations_OccupationId",
                        column: x => x.OccupationId,
                        principalTable: "Occupations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeamOccupations_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamOccupations_OccupationId",
                table: "TeamOccupations",
                column: "OccupationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeamOccupations_TeamId",
                table: "TeamOccupations",
                column: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamOccupations");

            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "Occupations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Occupations_TeamId",
                table: "Occupations",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Occupations_Teams_TeamId",
                table: "Occupations",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
