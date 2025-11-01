using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace animeSamaInfo.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Animes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: true),
                    Studio = table.Column<string>(type: "TEXT", nullable: true),
                    EpisodeTotal = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberSeason = table.Column<int>(type: "INTEGER", nullable: false),
                    EpisodeSeen = table.Column<int>(type: "INTEGER", nullable: false),
                    SeasonSeen = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    AnimeTypes = table.Column<string>(type: "TEXT", nullable: true),
                    AddDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animes", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Animes");
        }
    }
}
